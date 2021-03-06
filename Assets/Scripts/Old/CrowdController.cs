using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KargaGames.Drawing;


public class CrowdController : MonoBehaviour
{
    public SplineComputer _indicatorSpline;
    public Stickman stickmanPrefab;
    public StickmanPosition positionPrefab;
    public ColumnHeader ColumnPrefab;
    public GameObject IndicatorPrefab;
    public float HorizontalDistanceBetweenStickmans;
    public float VerticalDistanceBetweenStickmans;

    public GameObject PositionsParent;
    public GameObject StickmansParent;

    protected SplineComputer _spline;
    protected List<List<StickmanPosition>> StickmanPositions;
    protected List<List<StickmanPosition>> IndicatorPositions;
    public List<Stickman> StickmanList;
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

    bool setup = false;

    public int stickmanCount;

    public CoroutineQueue ActionQueue;

    // Start is called before the first frame update
    public virtual void Start()
    {
        ActionQueue = new CoroutineQueue(this);
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
        if (setup)
        {
            if (StickmanList.Count == 0 )
            {
                
                if (!GameSceneManager.gameOver)
                {
                    if (playerController.levelend)
                    {
                        playerController.speed = 0;
                        gameSceneManager.LevelPassed();
                    }
                    else
                    {
                        Fail();

                    }
                }
                
            }
            else
            {
                if (StickmanList[0] == null)
                {
                    StickmanList.RemoveAt(0);
                }
            }


            lastFixTime += Time.deltaTime;

            if (lastFixTime > 0.5f)
            {
                FixWorking = false;
            }

            CalculateVelocity();
        }

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
            //_splineGenerator.SetCrowd(this);
        }

        StickmanPositions = new List<List<StickmanPosition>>();
        IndicatorPositions = new List<List<StickmanPosition>>();
        if(StickmanList == null)
        {
            StickmanList = new List<Stickman>();
        }

        setup = true;
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

