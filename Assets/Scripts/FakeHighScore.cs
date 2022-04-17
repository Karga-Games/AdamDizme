using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FakeHighScore : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public string nameTag;

    public bool ShuffleNames;
    public List<string> fakeNames;
    public RectTransform Content;
    // Start is called before the first frame update
    void Start()
    {
        if (ShuffleNames)
        {
            RandomNames();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupRank(int rank)
    {
        rankText.text = rank.ToString();
        
    }

    public void ResetListPosition()
    {
        Content.LeanMoveY(0, 0.5f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void RandomNames()
    {
        List<GameObject> nickNameList = new List<GameObject>(GameObject.FindGameObjectsWithTag(nameTag));

        nickNameList.Shuffle();

        int i = 0;
        foreach(GameObject nickName in nickNameList)
        {

            if(i>= fakeNames.Count)
            {
                i = 0;
            }

            TextMeshProUGUI nameText = nickName.GetComponent<TextMeshProUGUI>();
            if(nameText != null)
            {
                nameText.text = fakeNames[i].ToString();
                i++;
            }

        }

    }
}
