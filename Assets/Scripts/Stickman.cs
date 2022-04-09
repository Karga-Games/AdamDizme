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

    SkinnedMeshRenderer _srenderer;
    MeshRenderer _renderer;
    Animator animator;
    Vector3 velocity;
    Vector3 lastPosition;
    public BoxCollider _Bcollider;
    public Collider _deadCollider;
    Rigidbody _rigidbody;
    bool alive = true;

    public bool changeDeadCollider = false;

    float animationTime;
    // Start is called before the first frame update

    private void Awake()
    {

        animator = GetComponentInChildren<Animator>();

        _srenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        _renderer = GetComponentInChildren<MeshRenderer>();

        _rigidbody = GetComponent<Rigidbody>();

        alive = true;

    }
    
    // Update is called once per frame
    public void Update()
    {
        if (alive)
        {
            GoToDesiredPosition();

            CalculateVelocity();

            UpdateRotation();

            UpdateAnimation();
        }

        if(animator != null)
        {
            animator.SetBool("alive", alive);
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
            else if(velocity.sqrMagnitude > 0.01)
            {
                Run();
            }
            else
            {
                Idle();
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
        desiredPosition.followingStickman = this;
    }

    public void Join(CrowdController crowd)
    {
        SetCrowd(crowd);
        ChangeMaterial(collectedMaterial);
    }


    public void Dead()
    {
        ChangeMaterial(deadMaterial);
        if(crowd != null)
        {
            crowd.RemoveStickman(this, true, 0.2f);
        }
        crowd = null;
        transform.SetParent(null);
        desiredPosition.RemovePosition();
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        alive = false;

        if (changeDeadCollider)
        {
            _Bcollider.enabled = false;
            _deadCollider.enabled = true;
        }

    }

    public void ChangeMaterial(Material mat)
    {
        if(_srenderer != null)
        {
            _srenderer.material = mat;
        }

        if(_renderer != null)
        {
            _renderer.material = mat;
        }
    }

    
    public void OnCollisionEnter(Collision collision)
    {
        //base.OnCollisionEnter(collision);

        Stickman stickman = collision.gameObject.GetComponent<Stickman>();

        if(stickman != null)
        {
            if (stickman.crowd == null && crowd != null && stickman.alive)
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
