using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
[RequireComponent(typeof(SplineFollower))]
public class DreamteckRoadPlayerController : PlayerController
{
    public string StartingRoadTag;

    public float speed = 0.1f;
    public float MovementSmoothing = 0.1f;
    public float maxHorizontalPos = 1.0f;
    public float minHorizontalPos = -1.0f;
    public float dragSensitivity = 50.0f;

    public InputReader inputReader;
    public SplineComputer roadGenerator;
    protected SplineFollower follower;
    public float distanceTravelled;

    public float fingerDragging;
    public float horizontalPos;
    protected Rigidbody _rb;

    public bool atEndRoad = false;

    public GameObject MoveAsChild;
    public Vector3 MovementOffset;
    public bool active=true;
    float CurrentVelocity;
    // Start is called before the first frame update
    public override void Start()
    {

        follower = GetComponent<SplineFollower>();


        if (inputReader == null)
        {
            inputReader = FindObjectOfType<InputReader>();
        }

        if (roadGenerator == null && StartingRoadTag != "")
        {
            GameObject roadObj = GameObject.FindGameObjectWithTag(StartingRoadTag);
            if (roadObj != null)
            {
                roadGenerator = roadObj.GetComponent<SplineComputer>();
                follower.spline = roadGenerator;
            }
        }
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }

        if (roadGenerator == null)
        {
            Debug.LogWarning("There is no RoadMeshGenerator to follow!!");
        }
        follower.follow = false;
    }

    // Update is called once per frame
    public override void Update()
    {

        if (roadGenerator == null && StartingRoadTag != "")
        {
            roadGenerator = GameObject.FindGameObjectWithTag(StartingRoadTag).GetComponent<SplineComputer>();
        }

        if (follower.spline == null && roadGenerator != null)
        {
            follower.spline = roadGenerator;
        }

        //follower.follow = autoMove;
        //follower.followSpeed = speed;

        if (active)
        {
            if (inputReader != null)
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

    }

    public override void FixedUpdate()
    {

        

    }
    
    public virtual void UpdatePosition()
    {
        if (roadGenerator != null)
        {
            if (horizontalPos > maxHorizontalPos)
            {
                horizontalPos = maxHorizontalPos;
            }

            if (horizontalPos < minHorizontalPos)
            {
                horizontalPos = minHorizontalPos;
            }


            if(MoveAsChild != null)
            {
                MoveAsChild.transform.localPosition = Vector3.Lerp(MoveAsChild.transform.localPosition, (transform.right * horizontalPos) + MovementOffset, MovementSmoothing * Time.deltaTime);
            }
            else
            {
                follower.motion.offset = Vector3.Lerp(follower.motion.offset, transform.right * horizontalPos, MovementSmoothing * Time.deltaTime);
            }
            

            follower.SetDistance(distanceTravelled);

        }

    }

    public virtual void MoveForward()
    {
        distanceTravelled = distanceTravelled + speed * Time.deltaTime;
    }


    public virtual void ChangeRoad(SplineComputer newRoad, Vector2 MinMaxHorizontalPos = default(Vector2), float distance = 0, float speed = 0, bool endRoad = false)
    {
        DisconnectFromRoad();
        distanceTravelled = distance;
        //horizontalPos = posHorizontal;
        atEndRoad = endRoad;
        if (speed > 0)
        {
            this.speed = speed;
        }
        this.roadGenerator = newRoad;
        follower.spline = roadGenerator;
    }

    public virtual void DisconnectFromRoad()
    {
        this.roadGenerator = null;
    }
    

}
