using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BasicStacker : Stacker
{
    [HideInInspector]
    public StackPointMatrix<BasicStackPoint> StackPoints;
    public MatrixSearchOrder MatrixFillOrder;
    public StackPointFollowType FollowType;
    public Vector3Int MaxStackSize = Vector3Int.one;
    public Vector3 StackPosition;
    public Vector3 StackRotation;
    public Vector3 DistanceBetweenPoints = Vector3.one;
    public Vector3 DistanceAfterFirstElement = Vector3.zero;
    public Vector3 ParticleFollowTimeFactor = new Vector3(100, 100, 100);
    public float MovementSmoothing = 1f;
    protected GameObject StackTable;
    public int Count;


    public List<Axis> autoArrange;
    public Vector3 ArrangeSpeed = new Vector3(100,10,100);
    public float ArrangeTime = 0.5f;
    public List<Axis> Breakable;

    public bool FirstElementStatic = true;
    public bool FirstElementImmortal = true;

    public float WaveTransportDelay;
    public bool WaveTransport;

    // Start is called before the first frame update
    public virtual void Start()
    {
        if(ParticleFollowTimeFactor.x == 0)
        {
            ParticleFollowTimeFactor.x = 100;
            Debug.Log("Particle Follow Time Factor X should be greater than zero!");
        }
        if (ParticleFollowTimeFactor.y == 0)
        {
            ParticleFollowTimeFactor.y = 100;
            Debug.Log("Particle Follow Time Factor Y should be greater than zero!");
        }
        if (ParticleFollowTimeFactor.z == 0)
        {
            ParticleFollowTimeFactor.z = 100;
            Debug.Log("Particle Follow Time Factor Z should be greater than zero!");
        }

        InitializeMatrix();

    }

    public virtual void InitializeMatrix()
    {
        StackPoints = new StackPointMatrix<BasicStackPoint>(MaxStackSize.x, MaxStackSize.y, MaxStackSize.z);
        StackPoints.SearchOrder = MatrixFillOrder;
        StackTable = new GameObject();
        StackTable.name = "StackTable";
        StackTable.transform.parent = transform;
        StackTable.transform.eulerAngles = StackRotation;
        StackTable.transform.localPosition = StackPosition;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        int currentCount = StackPoints.PointCount();

        if(Count != currentCount)
        {
            Count = currentCount;
            if (autoArrange.Count > 0)
            {
                ReArrange();
            }
        }
       
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void WaveFromStart()
    {
        WaveFrom(new Vector3Int(0,0,0));
    }

    public virtual void WaveFromEnd()
    {
        //WaveFrom(StackPoints.FirstEmptyCoordinate(StackPoints.SearchOrder));
    }

    public virtual void WaveFrom(Vector3Int pos)
    {
        //StackPoints.Matrix[pos.x][pos.y][pos.z].Wave();
    }


    

    public virtual BasicStackPoint SpawnNewStackPoint(string pointName, Vector3 pos)
    {
        GameObject newStackPointObj = new GameObject(pointName);
        newStackPointObj.transform.position = pos;
        return newStackPointObj.AddComponent<BasicStackPoint>();

    }

    public override void Stack(Stackable stackable, string pointName = "BasicStackPoint")
    {
        bool firstElement = false;
        if(StackPoints.Matrix[0][0][0] == null)
        {
            firstElement = true;
        }


        BasicStackPoint newStackPoint = StackPoints.Add(SpawnNewStackPoint(pointName, stackable.transform.position));

        newStackPoint.followType = FollowType;
        if (firstElement)
        {
            stackable.immortal = FirstElementImmortal;
            if (FirstElementStatic)
            {
                newStackPoint.FollowSpeed = new Vector3(500, 500, 500);
            }
        }
        else
        {
            newStackPoint.FollowSpeed = new Vector3(ParticleFollowTimeFactor.x, ParticleFollowTimeFactor.y, ParticleFollowTimeFactor.z);
        }
        
        
        newStackPoint.FollowSmoothing = MovementSmoothing;

        if(newStackPoint.Coordinate.z == 1)
        {
            newStackPoint.Distance = DistanceAfterFirstElement;
        }
        else
        {
            newStackPoint.Distance = DistanceBetweenPoints;
        }

        newStackPoint.WaveTransportDelay = WaveTransportDelay;
        newStackPoint.WaveTransport = WaveTransport;
        newStackPoint.SetParent(StackTable.transform,this,false);
        stackable.WaveTransportDelay = WaveTransportDelay;
        stackable.WaveTransport = WaveTransport;
        stackable.Link(newStackPoint);

        if (_stackSpline != null)
        {
            _stackSpline.Stack(newStackPoint);
        }

    }


    public virtual void StackAt(Stackable stackable, Vector3Int Coordinate, bool force = false, string pointName = "BasicStackPoint")
    {
        //Verilen koordinata stackle
        BasicStackPoint StackPoint = StackPoints.GetPointAt(Coordinate);
        if(StackPoint != null)
        {
            if(StackPoint.LinkedObject != null)
            {
                // StackPoint dolu
                if (force)
                {
                    StackPoint.LinkedObject.UnLink();
                    StackPoint.Link(stackable);
                }
            }
            else
            {
                // StackPoint var ama içi bos
                StackPoint.Link(stackable);
            }
        }
        else
        {
            //StackPoint yok
            GameObject newStackPointObj = new GameObject(pointName);
            newStackPointObj.transform.position = stackable.transform.position;
            BasicStackPoint newStackPoint = StackPoints.AddTo(Coordinate,newStackPointObj.AddComponent<BasicStackPoint>());

            newStackPoint.followType = FollowType;
            newStackPoint.FollowSpeed = new Vector3(ParticleFollowTimeFactor.x / newStackPoint.Coordinate.y, ParticleFollowTimeFactor.y, ParticleFollowTimeFactor.z);
            newStackPoint.FollowSmoothing = MovementSmoothing;
            newStackPoint.Distance = DistanceBetweenPoints;
            newStackPoint.WaveTransportDelay = WaveTransportDelay;
            newStackPoint.WaveTransport = WaveTransport;
            newStackPoint.SetParent(StackTable.transform,this);

            stackable.Link(newStackPoint);
        }

    }

    public void BreakFrom(Vector3Int breakPoint)
    {
        foreach (Axis axis in Breakable)
        {
            switch (axis)
            {
                case Axis.X:

                    for (int i = breakPoint.x + 1; i < StackPoints.MaxRow; i++)
                    {
                        BasicStackPoint stackpoint = StackPoints.Matrix[i][breakPoint.y][breakPoint.z];
                        if (stackpoint != null)
                        {
                            stackpoint.DestroyOnUnlink = true;
                            stackpoint.UnLink();
                        }
                        i++;
                    }

                    break;
                case Axis.Y:
                    for (int i = breakPoint.y + 1; i < StackPoints.MaxColumn; i++)
                    {
                        BasicStackPoint stackpoint = StackPoints.Matrix[breakPoint.x][i][breakPoint.z];
                        if (stackpoint != null)
                        {
                            stackpoint.DestroyOnUnlink = true;
                            stackpoint.UnLink();
                        }
                        i++;
                    }
                    break;
                case Axis.Z:
                    for (int i = breakPoint.z + 1; i < StackPoints.MaxHeight; i++)
                    {
                        BasicStackPoint stackpoint = StackPoints.Matrix[breakPoint.x][breakPoint.y][i];
                        if (stackpoint != null)
                        {
                            stackpoint.DestroyOnUnlink = true;
                            stackpoint.UnLink();
                        }
                        i++;
                    }
                    break;
            }
        }

    }

    public virtual void Multiply(Vector3Int Factor)
    {

        int rowToAdd = StackPoints.MaxRow * Factor.x -StackPoints.MaxRow;

        MatrixSearchOrder defaultOrder = StackPoints.SearchOrder;

        StackPointMatrix<BasicStackPoint>.Multiply(ref StackPoints, Factor);
        
        int PointCount = StackPoints.PointCount();
        Vector3Int PointsToAdd = PointCount * Factor - (PointCount * Vector3Int.one);

        StackPoints.SearchOrder = MatrixSearchOrder.XZ;
        BulkStack(PointsToAdd.x);

        StackPoints.SearchOrder = MatrixSearchOrder.XZ;
        BulkStack(PointsToAdd.y);

        StackPoints.SearchOrder = MatrixSearchOrder.ZX;
        BulkStack(PointsToAdd.z);

        StackPoints.SearchOrder = defaultOrder;

        StackTable.transform.position = new Vector3(StackTable.transform.position.x - (rowToAdd * DistanceBetweenPoints.x)/2, StackTable.transform.position.y, StackTable.transform.position.z); 

    }
    public void BulkStack(int PointsToAdd)
    {
        int AddedPointCount = 0;
        for (int i = 0; i < StackPoints.MaxRow; i++)
        {
            for (int j = 0; j < StackPoints.MaxColumn; j++)
            {
                for (int k = 0; k < StackPoints.MaxHeight; k++)
                {
                    if (StackPoints.Matrix[i][j][k] != null)
                    {
                        if (StackPoints.Matrix[i][j][k].LinkedObject != null)
                        {
                            Stack(Instantiate(StackPoints.Matrix[i][j][k].LinkedObject.gameObject).GetComponent<Stackable>());
                            AddedPointCount++;

                            if(AddedPointCount >= PointsToAdd)
                            {
                                return;
                            }
                        }

                    }
                }
            }
        }

    }

    public void ReArrange(Axis[] AxisArray = null)
    {

        TemporaryFollowSpeed(ArrangeSpeed,ArrangeTime);
        if (AxisArray == null)
        {
            AxisArray = autoArrange.ToArray();
        }

        StackPoints.ReArrange(AxisArray);

    }


    public void TemporaryFollowSpeed(Vector3 temporarySpeed,float time)
    {
        for (int i = 0; i < StackPoints.MaxRow; i++)
        {
            for (int j = 0; j < StackPoints.MaxColumn; j++)
            {
                for (int k = 0; k < StackPoints.MaxHeight; k++)
                {
                    if (StackPoints.Matrix[i][j][k] != null)
                    {
                        StackPoints.Matrix[i][j][k].FollowSpeed = temporarySpeed;
                    }
                }
            }
        }

        StartCoroutine(GeneralFunctions.executeAfterSec(() => {

            for (int i = 0; i < StackPoints.MaxRow; i++)
            {
                for (int j = 0; j < StackPoints.MaxColumn; j++)
                {
                    for (int k = 0; k < StackPoints.MaxHeight; k++)
                    {
                        if (StackPoints.Matrix[i][j][k] != null)
                        {
                            StackPoints.Matrix[i][j][k].FollowSpeed = ParticleFollowTimeFactor;
                        }
                    }
                }
            }

        }, time));

    }

    public virtual void OnTriggerEnter(Collider other)
    {

        BasicStackable stackable = other.gameObject.GetComponent<BasicStackable>();
        if (stackable != null)
        {
            if (stackable.LinkedPoint == null)
            {
                Stack(stackable);
            }
        }


    }


}


