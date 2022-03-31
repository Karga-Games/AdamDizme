using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum StackPointFollowType
{
    FollowBase,
    FollowPrevious
}

public class BasicStackPoint : StackPoint
{

    public Vector3 Distance;
    public Vector3 FollowSpeed;
    public float FollowSmoothing;
    public StackPointFollowType followType;
    protected Vector3 CurrentLocalPosition;
    protected Vector3 CurrentDesiredPosition;
    protected Quaternion CurrentDesiredRotation;
    protected System.Action FollowFunction;


    public virtual void Awake()
    {
        CurrentDesiredPosition = transform.position;
    }

    public virtual void FixedUpdate()
    {
        if (Active)
        {
            CalculateNewPos(FollowSpeed);
            FollowParent();
        }
    }

    public virtual void SetParent(Transform parent,Stacker stacker, bool resetPos = true)
    {
        base.SetParent(parent, stacker);
        if (resetPos)
        {
            transform.rotation = Parent.rotation;
            transform.position = Parent.TransformPoint(new Vector3(Coordinate.x * Distance.x, Coordinate.y * Distance.y, Coordinate.z * Distance.z));
        }
    }

    public virtual void FollowParent()
    {
        transform.rotation = CurrentDesiredRotation;
        transform.position = Vector3.Lerp(transform.position,CurrentDesiredPosition, FollowSmoothing);
    }

    public virtual void CalculateNewPos(Vector3 speed)
    {
        switch (followType)
        {
            case StackPointFollowType.FollowBase:

                CalculatePositionFromBase(speed);

                break;

            case StackPointFollowType.FollowPrevious:

                CalculatePositionFromPrevious(speed);

                break;

        }


    }

    public virtual void CalculatePositionFromBase(Vector3 speed)
    {
        if (Parent != null)
        {
            Vector3 DesiredLocalPosition = new Vector3(Coordinate.x * Distance.x, Coordinate.y * Distance.y, Coordinate.z * Distance.z);

            CurrentLocalPosition = Parent.InverseTransformPoint(transform.position);

            CurrentDesiredPosition = Parent.TransformPoint(new Vector3(Mathf.Lerp(CurrentLocalPosition.x, DesiredLocalPosition.x, Time.deltaTime * speed.x / Coordinate.y), Mathf.Lerp(CurrentLocalPosition.y, DesiredLocalPosition.y, Time.deltaTime * speed.y), Mathf.Lerp(CurrentLocalPosition.z, DesiredLocalPosition.z, Time.deltaTime * speed.z)));

        }
        CurrentDesiredRotation = Parent.rotation;
    }

    public virtual void CalculatePositionFromPrevious(Vector3 speed)
    {

        Transform PreviousX = GetPreviousPointX();
        Transform PreviousY = GetPreviousPointY();
        Transform PreviousZ = GetPreviousPointZ();

        Vector3 DesiredPosition;
       
        DesiredPosition.x = PreviousX.position.x + Distance.x;
        DesiredPosition.y = PreviousY.position.y + Distance.y;
        DesiredPosition.z = PreviousZ.position.z + Distance.z;

        if (Coordinate.x == 0)
        {
            DesiredPosition.x = Parent.transform.position.x;
        }
        if (Coordinate.y == 0)
        {
            DesiredPosition.y = Parent.transform.position.y;
        }
        if (Coordinate.z == 0)
        {
            DesiredPosition.z = Parent.transform.position.z;
        }


        CurrentDesiredPosition = new Vector3(Mathf.Lerp(CurrentDesiredPosition.x, DesiredPosition.x, Time.deltaTime*speed.x / 4), Mathf.Lerp(CurrentDesiredPosition.y, DesiredPosition.y, Time.deltaTime * speed.y / 4), Mathf.Lerp(CurrentDesiredPosition.z, DesiredPosition.z, Time.deltaTime * speed.z / 4));

        CurrentDesiredRotation = Parent.rotation;

    }



    #region GetPreviousPoint


    public virtual Transform GetPreviousPointX()
    {
        BasicStacker RParentStacker = (ParentStacker as BasicStacker);

        if(RParentStacker != null)
        {
            int index = 1;

            int XCoordinate = Coordinate.x - index;

            for (; index <= Coordinate.x; index++)
            {

                if (RParentStacker.StackPoints.Matrix[XCoordinate][Coordinate.y][Coordinate.z] != null)
                {
                    return RParentStacker.StackPoints.Matrix[XCoordinate][Coordinate.y][Coordinate.z].transform;
                }

            }

        }

        return Parent.transform;
    }

    public virtual Transform GetPreviousPointY()
    {
        BasicStacker RParentStacker = (ParentStacker as BasicStacker);

        if (RParentStacker != null)
        {
            int index = 1;

            int YCoordinate = Coordinate.y - index;

            for (; index <= Coordinate.y; index++)
            {

                if (RParentStacker.StackPoints.Matrix[Coordinate.x][YCoordinate][Coordinate.z] != null)
                {
                    return RParentStacker.StackPoints.Matrix[Coordinate.x][YCoordinate][Coordinate.z].transform;
                }

            }

        }

        return Parent.transform;
    }

    public virtual Transform GetPreviousPointZ()
    {
        BasicStacker RParentStacker = (ParentStacker as BasicStacker);

        if (RParentStacker != null)
        {
            int index = 1;

            int ZCoordinate = Coordinate.z - index;

            for (; index <= Coordinate.z; index++)
            {

                if (RParentStacker.StackPoints.Matrix[Coordinate.x][Coordinate.y][ZCoordinate] != null)
                {
                    return RParentStacker.StackPoints.Matrix[Coordinate.x][Coordinate.y][ZCoordinate].transform;
                }

            }

        }

        return Parent.transform;
    }
    #endregion
}
