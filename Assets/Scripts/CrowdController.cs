using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KargaGames.Drawing;

public class CrowdController : MonoBehaviour
{
    public Stickman stickmanPrefab;
    public StickmanPosition positionPrefab;
    public ColumnHeader ColumnPrefab;
    public float HorizontalDistanceBetweenStickmans;
    public float VerticalDistanceBetweenStickmans;

    public GameObject PositionsParent;
    public GameObject StickmansParent;

    protected SplineComputer _spline;
    protected List<List<StickmanPosition>> StickmanPositions;
    protected List<Stickman> StickmanList;
    protected GameSceneManager gameSceneManager;
    protected SplineByDrawing splineDrawer;
    protected SplineGenerator splineGenerator;
    protected GameObject drawingUI;
    protected RunnerPlayerController playerController;

    protected CameraController cameraController;

    float totalDistance;

    bool FixWorking = false;
    float lastFixTime = 0;
    int fixCommand = 0;

    public Vector3 velocity;
    Vector3 lastPosition;
    // Start is called before the first frame update
    public virtual void Start()
    {
        gameSceneManager = FindObjectOfType<GameSceneManager>();
        splineDrawer = FindObjectOfType<SplineByDrawing>();
        splineGenerator = FindObjectOfType<SplineGenerator>();
        drawingUI = GameObject.FindGameObjectWithTag("DrawingArea");

        cameraController = FindObjectOfType<CameraController>();
        playerController = FindObjectOfType<RunnerPlayerController>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(StickmanList.Count == 0 && !GameSceneManager.gameOver)
        {
            Fail();
        }

        lastFixTime += Time.deltaTime;

        if(lastFixTime > 0.5f)
        {
            FixWorking = false;
        }

        CalculateVelocity();
    }
    public void CalculateVelocity()
    {
        velocity = transform.position - lastPosition;
        lastPosition = transform.position;
    }
    public virtual void Fail()
    {
        gameSceneManager.LevelFailed();
        splineDrawer.enabled = false;
        drawingUI.SetActive(false);
        playerController.speed = 0;
    }

    public virtual void SetupController()
    {
        _spline = GetComponent<SplineComputer>();

        SplineGenerator _splineGenerator = FindObjectOfType<SplineGenerator>();
        if (_splineGenerator != null)
        {
            _splineGenerator.SetCrowdController(this);
        }

        StickmanPositions = new List<List<StickmanPosition>>();
        StickmanList = new List<Stickman>();
    }

    public virtual Stickman GetLastMan()
    {
        return StickmanList[StickmanList.Count-1];
    }

    public virtual void AddStickman(Stickman newStickman, bool reposition = true)
    {
        StickmanList.Add(newStickman);
        newStickman.transform.SetParent(StickmansParent.transform);
        newStickman.Join(this);

        if (reposition)
        {
            RePositionStickmans();
        }
    }

    public virtual void RemoveStickman(Stickman stickman, bool reposition = false, float repositionDelay = 0f)
    {

        Vector2Int coord = stickman.desiredPosition.ListCoordinate;

        StickmanList.Remove(stickman);

        if (reposition)
        {
            fixCommand++;

            float commandIndex = fixCommand;
            StartCoroutine(GeneralFunctions.executeAfterSec(() => {

                if(commandIndex == fixCommand)
                {
                    //RePositionStickmans();

                    if (!FixWorking)
                    {
                        FixWorking = true;
                        lastFixTime = 0;

                        StickmanPositions.RemoveAll(item => item == null);
                        StickmanPositions.RemoveAll(item => item.Count == 0);


                        FixColumns();
                        FixRows();
                        TweenStickmans();

                    }


                }



            },repositionDelay));
        }
    }


    public void FixColumns()
    {
        for (int i = 0; i < StickmanPositions.Count; i++)
        {
            FixColumn(i);
        }
    }

    public void FixColumn(int columnIndex)
    {

        StickmanPositions[columnIndex].RemoveAll(item => item == null);

        for(int i = 0; i< StickmanPositions[columnIndex].Count; i++)
        {
            if(StickmanPositions[columnIndex][i].ListCoordinate.y != i)
            {

                StickmanPositions[columnIndex][i].ListCoordinate.y = i;
                StickmanPositions[columnIndex][i].Height = i * VerticalDistanceBetweenStickmans;

            }
        }
    }
    
