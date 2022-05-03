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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
