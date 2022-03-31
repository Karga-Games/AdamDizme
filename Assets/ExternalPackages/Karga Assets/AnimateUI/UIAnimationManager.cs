using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIAnimationManager : MonoBehaviour
{
    public List<UIAnimationObject> AnimationObjects;

    public System.Action callbackFunction;

    public bool playOnAwake;
    public bool destroyOnFinish;

    protected void Start()
    {
        if (AnimationObjects.Count == 0)
        {
            foreach (UIAnimationObject animationObject in GetComponentsInChildren<UIAnimationObject>())
            {
                AnimationObjects.Add(animationObject);
            }
        }
        else
        {
            Debug.LogWarning("AnimationObjects List is already defined. Auto defining skipped");
        }

    }



}
