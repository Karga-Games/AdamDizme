using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathCreation.PathCreator))]
[ExecuteInEditMode]
public class RoadMeshGenerator : RoadMeshCreator
{
    public float distanceOffset = 0;
    public bool invisible = false;
    // Start is called before the first frame update
    void Start()
    {
        PathUpdated();
        /*
        int i = 0;
        foreach(Vector3 point in pathCreator.bezierPath.points)
        {
            GameObject obj = new GameObject();
            obj.name="Point No:"+i;
            obj.transform.position = transform.TransformPoint(point);
            i++;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void PathUpdated()
    {
        if (invisible)
        {

        }
        else
        {
            base.PathUpdated();
        }
    }
}
