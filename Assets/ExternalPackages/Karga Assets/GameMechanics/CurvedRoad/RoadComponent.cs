using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoadComponent : MonoBehaviour
{

    public bool findPathAuto = true;
    public RoadMeshGenerator roadGenerator;

    [SerializeField]
    public Vector3 positionOnRoad;
    public Vector3 rotationOnRoad;

    // Start is called before the first frame update
    void Start()
    {
        if(roadGenerator == null && findPathAuto)
        {
            roadGenerator = FindObjectOfType<RoadMeshGenerator>();
        }


        updatePosition();
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            updatePosition();
        }
#endif
    }

    void updatePosition()
    {
        if(roadGenerator != null)
        {
            Vector3 CenterPos = roadGenerator.pathCreator.path.GetPointAtDistance(roadGenerator.pathCreator.path.length - positionOnRoad.z);

            transform.position = CenterPos - (-1) * transform.right * positionOnRoad.x + transform.up * positionOnRoad.y;
            transform.rotation = roadGenerator.pathCreator.path.GetRotationAtDistance(roadGenerator.pathCreator.path.length - positionOnRoad.z) * Quaternion.Euler(180, 0, 90) * Quaternion.Euler(rotationOnRoad.x, rotationOnRoad.y, rotationOnRoad.z);

        }
    }
}
