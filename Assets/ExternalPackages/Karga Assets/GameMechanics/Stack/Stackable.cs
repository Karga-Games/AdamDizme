using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Stackable : MonoBehaviour
{
    public StackPoint LinkedPoint;
    public bool DestroyOnUnstack;
    public bool CanStack = false;
    public bool RigidbodyAfterUnlinked;
    public Rigidbody _rb;
    public Collider _collider;
    public GameObject DestroyEffect;


    public Vector3 WaveTargetScale;
    public float WaveTime;
    public bool WaveTransport;
    public float WaveTransportDelay;

    float WavePassedTime;
    Vector3 WaveOriginalScale;
    bool waving = false;
    int wavingStage = 0;
    public bool immortal = false;
    public bool UnlinkWave = true;
    // Start is called before the first frame update
    public virtual void Start()
    {

        _rb = GetComponent<Rigidbody>();
        if (RigidbodyAfterUnlinked)
        {
            if (_rb == null)
            {
                _rb = gameObject.AddComponent<Rigidbody>();
            }
        }

        if (_rb != null)
        {
            _rb.useGravity = false;
        }

        if (_collider == null)
        {
            _collider = GetComponent<Collider>();
        }

    }


    public virtual void FixedUpdate()
    {
        if (waving)
        {
            WaveUpdate();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
    }

    public virtual void WaveUpdate()
    {
        switch (wavingStage)
        {
            case 0:
                if ((WaveTargetScale - transform.localScale).magnitude > 0.1f)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, WaveTargetScale, Time.deltaTime * WaveTime);
                }
                else
                {
                    wavingStage = 1;
                }
                break;
            case 1:
                wavingStage = 2;
                break;
            case 2:
                if ((WaveOriginalScale - transform.localScale).magnitude > 0.1f)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, WaveOriginalScale, Time.deltaTime * WaveTime);
                }
                else
                {
                    waving = false;
                    wavingStage = 0;
                    transform.localScale = WaveOriginalScale;
                }
                break;
        }

    }

    public virtual void Link(StackPoint point)
    {
        if (_collider == null)
        {
            _collider = GetComponent<Collider>();
        }
        _collider.isTrigger = false;

        if (_rb != null)
        {
            _rb.isKinematic = true;
        }

        this.LinkedPoint = point;
        LinkedPoint.LinkedObject = this;

        this.transform.parent = LinkedPoint.gameObject.transform;
        
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        

    }

    public virtual void UnLink(bool deletePoint = false)
    {

        if (RigidbodyAfterUnlinked)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;

            this.transform.parent = null;
        }

        Stacker stacker = LinkedPoint.ParentStacker;

        if (stacker != null)
        {
            if (stacker._stackSpline != null)
            {
                stacker._stackSpline.Unlink(LinkedPoint);
            }
        }


        if (UnlinkWave)
        {
            Wave();
        }

        if (deletePoint)
        {
            transform.parent = null;
            Destroy(this.LinkedPoint.gameObject);
        }
        else
        {
            LinkedPoint.LinkedObject = null;
            LinkedPoint = null;
        }

        if (DestroyOnUnstack)
        {
            Destroy(gameObject);
        }
    }

    public void Wave(float delay = 0)
    {
        if (!waving)
        {
            WaveOriginalScale = transform.localScale;
            waving = true;

            if (WaveTransport)
            {
                WavePreviousPoint(WaveTransportDelay);
            }
            
        }
    }

    public void WavePreviousPoint(float delay = 0)
    {
        if (LinkedPoint != null)
        {
            if (LinkedPoint.previousStackPointZ != null)
            {
                LinkedPoint.ParentStacker.WaveStackable(LinkedPoint.previousStackPointZ.LinkedObject,delay);
            }
        }
    }

    public virtual void OnDestroy()
    {
        if (DestroyEffect != null)
        {
            Instantiate(DestroyEffect, transform.position, transform.rotation);
        }

    }


    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && !immortal)
        {
            if (_rb != null)
            {
                _rb.useGravity = true;
            }
            UnLink(true);
            Destroy(this);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Stackable otherStackable = other.GetComponent<Stackable>();
        if (otherStackable != null)
        {
            if (otherStackable.CanStack && this.LinkedPoint != null)
            {
                Stackable stackable = other.gameObject.GetComponent<Stackable>();
                if (stackable != null)
                {
                    if (stackable.LinkedPoint == null)
                    {
                        StackPoint stackPoint = this.LinkedPoint;

                        if (stackPoint != null)
                        {
                            Stacker stacker = this.LinkedPoint.ParentStacker;
                            if (stacker != null)
                            {
                                stacker.Stack(stackable);
                            }
                        }

                    }

                }

            }

        }
    }
}
