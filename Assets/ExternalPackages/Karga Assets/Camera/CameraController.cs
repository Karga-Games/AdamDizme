using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LockBools
{
    public bool X;
    public bool Y;
    public bool Z;
}

[System.Serializable]
public enum CameraModes
{
    Normal,
    WASD
}

public class CameraController : MonoBehaviour
{

    #region public variables

    [SerializeField]
    public CameraModes modeName;
    public float CameraSmoothTime = 0.1f;
    [Header("Follow Variables")]
    public Transform objectToFollow;
    public Vector3 FollowOffset = new Vector3(0, 4.5f, -5f);
    public float followSpeed = 0.1f;
    public bool FollowAsChild = true;
    public bool FollowObjectRotation = false;
    public Vector3 CurrentVelocity;
    [Header("Look Variables")]
    public Transform objectToLook;
    public Vector3 TargetOffset = new Vector3(0, 3.0f, 0f);
    public float lookSpeed = 0.1f;
    public bool LookAsChild = false;


    [Header("Lock Camera")]
    [SerializeField]
    public LockBools LockPosition;
    public LockBools LockRotation;


    [Header("FOV")]
    public float defaultFOV = 60f;
    public float FOVChangeSpeed = 50f;
    #endregion

    #region private variables
    private Camera CameraComp;
    protected CameraMode Mode;

    private Dictionary<CameraModes, CameraMode> CameraModeClasses = new Dictionary<CameraModes, CameraMode>()
    {
        { CameraModes.Normal, new NormalCameraMode() },
        { CameraModes.WASD, new WASDCameraMode() }
    };


    #endregion

    void Awake()
    {
        CameraComp = this.GetComponent<Camera>();

        ChangeModeByName(modeName);

    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Mode.FixedUpdate();
    }

    void Update()
    {
        Mode.Update();
    }

    private void LateUpdate()
    {
        Mode.LateUpdate();
    }

    #region Setup Functions
    public void SetupCamera(Transform followTarget, CameraMode mode)
    {

        SetupFollowTarget(followTarget);

        if (objectToLook == null)
        {
            SetupLookTarget(followTarget);
        }

        ChangeModeByClass(mode);
    }

    public void SetupCamera(Transform followTarget, Transform lookTarget, CameraMode mode)
    {
        SetupFollowTarget(followTarget);

        SetupLookTarget(lookTarget);

        ChangeModeByClass(mode);
    }

    public void SetupCamera(Transform followTarget, CameraModes mode)
    {

        SetupFollowTarget(followTarget);

        if (objectToLook == null)
        {
            SetupLookTarget(followTarget);
        }

        ChangeModeByName(mode);
    }

    public void SetupCamera(Transform followTarget, Transform lookTarget, CameraModes mode)
    {
        SetupFollowTarget(followTarget);

        SetupLookTarget(lookTarget);

        ChangeModeByName(mode);
    }

    public void SetupFollowTarget(Transform obj)
    {
        objectToFollow = obj;
    }

    public void SetupLookTarget(Transform obj)
    {
        objectToLook = obj;
    }

    public void ChangeMode(CameraMode mode)
    {
        this.Mode = mode;
        this.Mode.Start(this);
    }

    public void ChangeModeByClass(CameraMode mode, bool force = false)
    {
        if (this.Mode != null)
        {
            if (force || this.Mode.GetType().Name != mode.GetType().Name)
            {
                this.Mode.Exit();
                ChangeMode(mode);
            }
        }
        else
        {
            ChangeMode(mode);
        }

    }


    public void ChangeModeByName(CameraModes mode)
    {
        ChangeModeByClass(CameraModeClasses[mode]);
    }
    #endregion

    #region helper functions
    public void updateFOV(float FOV, float t = 50f)
    {
        if (FOV != CameraComp.fieldOfView)
        {
            CameraComp.fieldOfView = Mathf.Lerp(CameraComp.fieldOfView, FOV, t * Time.deltaTime);
        }
    }

    public void LookAtTarget()
    {
        if (objectToLook == null)
        {
            return;
        }
        Vector3 targetDirection;

        if (LookAsChild)
        {
            targetDirection = objectToLook.position + objectToLook.forward * TargetOffset.z + objectToLook.right * TargetOffset.x + objectToLook.up * TargetOffset.y - transform.position;
        }
        else
        {
            targetDirection = (objectToLook.position + TargetOffset) - transform.position;
        }

        Vector3 desiredEulerRotation = Quaternion.LookRotation(targetDirection, Vector3.up).eulerAngles;

        desiredEulerRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(desiredEulerRotation), lookSpeed).eulerAngles;

        ChangeRotation(desiredEulerRotation);


    }

    public void MoveToTarget()
    {
        if (objectToFollow == null)
        {
            return;
        }
        Vector3 targetPosition;
        if (FollowAsChild)
        {
            targetPosition = objectToFollow.position + objectToFollow.forward * FollowOffset.z + objectToFollow.right * FollowOffset.x + objectToFollow.up * FollowOffset.y;
        }
        else
        {
            targetPosition = objectToFollow.position + FollowOffset;
        }

        targetPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed);

        ChangePosition(targetPosition);

        if (FollowObjectRotation)
        {
            ChangeRotation(objectToFollow.rotation.eulerAngles);
        }

    }

    public void ChangePosition(Vector3 pos)
    {

        if (LockPosition.X)
        {
            pos.x = transform.position.x;
        }
        if (LockPosition.Y)
        {
            pos.y = transform.position.y;
        }
        if (LockPosition.Z)
        {
            pos.z = transform.position.z;
        }

        transform.position = Vector3.SmoothDamp(transform.position, pos, ref CurrentVelocity, CameraSmoothTime);
    }

    public void ChangeRotation(Vector3 rot)
    {

        if (LockRotation.X)
        {
            rot.x = transform.rotation.eulerAngles.x;
        }
        if (LockRotation.Y)
        {
            rot.y = transform.rotation.eulerAngles.y;
        }
        if (LockRotation.Z)
        {
            rot.z = transform.rotation.eulerAngles.z;
        }

        transform.rotation = Quaternion.Euler(rot);
    }

    #endregion
}
