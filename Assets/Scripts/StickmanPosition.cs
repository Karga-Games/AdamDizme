
using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanPosition : MonoBehaviour
{
    public SplinePositioner positioner;
    public float Distance;
    public float Height;
    public Transform indicator;

    public Vector2Int ListCoordinate;
    // Start is called before the first frame update
    void Start()
    {
        //positioner = GetComponent<SplinePositioner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(indicator != null)
        {
            indicator.transform.position = transform.position + new Vector3(0,Height,0);
        }
    }

    public void SetSpline(SplineComputer spline)
    {
        if (positioner == null)
        {
            positioner = GetComponent<SplinePositioner>();
        }
        positioner.spline = spline;
    }

    public void Position(float distance, Vector2 offset, Vector2Int coord, SplineComputer spline = null)
    {

        if (spline != null)
        {
            SetSpline(spline);
        }
        Height = offset.y;
        Distance = distance;
        ListCoordinate = coord;
        positioner.SetDistance(distance);

    }
}
