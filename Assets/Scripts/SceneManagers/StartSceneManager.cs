using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartSceneManager : MonoBehaviour
{
    public int targetFPS;
    public string targetScene;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFPS;

        SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
