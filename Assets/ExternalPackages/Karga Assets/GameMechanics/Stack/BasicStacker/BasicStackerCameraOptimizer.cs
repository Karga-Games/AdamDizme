using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStackerCameraOptimizer : MonoBehaviour
{
    public CameraController playerCamera;
    public BasicStacker stacker;
    public float minY;
    public float factor;
    // Start is called before the first frame update
    void Start()
    {
        if(playerCamera == null)
        {
            playerCamera = FindObjectOfType<CameraController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.FollowOffset.y = minY + stacker.Count * factor / stacker.StackPoints.MaxRow;
    }
}
