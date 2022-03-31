using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class CanvasCloner : MonoBehaviour
{

    public bool findCanvasAuto = true;
    public bool findCameraAuto = true;
    public Canvas OriginalCanvas;
    public Camera OriginalCamera;
    public float CanvasPlaneDistance;
    [SerializeField]
    public List<CanvasClone> Clones;

    public int HierarcyCount;
    private void Awake()
    {
        Setup();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Setup()
    {
        if (OriginalCanvas == null && findCanvasAuto)
        {
            OriginalCanvas = FindObjectOfType<Canvas>();
            HierarcyCount = OriginalCanvas.transform.childCount;
        }

        if (OriginalCamera == null && findCameraAuto)
        {
            OriginalCamera = FindObjectOfType<Camera>();
        }

        if (OriginalCanvas != null && OriginalCamera)
        {
            int index = 1;
            foreach (CanvasClone clone in Clones)
            {
                if (clone.active)
                {
                    Canvas ClonedCanvas = CloneCanvas(OriginalCanvas, CloneCamera(OriginalCamera, clone.CameraDisplayIndex, clone.CameraTag, "CloneCamera" + index), CanvasPlaneDistance, "CloneCanvas" + index);

                    CloneObjectRecursive(OriginalCanvas.gameObject, ClonedCanvas.gameObject);

                    clone.SetCanvas(ClonedCanvas);

                    index++;
                }
                
            }

        }
    }


    public void Reset()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int newHierarcyCount = OriginalCanvas.transform.childCount;

        if(HierarcyCount != newHierarcyCount)
        {
            HierarcyCount = newHierarcyCount;
            Setup();
        }
    }

    public Camera CloneCamera(Camera originalCamera, int targetDisplay, string cameraTag = null, string name = null)
    {

        Camera[] cameras = FindObjectsOfType<Camera>();

        foreach(Camera cam in cameras)
        {
            if(name == cam.name)
            {
                return cam;
            }
        }

        GameObject newObj = new GameObject();

        if (name != null)
        {
            newObj.name = name;
        }

        newObj.transform.SetParent(originalCamera.transform);
        newObj.transform.localPosition = Vector3.zero;
        newObj.transform.localEulerAngles = Vector3.zero;
        newObj.tag = cameraTag;

        Camera newCamera = newObj.AddComponent<Camera>();
        newCamera.targetDisplay = targetDisplay;

        return newCamera;

    }

    public Canvas CloneCanvas(Canvas originalCanvas, Camera cloneCamera, float PlaneDistance, string name = null)
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();

        foreach (Canvas canv in canvases)
        {
            if (name == canv.name)
            {
                return canv;
            }
        }

        GameObject newObj = new GameObject();

        if (name != null)
        {
            newObj.name = name;
        }

        newObj.transform.SetParent(transform);
        Canvas newCanvas = newObj.AddComponent<Canvas>();
        newCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        newCanvas.pixelPerfect = originalCanvas.pixelPerfect;
        newCanvas.worldCamera = cloneCamera;
        newCanvas.planeDistance = PlaneDistance;
        CanvasScaler originalCanvasScaler = originalCanvas.GetComponent<CanvasScaler>();
        CanvasScaler newCanvasScaler = newObj.AddComponent<CanvasScaler>();
        newCanvasScaler.uiScaleMode = originalCanvasScaler.uiScaleMode;
        newCanvasScaler.referenceResolution = originalCanvasScaler.referenceResolution;
        newCanvasScaler.screenMatchMode = originalCanvasScaler.screenMatchMode;
        newCanvasScaler.matchWidthOrHeight = originalCanvasScaler.matchWidthOrHeight;
        newCanvasScaler.referencePixelsPerUnit = originalCanvasScaler.referencePixelsPerUnit;


        return newCanvas;
    }

    public static void CloneObjectRecursive(GameObject OriginalObject, GameObject TargetObject) {

        foreach (Transform transform in GetTopLevelChildren(OriginalObject.transform))
        {
            GameObject newElement = new GameObject();
            newElement.name = transform.name;
            newElement.SetActive(transform.gameObject.activeSelf);
            newElement.transform.SetParent(TargetObject.transform);
            ClonedCanvasElement clonedElement = newElement.AddComponent<ClonedCanvasElement>();
            clonedElement.OriginalElement = transform.gameObject;

            OriginalCanvasElement originalElement;
            originalElement = transform.GetComponent<OriginalCanvasElement>();
            if(originalElement == null)
            {
                originalElement = transform.gameObject.AddComponent<OriginalCanvasElement>();
            }
            originalElement.AddClone(clonedElement);

            clonedElement.ReadComponents();
            
            CloneObjectRecursive(transform.gameObject, newElement);

        }

    }


    public static Transform[] GetTopLevelChildren(Transform Parent)
    {
        Transform[] Children = new Transform[Parent.childCount];
        for (int ID = 0; ID < Parent.childCount; ID++)
        {
            Children[ID] = Parent.GetChild(ID);
        }
        return Children;
    }
}



[System.Serializable]
public class CanvasClone
{
    public bool active;
    public int CameraDisplayIndex;
    public string CameraTag;
    [HideInInspector]
    public Canvas CloneCanvas;

    public void SetCanvas(Canvas canvas)
    {
        this.CloneCanvas = canvas;
    }


}

