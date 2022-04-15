using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Hole : MonoBehaviour
{
    public int index;
    public GameObject HoleText;
    Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        HoleText = GetComponentInChildren<TextMeshPro>().gameObject;
        size= HoleText.transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        Stickman stickman = other.GetComponent<Stickman>();

        if(stickman != null)
        {
            stickman.InHole(transform.position-stickman.transform.position);
        }

        LeanTween.scale(HoleText,size * 1.5f,0.1f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.scale(HoleText,size,0.1f).setDelay(0.1f).setEase(LeanTweenType.easeInOutQuad);

        Account.Score(index);

    }
}
