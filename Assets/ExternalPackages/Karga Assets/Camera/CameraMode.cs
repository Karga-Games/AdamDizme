using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraMode
{
    protected CameraController _controller;

    public virtual void Start(CameraController _cont)
    {
        _controller = _cont;
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void LateUpdate()
    {

    }

    public virtual void Exit()
    {

    }
 


}
