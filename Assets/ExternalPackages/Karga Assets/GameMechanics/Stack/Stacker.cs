using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Stacker : MonoBehaviour
{
    public StackDreamteckSpline _stackSpline;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void StackableUnlinked()
    {

    }

    public virtual void Stack(Stackable stackable, string pointName = "StackPoint")
    {

    }

    public virtual void WaveStackable(Stackable stackable, float delay = 0)
    {
        StartCoroutine(GeneralFunctions.executeAfterSec(() => {

            stackable.Wave();

        }, delay));

    }
}



