using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    public CrowdController crowd;
    public StickmanPosition desiredPosition;
    public float movementSpeed;
    public float positionResolution;


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
    bool tweening = false;
    public bool changeDeadCollider = false;
    bool free = false;
    public Gradient colorGradient;
    public Vector3 freeVelocity;
    public float freeY;
    public float freeZ;
    Vector3 desiredFreePos;

    float lastReflection;

    public GameObject Trail;
    Color ballColor;
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

        CalculateVelocity();

        if (alive)
        {
            GoToDesiredPosition();

            UpdateRotation();

            UpdateAnimation();

            UpdateColor();
        }

        if (free)
        {
            FreeMove();
        }

        if(animator != null)
        {
            animator.SetBool("alive", alive);
        }

        lastReflection += Time.deltaTime;


        if (!free && !alive && desiredPosition == null)
        {
            transform.SetParent(null);
            if (LeanTween.isTweening(gameObject))
            {
                LeanTween.cancel(gameObject);
            }
        }
    }

    public void CalculateVelocity()
    {
        velocity = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    public void UpdateRotation()
    {
        Vector3 movement = new Vector3(velocity.x, 0, velocity.z);
        if (movement != Vector3.zero)
        {

            transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
        }

    }

    public void UpdateColor()
    {
        if (crowd != null && desiredPosition != null)
        {
            float xIndex = (desiredPosition.ListCoordinate.x);
            float yIndex = (desiredPosition.ListCoordinate.y);

            if(xIndex <= 0)
            {
                xIndex = 1;
            }
            if(yIndex <= 0)
            {
                yIndex = 1;
            }


            float p = (float)(xIndex * yIndex) / (float)crowd.StickmanCount();

            ballColor = colorGradient.Evaluate(p);
            if (_srenderer != null)
            {
                _renderer.material.color = ballColor;
            }

            if (_renderer != null)
            {
                _renderer.material.color = ballColor;
            }
        }
    }


    public void TweenToDesiredPosition(LeanTweenType type =  LeanTweenType.easeOutBack)
    {
        
        float yDiff = transform.localPosition.y - (desiredPosition.transform.localPosition + new Vector3(0, desiredPosition.Height, 0)).y;
        float xDiff = transform.localPosition.x - (desiredPosition.transform.localPosition + new Vector3(0, desiredPosition.Height, 0)).x;
        if (Mathf.Abs(yDiff) > Mathf.Abs(xDiff)) {

            if(yDiff > 0)
            {
                type = LeanTweenType.easeOutBounce;
            }
            else
            {
                type = LeanTweenType.easeOutBack;
            }

        }
        else
        {
            type = LeanTweenType.easeOutBack;
        }

        tweening = true;
        float duration = Random.Range(0.5f, 0.8f);
        LeanTween.moveLocal(gameObject, desiredPosition.transform.localPosition + new Vector3(0, desiredPosition.Height, 0), duration).setEase(type).setOnComplete(TweeningFinished);

    }
    public void TweeningFinished()
    {
        tweening = false;
    }

    public void GoToDesiredPosition()
    {
        if (desiredPosition != null && !tweening)
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

    public void Free()
    {
        alive = false;
        gameObject.layer = 3;
        freeVelocity = new Vector3(Random.Range(-2f, 2f), 0, 0);
        free = true;
        freeY = transform.localPosition.y;
        freeZ = transform.localPosition.y;
        Trail.SetActive(true);
        Color trailColor = ballColor;
        trailColor.a = 0.1f;
        Trail.GetComponent<TrailRenderer>().material.color = trailColor;
        LeanTween.value(gameObject, freeY, 0f, 0.2f * desiredPosition.ListCoordinate.y).setOnUpdate((float val) =>
        {

            freeY = val;

        }).setEase(LeanTweenType.easeOutBounce);

        desiredPosition = null;

        desiredFreePos = transform.localPosition;

        desiredFreePos.z = freeZ;

    }

    public void FreeMove()
    {
        Vector3 cdesiredFreePos = transform.localPosition + freeVelocity * Time.deltaTime * 5f;
        cdesiredFreePos.y = freeY;
        cdesiredFreePos.z = freeZ;

        transform.localPosition = cdesiredFreePos;
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
        desiredPosition = position;
        desiredPosition.followingStickman = this;

    }

    public void Join(CrowdController crowd)
    {
        SetCrowd(crowd);
        ChangeMaterial(collectedMaterial);

        gameObject.layer = 6;
    }

    public void Dead()
    {
        LeanTween.cancel(gameObject);
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

    public void InHole(Vector3 speed)
    {
        free = false;

        transform.SetParent(null);

        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(speed,ForceMode.VelocityChange);
        _rigidbody.angularVelocity = Vector3.zero;
        
        if(crowd != null)
        {
            crowd.StickmanList.Remove(this);
        }

    }

    private void OnBecameInvisible()
    {
        
        
        if(!alive)
        {

            if(crowd != null)
            {
                crowd.StickmanList.Remove(this);
            }

            Destroy(gameObject);

        }
        
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        //base.OnCollisionEnter(collision);

        if (free)
        {
            if (collision.gameObject.tag == "Wall" && lastReflection > 0.1f)
            {

                Vector3 newvel = Vector3.Reflect(freeVelocity, collision.contacts[0].normal);

                freeVelocity = newvel;

                lastReflection = 0;

            }

        }
        else
        {
            Stickman stickman = collision.gameObject.GetComponent<Stickman>();

            if (stickman != null)
            {
                if (stickman.crowd == null && crowd != null && stickman.alive)
                {
                    crowd.AddStickman(stickman);
                }
                return;
            }

            Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
            if (obstacle != null && crowd != null)
            {
                Dead();
                return;
            }
        }


    }
    
}
