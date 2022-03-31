using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplashAnimation : UIAnimationManager
{

    protected void Start()
    {
        base.Start();

        if (playOnAwake)
        {
            PlayAnimation();
        }

    }

    public void PlayAnimation()
    {
        float destroyDelay = 0;

        foreach (UIAnimationObject animationObject in AnimationObjects)
        {
            
            float animationDuration = animationObject.PlayCommandsSimple();
            if (destroyDelay < animationDuration)
            {
                destroyDelay = animationDuration;
            }

        }


        if (destroyOnFinish)
        {
            StartCoroutine(GeneralFunctions.executeAfterSec(()=> { if (callbackFunction != null) { callbackFunction(); }  Destroy(gameObject); },destroyDelay));
        }


    }

}