    public void FixRows()
    {

            StickmanPositions.RemoveAll(item => item == null);
            StickmanPositions.RemoveAll(item => item.Count == 0);

            for (int i = 0; i < StickmanPositions.Count; i++)
            {
                foreach (StickmanPosition position in StickmanPositions[i])
                {
                    if (position.ListCoordinate.x != i)
                    {
                        position.ListCoordinate.x = i;

                        float mod = (i * HorizontalDistanceBetweenStickmans) % totalDistance;

                        if (Mathf.Abs(mod - totalDistance) < 0.01)
                        {
                            mod = 0;
                        }

                        position.Distance = mod;
                        position.positioner.SetDistance(mod);
                        position.positioner.RebuildImmediate();
                        
                    }
                }
            }
        
        

    }


    public void TweenStickmans()
    {
        foreach(Stickman stickman in StickmanList)
        {
            stickman.TweenToDesiredPosition();
        }
    }

    public virtual void RePositionStickmans()
    {
        ClearPositions();

        GeneratePositions();

        RePositionClosePoints();

        AssignPointsToStickmans();

        TweenStickmans();
    }

    public virtual void ClearPositions()
    {
        foreach (List<StickmanPosition> positionList in StickmanPositions)
        {
            foreach(StickmanPosition position in positionList)
            {
                Destroy(position.gameObject);
            }
        }

        foreach(ColumnHeader header in FindObjectsOfType<ColumnHeader>())
        {
           
            Destroy(header.gameObject);
            
        }

        StickmanPositions = new List<List<StickmanPosition>>();
    }

    public virtual void GeneratePositions()
    {
        totalDistance = _spline.CalculateLength();

        totalDistance = Mathf.FloorToInt(totalDistance / HorizontalDistanceBetweenStickmans) * HorizontalDistanceBetweenStickmans;

        float currentDistance = 0;

        if (totalDistance >= HorizontalDistanceBetweenStickmans)
        {
            for (int i = 0; i < StickmanList.Count; i++)
            {

                StickmanPosition position = Instantiate(positionPrefab, PositionsParent.transform);


                float mod = currentDistance % totalDistance;

                if(Mathf.Abs(mod - totalDistance) < 0.01)
                {
                    mod = 0;
                }

                float div = (currentDistance / totalDistance);

                if(Mathf.Abs(Mathf.Round(div) - div) < 0.01)
                {
                    div = Mathf.Round(div);
                }

                float xIndexHolder = (mod / HorizontalDistanceBetweenStickmans);

                int xIndex = 0;

                if (Mathf.Abs(Mathf.Round(xIndexHolder) - xIndexHolder) < 0.01)
                {
                    xIndex = Mathf.RoundToInt(xIndexHolder);
                }
                else
                {
                    xIndex = (int)xIndexHolder;
                }

                if (StickmanPositions.Count <= xIndex)
                {
                    StickmanPositions.Add(new List<StickmanPosition>());
                }

                StickmanPositions[xIndex].Add(position);

                position.ParentColumn = StickmanPositions[xIndex];

                int yIndex = (int)div;

                position.Position(mod, new Vector2(0, (yIndex * VerticalDistanceBetweenStickmans)), new Vector2Int(xIndex, yIndex), _spline);
                position.positioner.RebuildImmediate();
                currentDistance += HorizontalDistanceBetweenStickmans;

            }
        }
        else
        {

            for (int i = 0; i < StickmanList.Count; i++)
            {

                StickmanPosition position = Instantiate(positionPrefab, PositionsParent.transform);

                int xIndex = 0;

                if (StickmanPositions.Count <= xIndex)
                {
                    StickmanPositions.Add(new List<StickmanPosition>());
                }

                StickmanPositions[xIndex].Add(position);

                position.ParentColumn = StickmanPositions[xIndex];

                position.Position(currentDistance % totalDistance, new Vector2(0, (i * VerticalDistanceBetweenStickmans)), new Vector2Int(0, i), _spline);
                position.positioner.RebuildImmediate();
            }

        }


        foreach(List<StickmanPosition> positionColumn in StickmanPositions)
        {

            ColumnHeader column = Instantiate(ColumnPrefab, PositionsParent.transform);

            column.crowd = this;
            column.columnIndex = positionColumn[0].ListCoordinate.x;
            column.transform.position = positionColumn[0].transform.position;

        }
    }

