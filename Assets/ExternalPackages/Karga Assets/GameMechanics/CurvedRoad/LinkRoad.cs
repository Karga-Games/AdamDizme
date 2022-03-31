using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class LinkRoad : MonoBehaviour
{
    public RoadMeshGenerator LinkedTo;
    protected RoadMeshGenerator Self;
    protected GameObject SelfObj;
    // Start is called before the first frame update
    void Start()
    {
        if(Self == null)
        {
            Self = GetComponent<RoadMeshGenerator>();
            Debug.LogError("There is no RoadMeshGenerator Component to link with!");
        }    
    }


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if (LinkedTo != null)
        {
            if (Self != null)
            {
                Self.distanceOffset = LinkedTo.pathCreator.path.length;
                this.transform.position = LinkedTo.transform.TransformPoint(LinkedTo.pathCreator.bezierPath.points.First());
            }
            else
            {
                this.transform.position = LinkedTo.transform.TransformPoint(LinkedTo.pathCreator.bezierPath.points.First());
            }


        }

#endif
    }
}
