using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    public CrowdController crowd;
    public StickmanPosition desiredPosition;
    public float movementSpeed;
    public float positionResolution;
    bool reached = false;


    public Material collectableMaterial;
    public Material collectedMaterial;
    public Material deadMaterial;

    SkinnedMeshRenderer _renderer;
    Animator animator;
    Vector3 velocity;
    Vector3 lastPosition;
    BoxCollider _collider;
    Rigidbody _rigidbody;
    bool alive = true;
    // Start is called before the first frame update

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();

        _collider = GetComponent<BoxCollider>();

        _rigidbody = GetComponent<Rigidbody>();

        alive = true;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            GoToDesiredPosition();

            CalculateVelocity();

            UpdateRotation();

            UpdateAnimation();
        }

    }

    public void CalculateVelocity()
    {
        velocity = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    public void UpdateRotation()
    {

        transform.rotation = Quaternion.LookRotation(new Vector3(velocity.x,0, velocity.z), Vector3.up);

    }

    public void GoToDesiredPosition()
    {
        if (desiredPosition != null)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPosition.transform.localPosition + new Vector3(0, desiredPosition.Height, 0), Time.deltaTime * movementSpeed);
        }
    }

    public void UpdateAnimation()
    {
        if (desiredPosition == null)
        {
            Idle();
        }
        else
        {
            if (transform.localPosition.y > 0+positionResolution)
            {
                Stand();
            }
            else if(velocity.sqrMagnitude > 0)
            {
                Run();
            }
            else
            {
                Stand();
            }

        }
    }

    public void Idle()
    {
        if (animator != null)
        {
            animator.SetInteger("status", 0);
        }
    }

    public void Run()
    {
        if(animator != null)
        {
            animator.SetInteger("status",1);
        }
    }

    public void Stand()
    {
        if (animator != null)
        {
            animator.SetInteger("status", 2);
        }
    }

    public void SetCrowd(CrowdController controller)
    {
        crowd = controller;
    }

    public void SetDesiredPosition(StickmanPosition position)
    {
        reached = false;

        desiredPosition = position;
    }

    public void Join(CrowdController crowd)
    {
        SetCrowd(crowd);
        ChangeMaterial(collectedMaterial);
    }


    public void Dead()
    {
        ChangeMaterial(deadMaterial);
        crowd.RemoveStickman(this);
        crowd = null;
        transform.SetParent(null);
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        alive = false;
    }

    public void ChangeMaterial(Material mat)
    {
        _renderer.material = mat;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Stickman stickman = collision.gameObject.GetComponent<Stickman>();

        if(stickman != null)
        {
            if (stickman.crowd == null)
            {
                crowd.AddStickman(stickman);
            }
            return;
        }

        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if(obstacle != null && crowd != null)
        {
            Dead();
            return;
        }

    }

}