    public virtual void RePositionClosePoints()
    {
        
        for(int i = StickmanPositions.Count - 1; i >= 0; i--)
        {
            RePositionCloseColumn(i);
        }
        

    }

    public void AddToColumn(int columnIndex, int Count)
    {
        if (Count > 0)
        {
            for (int i = 0; i < Count; i++)
            {
                if (StickmanPositions.Count > columnIndex)
                {
                    if (StickmanPositions[columnIndex].Count - 1 >= i)
                    {
                        Stickman newStickman = Instantiate(stickmanPrefab, StickmansParent.transform);
                        StickmanList.Add(newStickman);
                        newStickman.Join(this);

                        StickmanPosition position = Instantiate(StickmanPositions[columnIndex][StickmanPositions[columnIndex].Count - 1], PositionsParent.transform);
                        StickmanPositions[columnIndex].Add(position);
                        position.ListCoordinate.y += 1;
                        position.Height += VerticalDistanceBetweenStickmans;
                        position.ParentColumn = StickmanPositions[columnIndex];
                        position.positioner.RebuildImmediate();


                        newStickman.desiredPosition = position;
                    }
                }
            }

        }
        else
        {
            for (int i = 0; i < Mathf.Abs(Count); i++)
            {
                if(StickmanPositions.Count > columnIndex)
                {
                    if (StickmanPositions[columnIndex].Count - 1 >= i)
                    {
                        if (StickmanPositions[columnIndex][StickmanPositions[columnIndex].Count - 1 - i] != null)
                        {
                            StickmanPositions[columnIndex][StickmanPositions[columnIndex].Count - 1 - i].followingStickman.Dead();
                        }
                    }
                }

            }

            FixColumn(columnIndex);
        }

    }
    public void MultiplyColumn(int columnIndex, float factor)
    {

    }

    public int StickmanCount()
    {
        return StickmanList.Count;
    }

    public virtual void AssignPointsToStickmans()
    {
        int k = 0;
        for(int i=0; i< StickmanPositions.Count;i++)
        {
            for(int j = 0; j< StickmanPositions[i].Count; j++)
            {
                StickmanList[k].SetDesiredPosition(StickmanPositions[i][j]);
                k++;
            }
        }
    }

    
    public virtual void RePositionCloseColumn(int xIndex)
    {

        for(int i = xIndex -1; i >= 0; i--)
        {
            if(Vector3.Distance(StickmanPositions[xIndex][0].transform.position , StickmanPositions[i][0].transform.position) < HorizontalDistanceBetweenStickmans/2f)
            {
                MigrateColumn(xIndex,i);

                return;
            }
        }

    }

    public virtual void MigrateColumn(int from, int to)
    {
        
        int yIndexOffset = StickmanPositions[to].Count;

        StickmanPosition basePosition = StickmanPositions[to][0];

        foreach(StickmanPosition position in StickmanPositions[from])
        {

            int yCoord = position.ListCoordinate.y + yIndexOffset;

            StickmanPositions[to].Add(position);

            position.Position(basePosition.Distance, new Vector2(0,yCoord * VerticalDistanceBetweenStickmans), new Vector2Int(to, yCoord));
            position.ParentColumn = StickmanPositions[to];
            position.positioner.RebuildImmediate();

        }

        StickmanPositions.Remove(StickmanPositions[from]);

        Destroy(FindColumnHeader(from).gameObject);
        
    }

    public ColumnHeader FindColumnHeader(int index) {

        ColumnHeader header = null;

        foreach(ColumnHeader h in FindObjectsOfType<ColumnHeader>())
        {
            if(h.columnIndex == index)
            {
                header = h;
            }
        }


        return header;

    }

    public SplineComputer GetSpline()
    {
        return _spline;
    }

    public virtual void UpdateSpline(SplinePoint[] points)
    {
        if (_spline != null)
        {
            _spline.SetPoints(points, SplineComputer.Space.Local);

            _spline.RebuildImmediate();
        }

        RePositionStickmans();

    }
}