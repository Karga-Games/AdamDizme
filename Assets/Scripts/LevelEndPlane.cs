using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelEndPlane : MonoBehaviour
{
    public GameObject HighScore;
    private void OnValidate()
    {
        //SetupLevelEnd();
    }


    public void SetupLevelEnd()
    {
        int index = 0;
        foreach (Hole hole in GetComponentsInChildren<Hole>())
        {
            index++;
            hole.GetComponentInChildren<TextMeshPro>().text = "x" + index;

            hole.index = index;

            if(Account.HighScore == index && HighScore!=null)
            {
                Vector3 pos = hole.transform.position + new Vector3(0, 0.4f, 0);

                pos.x = 0;
                
                HighScore.transform.position = pos;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetupLevelEnd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
