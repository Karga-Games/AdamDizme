using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Stickman stickman = other.GetComponent<Stickman>();

        if(stickman != null)
        {
            stickman.InHole(transform.position-stickman.transform.position);
        }
    }
}
