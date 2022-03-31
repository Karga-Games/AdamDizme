using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class RoadComponentStackPoint : BasicStackPoint
{
   
    public Vector3 LastPos;
    protected float DesiredX;
    protected float DesiredY;
    protected float DesiredZ;

    public override void Awake()
    {
        LastPos = transform.position;
    }

    public override void CalculatePositionFromBase(Vector3 speed)
    {
        if (ParentStacker != null)
        {
            RoadComponentStacker RParentStacker = (ParentStacker as RoadComponentStacker);
            if(RParentStacker != null)
            {
                DesiredX = Mathf.Lerp(LastPos.x, Coordinate.x * this.Distance.x + RParentStacker.TablePositionOnRoad.x, Time.deltaTime * speed.x / Coordinate.z);
                LastPos.x = DesiredX;

                DesiredY = Mathf.Lerp(LastPos.y, Coordinate.y * this.Distance.y + RParentStacker.TablePositionOnRoad.y, Time.deltaTime * speed.y);
                LastPos.y = DesiredY;

                DesiredZ = Mathf.Lerp(LastPos.z, RParentStacker.TablePositionOnRoad.z + Coordinate.z * this.Distance.z, Time.deltaTime * speed.z);
                LastPos.z = DesiredZ;

                Vector3 zPosition = RParentStacker.RoadPlayer.roadGenerator.pathCreator.path.GetPointAtDistance(RParentStacker.RoadPlayer.roadGenerator.pathCreator.path.length - DesiredZ, EndOfPathInstruction.Loop);
                Vector3 yzPosition = zPosition + transform.right * DesiredX + transform.up * DesiredY;

                CurrentDesiredPosition = yzPosition;

                CurrentDesiredRotation = RParentStacker.RoadPlayer.roadGenerator.pathCreator.path.GetRotationAtDistance(RParentStacker.RoadPlayer.roadGenerator.pathCreator.path.length - DesiredZ, EndOfPathInstruction.Loop) * Quaternion.Euler(180, 0, 90);

            }


        }
    }

    public override void CalculatePositionFromPrevious(Vector3 speed)
    {

        DesiredLocalPosition PreviousX = GetPreviousPointX();
        DesiredLocalPosition PreviousY = GetPreviousPointY();
        DesiredLocalPosition PreviousZ = GetPreviousPointZ();


        if (ParentStacker != null)
        {

            Vector3 DesiredDistance = Distance;

            if(Coordinate.x == 0)
            {
                DesiredDistance.x = 0;
            }
            if (Coordinate.y == 0)
            {
                DesiredDistance.y = 0;
            }
            if (Coordinate.z == 0)
            {
                DesiredDistance.z = 0;
                if(Coordinate.x != 0)
                {
                    PreviousZ.DesiredX = PreviousX.DesiredX + Distance.x;
                }
            }

            RoadComponentStacker RParentStacker = (ParentStacker as RoadComponentStacker);
            if (RParentStacker != null)
            {
                DesiredX = Mathf.Lerp(LastPos.x, PreviousZ.DesiredX, Time.deltaTime * speed.x / 4);
                LastPos.x = DesiredX;

                DesiredY = Mathf.Lerp(LastPos.y, PreviousY.DesiredY + DesiredDistance.y, Time.deltaTime * speed.y / 4);
                LastPos.y = DesiredY;

                DesiredZ = Mathf.Lerp(LastPos.z, PreviousZ.DesiredZ + DesiredDistance.z, Time.deltaTime * speed.z / 4);
                LastPos.z = DesiredZ;

                Vector3 zPosition = RParentStacker.RoadPlayer.roadGenerator.pathCreator.path.GetPointAtDistance(RParentStacker.RoadPlayer.roadGenerator.pathCreator.path.length - DesiredZ, EndOfPathInstruction.Loop);
                Vector3 yzPosition = zPosition + transform.right * DesiredX + transform.up * DesiredY;

                CurrentDesiredPosition = yzPosition;

                CurrentDesiredRotation = RParentStacker.RoadPlayer.roadGenerator.pathCreator.path.GetRotationAtDistance(RParentStacker.RoadPlayer.roadGenerator.pathCreator.path.length - DesiredZ, EndOfPathInstruction.Loop) * Quaternion.Euler(180, 0, 90);

            }


        }

    }


    public new DesiredLocalPosition GetPreviousPointX()
    {
        RoadComponentStacker RParentStacker = (ParentStacker as RoadComponentStacker);

        DesiredLocalPosition localPos = new DesiredLocalPosition();

        if (RParentStacker != null)
        {
            int index = 1;

            int XCoordinate = Coordinate.x - index;

            for (; index <= Coordinate.x; index++)
            {

                if (RParentStacker.StackPoints.Matrix[XCoordinate][Coordinate.y][Coordinate.z] != null)
                {
                    RoadComponentStackPoint previousPoint = (RParentStacker.StackPoints.Matrix[XCoordinate][Coordinate.y][Coordinate.z] as RoadComponentStackPoint);
                    
                    localPos.DesiredX = previousPoint.DesiredX;
                    localPos.DesiredY = previousPoint.DesiredY;
                    localPos.DesiredZ = previousPoint.DesiredZ;

                    return localPos;

                }

            }

        }

        localPos.DesiredX = RParentStacker.TablePositionOnRoad.x;
        localPos.DesiredY = RParentStacker.TablePositionOnRoad.y;
        localPos.DesiredZ = RParentStacker.TablePositionOnRoad.z;

        return localPos;
    }

    public new DesiredLocalPosition GetPreviousPointY()
    {
        RoadComponentStacker RParentStacker = (ParentStacker as RoadComponentStacker);

        DesiredLocalPosition localPos = new DesiredLocalPosition();

        if (RParentStacker != null)
        {
            int index = 1;

            int YCoordinate = Coordinate.y - index;

            for (; index <= Coordinate.y; index++)
            {

                if (RParentStacker.StackPoints.Matrix[Coordinate.x][YCoordinate][Coordinate.z] != null)
                {
                    RoadComponentStackPoint previousPoint = (RParentStacker.StackPoints.Matrix[Coordinate.x][YCoordinate][Coordinate.z] as RoadComponentStackPoint);

                    localPos.DesiredX = previousPoint.DesiredX;
                    localPos.DesiredY = previousPoint.DesiredY;
                    localPos.DesiredZ = previousPoint.DesiredZ;

                    return localPos;

                }

            }

        }

        localPos.DesiredX = RParentStacker.TablePositionOnRoad.x;
        localPos.DesiredY = RParentStacker.TablePositionOnRoad.y;
        localPos.DesiredZ = RParentStacker.TablePositionOnRoad.z;

        return localPos;
    }

    public new DesiredLocalPosition GetPreviousPointZ()
    {
        RoadComponentStacker RParentStacker = (ParentStacker as RoadComponentStacker);

        DesiredLocalPosition localPos = new DesiredLocalPosition();

        if (RParentStacker != null)
        {
            int index = 1;

            int ZCoordinate = Coordinate.z - index;

            for (; index <= Coordinate.z; index++)
            {

                if (RParentStacker.StackPoints.Matrix[Coordinate.x][Coordinate.y][ZCoordinate] != null)
                {
                    RoadComponentStackPoint previousPoint = (RParentStacker.StackPoints.Matrix[Coordinate.x][Coordinate.y][ZCoordinate] as RoadComponentStackPoint);
                    previousStackPointZ = previousPoint;
                    localPos.DesiredX = previousPoint.DesiredX;
                    localPos.DesiredY = previousPoint.DesiredY;
                    localPos.DesiredZ = previousPoint.DesiredZ;

                    return localPos;

                }

            }

        }

        localPos.DesiredX = RParentStacker.TablePositionOnRoad.x;
        localPos.DesiredY = RParentStacker.TablePositionOnRoad.y;
        localPos.DesiredZ = RParentStacker.TablePositionOnRoad.z;

        return localPos;
    }


    public DesiredLocalPosition GetNextPointZ()
    {
        RoadComponentStacker RParentStacker = (ParentStacker as RoadComponentStacker);

        DesiredLocalPosition localPos = new DesiredLocalPosition();

        if (RParentStacker != null)
        {
            int index = 1;

            int ZCoordinate = Coordinate.z + index;

            for (; index < RParentStacker.Count; index++)
            {

                if (RParentStacker.StackPoints.Matrix[Coordinate.x][Coordinate.y][ZCoordinate] != null)
                {
                    RoadComponentStackPoint nextPoint = (RParentStacker.StackPoints.Matrix[Coordinate.x][Coordinate.y][ZCoordinate] as RoadComponentStackPoint);
                    nextStackPointZ = nextPoint;
                    localPos.DesiredX = nextPoint.DesiredX;
                    localPos.DesiredY = nextPoint.DesiredY;
                    localPos.DesiredZ = nextPoint.DesiredZ;

                    return localPos;

                }

            }

        }

        localPos.DesiredX = RParentStacker.TablePositionOnRoad.x;
        localPos.DesiredY = RParentStacker.TablePositionOnRoad.y;
        localPos.DesiredZ = RParentStacker.TablePositionOnRoad.z;

        return localPos;
    }

    public override void RefreshCoordinate(Vector3Int coord)
    {
        base.RefreshCoordinate(coord);
    }
}



public class DesiredLocalPosition
{
    public float DesiredX;
    public float DesiredY;
    public float DesiredZ;
}
