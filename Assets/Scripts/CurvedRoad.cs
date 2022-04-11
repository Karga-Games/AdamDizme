using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedRoad : MonoBehaviour
{
    public Vector2 tiling;
    Material roadMaterial;

    private void OnValidate()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        roadMaterial = mr.material;

        if (roadMaterial != null)
        {
            roadMaterial.mainTextureScale = tiling;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
