using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class RoadComponentStacker : BasicStacker
{

    public Vector3 TablePositionOnRoad;
    
    public RoadPlayerController RoadPlayer;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if(RoadPlayer == null)
        {
            RoadPlayer = FindObjectOfType<RoadPlayerController>();
        }
    }


    // Update is called once per frame
    public override void FixedUpdate()
    {
        TablePositionOnRoad = StackPosition + new Vector3(RoadPlayer.horizontalPos,0,RoadPlayer.distanceTravelled);
    }

    public override BasicStackPoint SpawnNewStackPoint(string pointName, Vector3 pos)
    {
        GameObject newStackPointObj = new GameObject(pointName);
        newStackPointObj.transform.position = pos;
        return newStackPointObj.AddComponent<RoadComponentStackPoint>();
    }

    public override void Stack(Stackable stackable, string pointName = "RoadComponentStackPoint")
    {
        base.Stack(stackable, pointName);
        stackable.Wave();

    }


    public override void StackAt(Stackable stackable, Vector3Int Coordinate, bool force = false, string pointName = "RoadComponentStackPoint")
    {
        base.StackAt(stackable, Coordinate, force, pointName);
    }


    public override void OnTriggerEnter(Collider other)
    {

        RoadComponentStackable stackable = other.gameObject.GetComponent<RoadComponentStackable>();
        if (stackable != null)
        {
            if (stackable.LinkedPoint == null)
            {
                Stack(stackable);
            }
        }

    }
}
