using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStackable : Stackable
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

    public override void UnLink(bool deletePoint = false)
    {
        BasicStacker stacker = this.LinkedPoint.ParentStacker as BasicStacker;
        if (stacker != null)
        {
            if (stacker.Breakable.Count > 0)
            {
                stacker.BreakFrom(this.LinkedPoint.Coordinate);
            }
        }

        base.UnLink(deletePoint);

    }

    public override void OnTriggerEnter(Collider other)
    {
        Stackable otherStackable = other.GetComponent<Stackable>();
        if (otherStackable != null)
        {
            if (otherStackable.CanStack && this.LinkedPoint != null)
            {
                BasicStackable stackable = other.gameObject.GetComponent<BasicStackable>();
                if (stackable != null)
                {
                    if (stackable.LinkedPoint == null)
                    {
                        BasicStackPoint stackPoint = this.LinkedPoint as BasicStackPoint;

                        if (stackPoint != null)
                        {
                            BasicStacker stacker = (this.LinkedPoint as BasicStackPoint).Parent.GetComponent<BasicStacker>();
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
