using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelEndPlane : MonoBehaviour
{

    private void OnValidate()
    {
        int index = 0;
        foreach(TextMeshPro text in GetComponentsInChildren<TextMeshPro>())
        {
            index++;
            text.text = "x" + index;
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
