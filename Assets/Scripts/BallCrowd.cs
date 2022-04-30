using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCrowd : MonoBehaviour
{

    public float HorizontalDistanceBetweenBalls;
    public float VerticalDistanceBetweenBalls;

    public GameObject Columns;
    public CrowdColumn ColumnPrefab;
    protected List<CrowdColumn> ColumnList;

    public GameObject Balls;
    public Ball BallPrefab;
    protected List<Ball> BallList;

    protected CoroutineQueue ActionQueue;

    protected SplineComputer crowdSpline;

    // Start is called before the first frame update
    void Start()
    {
        SetupCrowd();
    }

    public void SetupCrowd()
    {

        ActionQueue = new CoroutineQueue(this);

        crowdSpline = GetComponent<SplineComputer>();

        AttachToSplineGenerator();

        if (BallList == null)
        {
            BallList = new List<Ball>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBall(Ball ball)
    {
        BallList.Add(ball);
        ball.transform.SetParent(Balls.transform);
    }

    public void AttachToSplineGenerator()
    {
        SplineGenerator _splineGenerator = FindObjectOfType<SplineGenerator>();
        if (_splineGenerator != null)
        {
            _splineGenerator.SetCrowd(this);
        }
    }

    public SplineComputer GetSpline()
    {
        return crowdSpline;
    }

    public void AlignBallsOnSpline(SplineComputer _spline)
    {
        List<CrowdColumn> newColumns = GenerateColumnsOnSpline(_spline);

        AlignBallsOnColumns(newColumns);

    }

    public void AlignBallsOnColumns(List<CrowdColumn> columns)
    {
        int i = 0;

        foreach(Ball ball in BallList)
        {
            if(i >= columns.Count)
            {
                i = 0;
            }

            columns[i].AddBall(ball);

            i++;
        }

        if(ColumnList != null)
        {
            foreach (CrowdColumn column in ColumnList)
            {
                Destroy(column.gameObject);
            }
        }

        ColumnList = columns;

    }


    public List<CrowdColumn> GenerateColumnsOnSpline(SplineComputer _spline)
    {
        float splineLength = _spline.CalculateLength();

        splineLength = Mathf.FloorToInt(splineLength / HorizontalDistanceBetweenBalls) * HorizontalDistanceBetweenBalls;

        float currentDistance = 0;

        int ballCount = BallList.Count;

        List<CrowdColumn> generatedColumns = new List<CrowdColumn>();

        if(splineLength >= HorizontalDistanceBetweenBalls)
        {
            for (int i = 0; i < ballCount; i++)
            {
                
                if (Mathf.Abs(currentDistance - splineLength) > 0.01 && currentDistance < splineLength)
                {
                    CrowdColumn column = CreateColumn();

                    column.SetCrowd(this,i);

                    float distanceOnSpline = currentDistance % splineLength;

                    if (Mathf.Abs(distanceOnSpline - splineLength) < 0.01)
                    {
                        distanceOnSpline = 0;
                    }

                    column.SetDistance(distanceOnSpline);

                    generatedColumns.Add(column);

                    currentDistance += HorizontalDistanceBetweenBalls;

                }
                else
                {
                    break;
                }
            }

        }
        else
        {
            CrowdColumn column = CreateColumn();

            column.SetCrowd(this, 0);

            column.SetDistance(0);

            generatedColumns.Add(column);

        }



        return generatedColumns;



    }

    public CrowdColumn CreateColumn()
    {
        CrowdColumn newColumn = Instantiate(ColumnPrefab,Columns.transform);
        newColumn.SetupPositioner();
        return newColumn;
    }

    public virtual void UpdateSpline(SplinePoint[] points, bool updateCrowd = true)
    {

        if (crowdSpline != null)
        {

            crowdSpline.SetPoints(points, SplineComputer.Space.Local);

            crowdSpline.RebuildImmediate();


            if (updateCrowd)
            {
                AlignBallsOnSpline(crowdSpline);
            }

        }


    }

}
