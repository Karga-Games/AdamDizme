using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSText : MonoBehaviour
{
    private TextMeshProUGUI fpsText;

    // Start is called before the first frame update
    void Start()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        fpsText.text = "FPS: " + (int)(1f / Time.unscaledDeltaTime);
    }
}
