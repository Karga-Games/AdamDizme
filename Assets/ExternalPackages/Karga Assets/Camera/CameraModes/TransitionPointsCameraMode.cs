using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraTransitionPoint
{
    public float TransitionSpeed;
    public Transform TransitionPoint;
    public bool Rotate;
    public float reachingDistance;
}

public class TransitionPointsCameraMode : CameraMode
{
    public List<CameraTransitionPoint> TransitionPoints;

    private int TransitionIndex = 0;

    public TransitionPointsCameraMode(List<CameraTransitionPoint> points)
    {
        this.TransitionPoints = points;
    }

    public override void Start(CameraController _cont)
    {
        base.Start(_cont);

        UpdateFollowObject();
        _controller.FollowOffset = Vector3.zero;

    }

    public override void Update()
    {
        UpdateFollowObject();
    }

    public override void FixedUpdate()
    {
        _controller.updateFOV(_controller.defaultFOV, _controller.FOVChangeSpeed);
        _controller.LookAtTarget();
        _controller.MoveToTarget();
    }

    public override void LateUpdate()
    {

    }


    public void UpdateFollowObject()
    {
        if((_controller.transform.position - TransitionPoints[TransitionIndex].TransitionPoint.position).magnitude > TransitionPoints[TransitionIndex].reachingDistance)
        {
            _controller.objectToFollow = TransitionPoints[TransitionIndex].TransitionPoint;
        }
        else
        {
            if(TransitionPoints.Count-1 > TransitionIndex)
            {
                TransitionIndex++;
            }
        }

    }


}
