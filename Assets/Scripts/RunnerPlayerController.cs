using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerPlayerController : DreamteckRoadPlayerController
{
    public bool levelend = false;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!levelend)
        {
            base.Update();
        }
        else
        {
            transform.position += Vector3.Lerp(transform.position, transform.position + transform.forward, Time.deltaTime * 0.00001f);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

    }
}
