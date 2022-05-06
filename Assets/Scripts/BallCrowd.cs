using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCrowd : MonoBehaviour
{

    public float HorizontalDistanceBetweenBalls;
    public float VerticalDistanceBetweenBalls;

    public Gradient CrowdColorGradient;

    public GameObject Indicators;
    public Indicator IndicatorPrefab;


    public GameObject Columns;
    public CrowdColumn ColumnPrefab;
    protected List<CrowdColumn> ColumnList;

    public GameObject Balls;
    public Ball BallPrefab;
    public List<Ball> BallList;

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
        ActionQueue.StartLoop();

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

    public void AddBallsToColumn(List<CrowdColumn> columns, int ballCount)
    {
        int added = 0;
        for(int i=0; i<columns.Count;)
        {
            Ball ball = Instantiate(BallPrefab, transform.position + new Vector3(0, 10f, 0), Quaternion.identity);
            ball.FindRenderer();
            AddBall(ball);
            columns[i].AddBall(ball);
            added++;
            i++;
            if (i >= columns.Count && added < ballCount)
            {
                i = 0;
            }
        }

    }

    public void AddBallsToColumn(CrowdColumn column, int ballCount, bool coroutine = true)
    {
        if (coroutine)
        {
            ActionQueue.EnqueueAction(AddBallsToColumnIEnumerator(column,ballCount));
        }
        else
        {
            for (int i = 0; i < ballCount; i++)
            {
                Ball ball = Instantiate(BallPrefab, transform.position + new Vector3(0, 10f, 0), Quaternion.identity);
                ball.FindRenderer();
                AddBall(ball);
                column.AddBall(ball);
            }

        }

    }

    public void KillBallsFromColumn(List<CrowdColumn> columns, int ballCount)
    {
        int killed = 0;
        for (int i = 0; i < columns.Count;)
        {
            columns[i].KillLast();
            killed++;
            i++;
            if (i >= columns.Count && killed < ballCount)
            {
                i = 0;
            }
        }
    }

    public void KillBallsFromColumn(CrowdColumn column, int ballCount, bool coroutine = true)
    {
        if (coroutine)
        {
            ActionQueue.EnqueueAction(KillBallsFromColumnIEnumerator(column,ballCount));
        }
        else
        {
            if (column.balls.Count >= ballCount)
            {
                for (int i = 0; i < ballCount; i++)
                {
                    column.KillLast();
                }
            }
        }

        
    }

    public IEnumerator KillBallsFromColumnIEnumerator(CrowdColumn column, int ballCount)
    {
        KillBallsFromColumn(column,ballCount,false);
        yield return null;
    }
    public IEnumerator AddBallsToColumnIEnumerator(CrowdColumn column, int ballCount)
    {

        AddBallsToColumn(column,ballCount,false);
        yield return null;
    }

    public void AddBall(Ball ball, bool realign = false)
    {
        BallList.Add(ball);
        ball.JoinToCrowd(this);
        
        if (realign)
        {
            AlignBallsOnColumns();
        }
        
    }

    public void RemoveBall(Ball ball)
    {
        BallList.Remove(ball);
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

    public void AlignBallsOnColumns(List<CrowdColumn> columns=null)
    {
        bool reAlign = false;
        if(columns == null)
        {
            columns = ColumnList;
            foreach (CrowdColumn column in columns)
            {
                column.ResetColumn();
            }

            reAlign = true;
        }

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

        if (!reAlign)
        {
            if (ColumnList != null)
            {
                foreach (CrowdColumn column in ColumnList)
                {
                    Destroy(column.gameObject);
                }
            }

            ColumnList = columns;
        }


    }


    public void ClearIndicators()
    {

        foreach(Transform t in Indicators.transform)
        {
            t.GetComponent<Indicator>().DestroyIndicator();
        }
    }

    public void GenerateIndicator(SplineComputer _spline)
    {
        ClearIndicators();

        float splineLength = _spline.CalculateLength();

        splineLength = Mathf.FloorToInt(splineLength / HorizontalDistanceBetweenBalls) * HorizontalDistanceBetweenBalls;

        float currentDistance = 0;

        int ballCount = BallList.Count;

        List<Indicator> generatedIndicators = new List<Indicator>();

        if (splineLength >= HorizontalDistanceBetweenBalls)
        {
            for (int i = 0; i < ballCount; i++)
            {

                if (Mathf.Abs(currentDistance - splineLength) > 0.01 && currentDistance < splineLength)
                {
                    Indicator indicator = CreateIndicator(_spline);

                    float distanceOnSpline = currentDistance % splineLength;

                    if (Mathf.Abs(distanceOnSpline - splineLength) < 0.01)
                    {
                        distanceOnSpline = 0;
                    }

                    indicator.SetDistance(distanceOnSpline);

                    generatedIndicators.Add(indicator);

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
            Indicator indicator = CreateIndicator(_spline);

            indicator.SetDistance(0);

            generatedIndicators.Add(indicator);

        }

        Indicator.MinHitZ = 9999;


        foreach (Indicator indicator in generatedIndicators)
        {
            indicator.CalculateHitZ();
        }

        foreach (Indicator indicator in generatedIndicators)
        {
            indicator.SpawnIndicator();
        }
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


    public Indicator CreateIndicator(SplineComputer spline = null)
    {
        Indicator newIndicator = Instantiate(IndicatorPrefab, Indicators.transform);
        newIndicator.SetupPositioner();
        newIndicator.SetupSpline(spline);
        return newIndicator;
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

            GenerateIndicator(crowdSpline);

            if (updateCrowd)
            {
                AlignBallsOnSpline(crowdSpline);
            }

        }


    }

}
