using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Facebook.Unity;

public class FacebookManager : MonoBehaviour
{/*
    static FacebookManager instance;
    void Awake()
    {

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }

        DontDestroyOnLoad(gameObject);
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void LevelStarted(string levelname)
    {
        var logParams = new Dictionary<string, object>();
        logParams["Level"] = levelname;

        FB.LogAppEvent(
            "Level Started",
            parameters: logParams
        );

        Debug.LogWarning("FBLevelStartedLog"+logParams["Level"]);
    }

    public void LevelCompleted(string levelname)
    {
        var logParams = new Dictionary<string, object>();
        logParams["Level"] = levelname;

        FB.LogAppEvent(
            "Level Completed",
            parameters: logParams
        );

        Debug.LogWarning("FBLevelCompletedLog"+logParams["Level"]);
    }

    public void LevelRestart(string levelname)
    {
        var logParams = new Dictionary<string, object>();
        logParams["Level"] = levelname;

        FB.LogAppEvent(
            "Level Restart",
            parameters: logParams
        );

        Debug.LogWarning("FBLevelRestartLog"+logParams["Level"]);
    }

    public void LevelFailed(string levelname)
    {
        var logParams = new Dictionary<string, object>();
        logParams["Level"] = levelname;

        FB.LogAppEvent(
            "Level Failed",
            parameters: logParams
        );

        Debug.LogWarning("FBLevelFailedLog" + logParams["Level"]);
    }
    */
}
