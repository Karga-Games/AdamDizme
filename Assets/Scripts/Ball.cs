using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float movementSpeed;
    public Vector3 desiredPosition;
    public GameObject MeshHolder;

    [Header("Collectable Settings")]
    public Vector3 CollectableSize;
    public Material CollectableMaterial;
    public int CollectableBallLayerIndex;

    [Header("InCrowd Settings")]
    public Vector3 InCrowdSize;
    public Material InCrowdMaterial;
    public int InCrowdBallLayerIndex;

    [Header("Dead Settings")]
    public Vector3 DeadSize;
    public Material DeadMaterial;
    public int DeadBallLayerIndex;
    public bool dead;

    [Header("Column Data")]
    public CrowdColumn column;
    public int columnCoordinate;


    protected SkinnedMeshRenderer _srenderer;
    protected MeshRenderer _renderer;
    protected Rigidbody _rigidbody;
    protected bool tweening;
    // Start is called before the first frame update
    void Start()
    {
        FindRenderer();

        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(column != null && !tweening)
        {
            LerpToDesiredPosition();
        }
    }

    public void FindRenderer()
    {
        if (MeshHolder != null)
        {

        }
        _srenderer = MeshHolder.GetComponent<SkinnedMeshRenderer>();
        _renderer = MeshHolder.GetComponent<MeshRenderer>();
    }

    public void JoinToCrowd(BallCrowd crowd)
    {

        transform.SetParent(crowd.Balls.transform);

        ChangeSize(InCrowdSize);
        ChangeMaterial(InCrowdMaterial);
        ChangeLayer(InCrowdBallLayerIndex);
    }

    public void Dead()
    {
        LeanTween.cancel(gameObject);
        transform.SetParent(null);

        if (column != null)
        {
            column.RemoveBall(this);
            column = null;
            columnCoordinate = 0;
        }

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        ChangeSize(DeadSize);
        ChangeMaterial(DeadMaterial);
        ChangeLayer(DeadBallLayerIndex);

        dead = true;
    }

    public void SetColumn(CrowdColumn column, int coordinate)
    {
        this.column = column;
        columnCoordinate = coordinate;

        desiredPosition = column.transform.localPosition;
        desiredPosition.y = columnCoordinate * column.crowd.VerticalDistanceBetweenBalls;

        ChangeColor(FindCrowdColor());

        TweenToDesiredPosition();

    }

    public void LanceGlow()
    {
        Color initialColor;

        if(_renderer != null)
        {
            initialColor = _renderer.material.color;
        }

        if(_srenderer != null)
        {
            initialColor = _srenderer.material.color;
        }


    }

    public Color FindCrowdColor()
    {
        float p = (float)(column.coordinate * columnCoordinate) / (float)column.crowd.BallList.Count;

        return column.crowd.CrowdColorGradient.Evaluate(p);

    }

    public void LerpToDesiredPosition()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPosition, Time.deltaTime * movementSpeed);
        
    }

    public void TweenToDesiredPosition()
    {
        LeanTween.cancel(gameObject);
        tweening = true;
        float duration = Random.Range(0.5f, 0.8f);
        LeanTween.moveLocal(gameObject, desiredPosition, duration).setEase(FindTweenType()).setOnComplete(TweeningFinished);
    }

    public LeanTweenType FindTweenType()
    {
        LeanTweenType type;
        float yDiff = transform.localPosition.y - desiredPosition.y;
        float xDiff = transform.localPosition.x - desiredPosition.x;
        if (Mathf.Abs(yDiff) > Mathf.Abs(xDiff))
        {

            if (yDiff > 0)
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

        return type;
    }

    public void TweeningFinished()
    {
        tweening = false;
    }

    public void ChangeMaterial(Material mat)
    {

        if (_srenderer != null)
        {
            _srenderer.material = mat;
        }

        if (_renderer != null)
        {
            _renderer.material = mat;
        }

    }

    public void ChangeColor(Color color)
    {
        if (_srenderer != null)
        {
            _renderer.material.color = color;
        }

        if (_renderer != null)
        {
            _renderer.material.color = color;
        }
    }

    public void ChangeSize(Vector3 size)
    {
        transform.localScale = size;
    }

    public void ChangeLayer(int layer)
    {
        gameObject.layer = layer;
    }


    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.layer)
        {
            
            case 9:
                //Obstacle Layer
                Dead();

                break;
            case 3:
                //Collectable Ball

                Ball ball = collision.gameObject.GetComponent<Ball>();
                if(ball != null)
                {
                    column.crowd.AddBall(ball,true);
                }


                break;
        }
    }

    private void OnBecameInvisible()
    {
        if(dead)
        {
            Destroy(gameObject);
        }
    }
}
