using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommon : MonoBehaviour
{
    public static GameObject SpawnFullScreenPrefab(GameObject Prefab, Canvas canvas)
    {

        GameObject newUIObject = Instantiate(Prefab, Vector3.zero, Quaternion.identity);
        newUIObject.transform.SetParent(canvas.transform);

        newUIObject.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        newUIObject.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        newUIObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        newUIObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        newUIObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        return newUIObject;

    }

}
