using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NormalCameraMode : CameraMode
{
    public override void Update()
    {

    }

    public override void FixedUpdate()
    {
    }

    public override void LateUpdate()
    {

        _controller.updateFOV(_controller.defaultFOV, _controller.FOVChangeSpeed);
        _controller.LookAtTarget();
        _controller.MoveToTarget();
    }

}
