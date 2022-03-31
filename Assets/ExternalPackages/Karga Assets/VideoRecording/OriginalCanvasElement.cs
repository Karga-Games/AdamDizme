using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalCanvasElement : MonoBehaviour
{
    [SerializeField]
    public List<ClonedCanvasElement> Clones;
    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddClone(ClonedCanvasElement element)
    {
        if (Clones == null)
        {
            Clones = new List<ClonedCanvasElement>();
        }

        Clones.Add(element);
    }

    private void OnEnable()
    {
        if (Clones == null)
        {
            Clones = new List<ClonedCanvasElement>();
        }
        foreach (ClonedCanvasElement element in Clones)
        {
            if(gameObject != null && element != null)
            {
                element.gameObject.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        if (Clones == null)
        {
            Clones = new List<ClonedCanvasElement>();
        }
        foreach (ClonedCanvasElement element in Clones)
        {
            if (gameObject != null && element != null)
            {
                element.gameObject.SetActive(false);
            }
        }
    }
}
