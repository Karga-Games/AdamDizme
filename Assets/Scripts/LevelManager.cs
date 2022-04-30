using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject BallsToStart;
    // Start is called before the first frame update
    void Start()
    {
        
        BallCrowd crowd = FindObjectOfType<BallCrowd>();

        crowd.SetupCrowd();

        foreach(Ball t in BallsToStart.GetComponentsInChildren<Ball>())
        {
            crowd.AddBall(t);
        }

        Destroy(BallsToStart);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
