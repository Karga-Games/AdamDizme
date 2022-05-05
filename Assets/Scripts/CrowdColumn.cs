using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdColumn : MonoBehaviour
{
    public BallCrowd crowd;
    public List<Ball> balls;
    public int coordinate;

    protected SplinePositioner positioner;

    protected static float HitZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        int layermask = 1 << 9;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, layermask))
        {

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            Debug.Log("Did hit: " +hit.transform.gameObject.name);

            if(hit.point.z < HitZ)
            {
                HitZ = hit.point.z;
            }

        }
        else
        {

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 1000, Color.white);
            Debug.Log("Did not Hit");

        }
    }


    public void ResetColumn()
    {
        balls = new List<Ball>();
    }

    public void SetDistance(float dist)
    {
        if(positioner != null)
        {
            positioner.SetDistance(dist);
            positioner.RebuildImmediate();
        }
    }

    public void SetupPositioner()
    {
        if(positioner == null)
        {
            positioner = GetComponent<SplinePositioner>();
        }
    }

    public void SetCrowd(BallCrowd crowd, int coordinate)
    {
        this.crowd = crowd;
        this.coordinate = coordinate;

        if(positioner == null)
        {
            SetupPositioner();
        }

        positioner.spline = crowd.GetSpline();
    }

    public void AddBall(Ball ball)
    {

        int coordinate = balls.Count;

        balls.Add(ball);

        ball.SetColumn(this, coordinate);

    }

    public void RemoveBall(Ball ball)
    {
        balls.Remove(ball);
        crowd.RemoveBall(ball);
    }

    public void KillLast()
    {
        Ball lastBall = balls[balls.Count - 1];
        lastBall.Dead();
    }

    public void RemoveLast()
    {
        Ball lastBall = balls[balls.Count - 1];
        RemoveBall(lastBall);
    }
}
