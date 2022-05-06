using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{

    public GameObject IndicatorPrefab;
    public bool HitObstacle;
    public static float MinHitZ;
    public Vector3 HitPoint;

    public Color GoodColor;
    public Color BadColor;

    protected SplinePositioner positioner;
    protected GameObject indicator;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //CalculateHitZ();
    }
    public void SetDistance(float dist)
    {
        if (positioner != null)
        {
            positioner.SetDistance(dist);
            positioner.RebuildImmediate();
        }
    }

    public void SetupSpline(SplineComputer spline)
    {
        if (positioner != null)
        {
            positioner.spline = spline;
        }
    }

    public void SetupPositioner()
    {
        if (positioner == null)
        {
            positioner = GetComponent<SplinePositioner>();
        }
    }
    public void CalculateHitZ()
    {
        RaycastHit hit;

        int obstacleLayer = 1 << 9;
        int indicatorPlaneLayer = 1 << 11;

        int layermask = obstacleLayer | indicatorPlaneLayer;


        if (Physics.SphereCast(transform.position, 0.2f ,transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layermask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            HitPoint = hit.point;

            if(hit.transform.gameObject.layer == 9)
            {
                //obstacle
                HitObstacle = true;

            }
            else
            {
                //plane
                HitObstacle = false;

            }

            if (HitPoint.z-0.1f < MinHitZ)
            {
                MinHitZ = HitPoint.z-0.1f;
            }

        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            HitPoint = transform.position;
        }
    
    }

    public void SpawnIndicator()
    {
        Vector3 spawnPos = HitPoint;
        spawnPos.z = MinHitZ;

        GameObject indicator = Instantiate(IndicatorPrefab,spawnPos,Quaternion.Euler(-90,0,0));
        //indicator.transform.SetParent(transform);

        if (!HitObstacle) {

            indicator.GetComponent<MeshRenderer>().material.color = GoodColor;

        }
        else
        {
            indicator.GetComponent<MeshRenderer>().material.color = BadColor;
        }

        this.indicator = indicator;
    }

    public void DestroyIndicator()
    {
        Destroy(indicator.gameObject);
        Destroy(gameObject);
    }
}
