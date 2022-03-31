using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlayerController : PlayerController
{

    public string StartingRoadTag;

    
    public float speed = 0.1f;
    public float MovementSmoothing = 0.1f;
    public float maxHorizontalPos = 1.0f;
    public float minHorizontalPos = -1.0f;
    public float dragSensitivity = 50.0f;

    public InputReader inputReader;
    public RoadMeshGenerator roadGenerator;
    public float distanceTravelled;
    public EndOfPathInstruction endOfPathInstruction;

    public float fingerDragging;
    public float horizontalPos;
    protected Rigidbody _rb;

    public bool atEndRoad=false;
    // Start is called before the first frame update
    public override void Start()
    {
        
        if(inputReader == null)
        {
            inputReader = FindObjectOfType<InputReader>();
        }

        if(roadGenerator == null && StartingRoadTag != "")
        {
            GameObject roadObj = GameObject.FindGameObjectWithTag(StartingRoadTag);
            if(roadObj != null)
            {
                roadGenerator = roadObj.GetComponent<RoadMeshGenerator>();
            }
        }
        if(_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }

        if(roadGenerator == null)
        {
            Debug.LogError("There is no RoadMeshGenerator to follow!!");
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (roadGenerator == null && StartingRoadTag != "")
        {
            roadGenerator = GameObject.FindGameObjectWithTag(StartingRoadTag).GetComponent<RoadMeshGenerator>();
        }
    }

    public override void FixedUpdate()
    {
        
        if(inputReader != null)
        {
            fingerDragging = inputReader.horizontalInput;
            horizontalPos += fingerDragging * dragSensitivity;

            if (autoMove)
            {
                MoveForward();
            }
            else
            {
                if (inputReader.currentInput != null)
                {
                    if (inputReader.currentInput.touching)
                    {
                        MoveForward();
                    }
                }
            }
        }
        
        UpdatePosition();
    }

    public virtual void UpdatePosition()
    {
        if(roadGenerator != null)
        {
            if (horizontalPos > maxHorizontalPos)
            {
                horizontalPos = maxHorizontalPos;
            }

            if(horizontalPos < minHorizontalPos)
            {
                horizontalPos = minHorizontalPos;
            }


            
            Vector3 zPosition = roadGenerator.pathCreator.path.GetPointAtDistance(roadGenerator.pathCreator.path.length - distanceTravelled + roadGenerator.distanceOffset, endOfPathInstruction);
            Vector3 yzPosition = zPosition + transform.right * horizontalPos;

            if(_rb != null)
            {
                _rb.MovePosition(Vector3.Lerp(_rb.position, yzPosition, MovementSmoothing));
                _rb.MoveRotation(roadGenerator.pathCreator.path.GetRotationAtDistance(roadGenerator.pathCreator.path.length - distanceTravelled + roadGenerator.distanceOffset, endOfPathInstruction) * Quaternion.Euler(180, 0, 90));
            }
            else
            {
                
                transform.position = Vector3.Lerp(transform.position, yzPosition, MovementSmoothing);

                transform.rotation = roadGenerator.pathCreator.path.GetRotationAtDistance(roadGenerator.pathCreator.path.length - distanceTravelled + roadGenerator.distanceOffset, endOfPathInstruction) * Quaternion.Euler(180, 0, 90);

            }


        }
       
    }

    public virtual void MoveForward()
    {
        distanceTravelled = distanceTravelled + speed;
    }


    public virtual void ChangeRoad(RoadMeshGenerator newRoad, Vector2 MinMaxHorizontalPos = default(Vector2), float distance = 0, float speed = 0, bool endRoad=false)
    {
        DisconnectFromRoad();
        distanceTravelled = distance;
        //horizontalPos = posHorizontal;
        atEndRoad = endRoad;
        if(speed > 0)
        {
            this.speed = speed;
        }
        this.roadGenerator = newRoad;
    }

    public virtual void DisconnectFromRoad()
    {
        this.roadGenerator = null;
    }



}
