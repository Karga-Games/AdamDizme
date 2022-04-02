using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdController : MonoBehaviour
{

    public StickmanPosition positionPrefab;
    public float HorizontalDistanceBetweenStickmans;
    public float VerticalDistanceBetweenStickmans;

    SplineComputer _spline;
    List<List<StickmanPosition>> StickmanPositions;
    List<Stickman> StickmanList;
    // Start is called before the first frame update
    void Start()
    {

        _spline = GetComponent<SplineComputer>();

        SplineGenerator _splineGenerator = FindObjectOfType<SplineGenerator>();
        if(_splineGenerator != null)
        {
            _splineGenerator.SetCrowdController(this);
        }

        StickmanPositions = new List<List<StickmanPosition>>();
        StickmanList = new List<Stickman>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddStickman(Stickman newStickman)
    {
        StickmanList.Add(newStickman);
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

    public void RePositionStickmans()
    {
        ClearPositions();

        float totalDistance = _spline.CalculateLength();

        totalDistance -= totalDistance % HorizontalDistanceBetweenStickmans;

        float currentDistance = 0;

        if (totalDistance >= HorizontalDistanceBetweenStickmans)
        {
            for (int i = 0; i < 50; i++)
            {

                StickmanPosition position = Instantiate(positionPrefab);

                int xIndex = (int)((currentDistance % totalDistance) / HorizontalDistanceBetweenStickmans);

                if(StickmanPositions.Count <= xIndex)
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

            for (int i = 0; i < 50; i++)
            {

                StickmanPosition position = Instantiate(positionPrefab);

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

        RePositionClosePoints();

    }

    public void RePositionClosePoints()
    {

        for(int i = StickmanPositions.Count - 1; i >= 0; i--)
        {
            RePositionCloseColumn(i);
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
