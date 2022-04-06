using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject StickmansToStart;
    // Start is called before the first frame update
    void Start()
    {
        CrowdController crowdController = FindObjectOfType<CrowdController>();

        crowdController.SetupController();

        foreach(Stickman t in StickmansToStart.GetComponentsInChildren<Stickman>())
        {
            crowdController.AddStickman(t,false);
        }

        Destroy(StickmansToStart);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
