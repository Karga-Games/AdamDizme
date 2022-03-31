using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPoint : MonoBehaviour
{
    public Transform Parent;
    public Stacker ParentStacker;
    public Stackable LinkedObject;
    public bool DestroyOnUnlink;
    public Vector3Int Coordinate;
    public StackPoint previousStackPointZ;
    public StackPoint nextStackPointZ;
    public float WaveTransportDelay;
    public bool WaveTransport = false;

    public bool Active = true;
    public virtual void SetParent(Transform parent, Stacker parentStacker)
    {
        Parent = parent;

        ParentStacker = parentStacker;

    }

    public void Link(Stackable stackable)
    {
        stackable.Link(this);
    }

    public void UnLink()
    {

        LinkedObject.UnLink(DestroyOnUnlink);

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position,0.1f);
    }

    public virtual void RefreshCoordinate(Vector3Int coord)
    {
        Coordinate = coord;
    }

}



public class PreviousPoint
{
    public Transform X;
    public Transform Y;
    public Transform Z;
}
