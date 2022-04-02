using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    public CrowdController crowd;
    public StickmanPosition desiredPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (desiredPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition.transform.position + new Vector3(0, desiredPosition.Height, 0), Time.deltaTime * 5f);
        }
    }

    public void SetCrowd(CrowdController controller)
    {
        crowd = controller;
    }

    public void SetDesiredPosition(StickmanPosition position)
    {
        desiredPosition = position;
    }
}