        if(stickman != null)
        {
            if(stickman.desiredPosition!= null)
            {
                Vector2Int coord = stickman.desiredPosition.ListCoordinate;

                StickmanList.Remove(stickman);

                if (reposition)
                {
                    fixCommand++;

                    float commandIndex = fixCommand;
                    StartCoroutine(GeneralFunctions.executeAfterSec(() => {

                        if (commandIndex == fixCommand)
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
                                //TweenStickmans();

                            }

                        }

                    }, repositionDelay));
                }
            }
            
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
            if(stickman != null)
            {
                stickman.TweenToDesiredPosition();
            }
        }
    }

    public virtual void RePositionStickmans()
    {

        ClearPositions();

        GeneratePositions();

        RePositionClosePoints();

        FixListIndexes();

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

        StickmanList.RemoveAll(item => item == null);


        StickmanPositions = new List<List<StickmanPosition>>();
    }


    public void ClearIndicator()
    {
        foreach (List<StickmanPosition> positionList in IndicatorPositions)
        {
            foreach (StickmanPosition position in positionList)
            {
                Destroy(position.gameObject);
            }
        }

        foreach (ColumnHeader header in FindObjectsOfType<ColumnHeader>())
        {

            Destroy(header.gameObject);

        }

        StickmanList.RemoveAll(item => item == null);


        IndicatorPositions = new List<List<StickmanPosition>>();

    }

    public void GenerateIndicators()
    {
        ClearIndicator();

        totalDistance = _indicatorSpline.CalculateLength();

        totalDistance = Mathf.FloorToInt(totalDistance / HorizontalDistanceBetweenStickmans) * HorizontalDistanceBetweenStickmans;

        float currentDistance = 0;

        int range = (int)(totalDistance / HorizontalDistanceBetweenStickmans);

        if (totalDistance >= HorizontalDistanceBetweenStickmans)
        {
            for (int i = 0; i < range; i++)
            {

                StickmanPosition position = Instantiate(positionPrefab, PositionsParent.transform);


                float mod = currentDistance % totalDistance;

                if (Mathf.Abs(mod - totalDistance) < 0.01)
                {
                    mod = 0;
                }

                float div = (currentDistance / totalDistance);

                if (Mathf.Abs(Mathf.Round(div) - div) < 0.01)
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

                if (IndicatorPositions.Count <= xIndex)
                {
                    IndicatorPositions.Add(new List<StickmanPosition>());
                }

                IndicatorPositions[xIndex].Add(position);

                position.ParentColumn = IndicatorPositions[xIndex];

                int yIndex = (int)div;

                position.Position(mod, new Vector2(0, (yIndex * VerticalDistanceBetweenStickmans)), new Vector2Int(xIndex, yIndex), _indicatorSpline);
                position.positioner.RebuildImmediate();
                currentDistance += HorizontalDistanceBetweenStickmans;

            }
        }
        else
        {

            for (int i = 0; i < range; i++)
            {

                StickmanPosition position = Instantiate(positionPrefab, PositionsParent.transform);

                int xIndex = 0;

                if (IndicatorPositions.Count <= xIndex)
                {
                    IndicatorPositions.Add(new List<StickmanPosition>());
                }

                IndicatorPositions[xIndex].Add(position);

                position.ParentColumn = IndicatorPositions[xIndex];

                position.Position(currentDistance % totalDistance, new Vector2(0, (i * VerticalDistanceBetweenStickmans)), new Vector2Int(0, i), _indicatorSpline);
                position.positioner.RebuildImmediate();
            }

        }


        foreach (List<StickmanPosition> positionColumn in IndicatorPositions)
        {

            ColumnHeader column = Instantiate(ColumnPrefab, PositionsParent.transform);

            column.crowd = this;
            column.columnIndex = positionColumn[0].ListCoordinate.x;
            column.transform.position = positionColumn[0].transform.position;

        }

    }


    public virtual void GeneratePositions(bool full = true)
    {
        totalDistance = _spline.CalculateLength();

        totalDistance = Mathf.FloorToInt(totalDistance / HorizontalDistanceBetweenStickmans) * HorizontalDistanceBetweenStickmans;

        float currentDistance = 0;

        int range = StickmanList.Count;

        if (!full)
        {
            range = (int)(totalDistance / HorizontalDistanceBetweenStickmans);
        }

        if (totalDistance >= HorizontalDistanceBetweenStickmans)
        {
            for (int i = 0; i < range; i++)
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

            for (int i = 0; i < range; i++)
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

    public void Spread()
    {

        foreach(Stickman stickman in StickmanList)
        {
            stickman.Free();
        }

        playerController.levelend = true;

        transform.SetParent(playerController.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        /*
        LeanTween.moveLocal(gameObject, Vector3.zero,0.5f);
        LeanTween.rotateLocal(gameObject, Vector3.zero, 0.5f);
        */



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
                        newStickman.transform.localPosition = new Vector3(0,0,-5);
                        StickmanList.Add(newStickman);
                        newStickman.Join(this);

                        StickmanPosition position = Instantiate(StickmanPositions[columnIndex][StickmanPositions[columnIndex].Count - 1], PositionsParent.transform);
                        StickmanPositions[columnIndex].Add(position);
                        position.ListCoordinate.y += 1;
                        position.Height += VerticalDistanceBetweenStickmans;
                        position.ParentColumn = StickmanPositions[columnIndex];

                        position.positioner.Rebuild();

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
                    if(StickmanPositions[columnIndex].Count > 0)
                    {
                        StickmanPositions[columnIndex][0].followingStickman.Dead();
                    }
                }

            }

            FixColumn(columnIndex);
        }



    }


    public void MultiplyColumn(int columnIndex, float factor, int max = 999)
    {
        if(factor > 0)
        {
            int stickmanCount = StickmanPositions[columnIndex].Count;
            if (stickmanCount > max)
            {
                stickmanCount = max;
            }

            if (factor > 1)
            {

                int count = (int)(stickmanCount * factor) - stickmanCount;
                AddToColumn(columnIndex, count);

            }
            else
            {
                int count = -1*(int)(stickmanCount * (1-factor));
                AddToColumn(columnIndex, count);

            }
        }

        
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


    public virtual void FixListIndexes()
    {
        ColumnHeader[] headers = FindObjectsOfType<ColumnHeader>();


        int i = 0;
        foreach(List<StickmanPosition> positionList in StickmanPositions)
        {

            foreach (ColumnHeader h in headers)
            {
                if (h.columnIndex == positionList[0].ListCoordinate.x)
                {
                    h.columnIndex = i;
                }
            }


            foreach (StickmanPosition position in positionList)
            {


                position.ListCoordinate.x = i;
            }
            i++;
        }

        i = 0;

        

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


    public void UpdateIndicator(SplinePoint[] points)
    {
        if(_indicatorSpline != null)
        {
            _indicatorSpline.SetPoints(points, SplineComputer.Space.Local);

            _indicatorSpline.RebuildImmediate();
        }

        GenerateIndicators();
    }


    public virtual void UpdateSpline(SplinePoint[] points, bool drawingFinished = false)
    {

        if (drawingFinished)
        {
            if (_spline != null)
            {
                _spline.SetPoints(points, SplineComputer.Space.Local);

                _spline.RebuildImmediate();
            }

            RePositionStickmans();

        }
        else
        {
            UpdateIndicator(points);
        }


    }
}