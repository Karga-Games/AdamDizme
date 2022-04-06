using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KargaGames.Drawing;

public class CrowdController : MonoBehaviour
{

    public StickmanPosition positionPrefab;
    public float HorizontalDistanceBetweenStickmans;
    public float VerticalDistanceBetweenStickmans;

    public GameObject PositionsParent;
    public GameObject StickmansParent;

    SplineComputer _spline;
    List<List<StickmanPosition>> StickmanPositions;
    List<Stickman> StickmanList;
    GameSceneManager gameSceneManager;
    SplineByDrawing splineDrawer;
    SplineGenerator splineGenerator;
    GameObject drawingUI;
    RunnerPlayerController playerController;

    CameraController cameraController;
    // Start is called before the first frame update
    void Start()
    {
        gameSceneManager = FindObjectOfType<GameSceneManager>();
        splineDrawer = FindObjectOfType<SplineByDrawing>();
        splineGenerator = FindObjectOfType<SplineGenerator>();
        drawingUI = GameObject.FindGameObjectWithTag("DrawingArea");

        cameraController = FindObjectOfType<CameraController>();
        playerController = FindObjectOfType<RunnerPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(StickmanList.Count == 0 && !GameSceneManager.gameOver)
        {
            Fail();

        }
    }

    public void Fail()
    {
        gameSceneManager.LevelFailed();
        splineDrawer.enabled = false;
        drawingUI.SetActive(false);
        playerController.speed = 0;
    }

    public void SetupController()
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


    public Stickman GetLastMan()
    {
        return StickmanList[StickmanList.Count-1];
    }

    public void AddStickman(Stickman newStickman, bool reposition = true)
    {
        StickmanList.Add(newStickman);
        newStickman.transform.SetParent(StickmansParent.transform);
        newStickman.Join(this);

        if (reposition)
        {
            RePositionStickmans();
        }
    }

    public void RemoveStickman(Stickman stickman, bool reposition = false, float repositionDelay = 0f)
    {
        StickmanList.Remove(stickman);

        if (reposition)
        {
            StartCoroutine(GeneralFunctions.executeAfterSec(() => {

                RePositionStickmans();

            },repositionDelay));
        }
    }

    public void RePositionStickmans()
    {
        ClearPositions();

        GeneratePositions();

        RePositionClosePoints();

        AssignPointsToStickmans();

    }

    public void ClearPositions()
    {
        foreach (List<StickmanPosition> positionList in StickmanPositions)
        {
            foreach(StickmanPosition position in positionList)
            {
                Destroy(position.gameObject);
            }
        }

        StickmanPositions = new List<List<StickmanPosition>>();
    }


    public void GeneratePositions()
    {
        float totalDistance = _spline.CalculateLength();

        totalDistance -= totalDistance % HorizontalDistanceBetweenStickmans;

        float currentDistance = 0;

        if (totalDistance >= HorizontalDistanceBetweenStickmans)
        {
            for (int i = 0; i < StickmanList.Count; i++)
            {

                StickmanPosition position = Instantiate(positionPrefab, PositionsParent.transform);

                int xIndex = (int)((currentDistance % totalDistance) / HorizontalDistanceBetweenStickmans);

                if (StickmanPositions.Count <= xIndex)
                {
                    StickmanPositions.Add(new List<StickmanPosition>());
                }

                StickmanPositions[xIndex].Add(position);

                int yIndex = (int)(currentDistance / totalDistance);

                position.Position(currentDistance % totalDistance, new Vector2(0, (yIndex * VerticalDistanceBetweenStickmans)), new Vector2Int(xIndex, yIndex), _spline);
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

                position.Position(currentDistance % totalDistance, new Vector2(0, (i * VerticalDistanceBetweenStickmans)), new Vector2Int(0, i), _spline);
                position.positioner.RebuildImmediate();
            }

        }
    }

    public void RePositionClosePoints()
    {

        for(int i = StickmanPositions.Count - 1; i >= 0; i--)
        {
            RePositionCloseColumn(i);
        }


    }


    public void AssignPointsToStickmans()
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

    
    public void RePositionCloseColumn(int xIndex)
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

    public void MigrateColumn(int from, int to)
    {

        int yIndexOffset = StickmanPositions[to].Count;

        StickmanPosition basePosition = StickmanPositions[to][0];

        foreach(StickmanPosition position in StickmanPositions[from])
        {

            int yCoord = position.ListCoordinate.y + yIndexOffset;

            StickmanPositions[to].Add(position);

            position.Position(basePosition.Distance, new Vector2(0,yCoord * VerticalDistanceBetweenStickmans), new Vector2Int(to, yCoord));

            position.positioner.RebuildImmediate();

        }

        StickmanPositions.Remove(StickmanPositions[from]);

    }

    public SplineComputer GetSpline()
    {
        return _spline;
    }

    public void UpdateSpline(SplinePoint[] points)
    {
        if (_spline != null)
        {
            _spline.SetPoints(points, SplineComputer.Space.Local);

            _spline.RebuildImmediate();
        }

        RePositionStickmans();

    }
}