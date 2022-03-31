using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BasicStackTrigger : MonoBehaviour
{
    public bool Multiplier;
    public Vector3Int Multiply_Value;
    public bool ReArrange;
    public Axis[] AxisesToReArrange;
    private void OnTriggerEnter(Collider other)
    {
        BasicStacker stacker = other.GetComponent<BasicStacker>();
        if (stacker != null)
        {
            if (ReArrange)
            {
                stacker.ReArrange(AxisesToReArrange);
            }
            if (Multiplier)
            {
                stacker.Multiply(Multiply_Value);
            }
        }
    }
}
