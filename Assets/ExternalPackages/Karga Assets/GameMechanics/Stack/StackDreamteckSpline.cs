using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
[RequireComponent(typeof(SplineComputer))]
public class StackDreamteckSpline : MonoBehaviour
{
    public BasicStacker _stacker;
    SplineComputer _spline;
    public List<SplineStackPointPair> _points;

    int index = 0;
    public void Start()
    {
        _spline = GetComponent<SplineComputer>();
        _points = new List<SplineStackPointPair>();
    }

    public void FixedUpdate()
    {
        UpdatePointPositions();
    }

    public void Stack(StackPoint newStackpoint)
    {
        index = _points.Count;
        SplinePoint newPoint = new SplinePoint();
        newPoint.SetPosition(newStackpoint.transform.position);
        newPoint.size = 1;
        SplineStackPointPair newPair = new SplineStackPointPair();
        newPair.stackPoint = newStackpoint;
        newPair.splinePoint = newPoint;
        newPair.splineComputer = _spline;
        newPair.splineManager = this;
        newPair.index = index;

        _spline.SetPoint(index, newPoint);
        _points.Add(newPair);
    }

    public void Unlink(StackPoint stackPoint)
    {
        
        SplineStackPointPair pairToDelete = null;
        foreach (SplineStackPointPair pair in _points)
        {
            if(pair.stackPoint.Coordinate == stackPoint.Coordinate)
            {
                pairToDelete = pair;
                break;
            }
        }

        if(pairToDelete != null)
        {
            pairToDelete.DeletePoint();
            _points.Remove(pairToDelete);
            RefreshIndexes();
        }
        
    }

    public void UpdatePointPositions()
    {
        foreach (SplineStackPointPair pair in _points)
        {
            pair.UpdatePointPosition();
        }
    }
    public void RefreshIndexes()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            _points[i].index = i;
        }
    }

}

public class SplineStackPointPair
{
    public StackPoint stackPoint;
    public SplinePoint splinePoint;
    public SplineComputer splineComputer;
    public StackDreamteckSpline splineManager;
    public int index;
    public void UpdatePointPosition()
    {
        splineComputer.SetPointPosition(index,stackPoint.transform.position);
    }
    public void DeletePoint()
    {
        SplinePoint[] points = splineComputer.GetPoints();
        List<SplinePoint> pointList = new List<SplinePoint>(points);
        pointList.RemoveAt(index);
        splineComputer.SetPoints(pointList.ToArray());
    }

    

}



