using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadComponentStackable : BasicStackable
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
    }


    public override void OnTriggerEnter(Collider other)
    {
        Stackable otherStackable = other.GetComponent<Stackable>();
        if (otherStackable != null)
        {
            if (otherStackable.CanStack && this.LinkedPoint != null)
            {
                RoadComponentStackable stackable = other.gameObject.GetComponent<RoadComponentStackable>();
                if (stackable != null)
                {
                    if (stackable.LinkedPoint == null)
                    {
                        RoadComponentStackPoint stackPoint = this.LinkedPoint as RoadComponentStackPoint;

                        if (stackPoint != null)
                        {
                            RoadComponentStacker stacker = this.LinkedPoint.ParentStacker as RoadComponentStacker;
                            if (stacker != null)
                            {
                                stacker.Stack(stackable);
                            }
                        }

                    }

                }

            }

        }
    }

}
