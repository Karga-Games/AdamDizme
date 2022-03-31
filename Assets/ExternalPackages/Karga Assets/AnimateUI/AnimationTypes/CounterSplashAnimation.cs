using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CounterAnimStatus
{
    None,
    Starting,
    Counting,
    Finishing
}


public class CounterSplashAnimation : UIAnimationManager
{

    public int counter = 0;

    public string counterPrefix = "";

    private bool counterAnimating = false;

    public bool animationFinished = false;

    public Sound startingSound;

    public Sound countingLoopSound;

    public Sound finishingSound;

    [SerializeField]
    public UIAnimationText CounterObject;
    [SerializeField]
    public CounterAnimStatus counterStatus = CounterAnimStatus.None;

    protected void Start()
    {
        base.Start();

        LoadSound(startingSound);
        LoadSound(countingLoopSound);
        LoadSound(finishingSound);

        if (playOnAwake)
        {
            StartAnimation();
        }

    }

    protected void Update()
    {
        if (!animationFinished)
        {
            if (counterStatus != CounterAnimStatus.Finishing)
            {
                UpdateCounter();
            }

            if (counterStatus == CounterAnimStatus.Counting)
            {
                UpdateCounterAnimation();
            }

            if(counterStatus == CounterAnimStatus.Finishing && !counterAnimating)
            {
                FinishAnimation();
            }
        }

    }

    public void StartAnimation()
    {

        PlaySound(startingSound);
        animationFinished = false;
        counterStatus = CounterAnimStatus.Starting;
        float startAnimationDuration = 0;
        foreach (UIAnimationObject animationObject in AnimationObjects)
        {
            float animationDuration = animationObject.PlayCommandsByGroup(UIMovementGroup.StartingMovement);
            if (startAnimationDuration < animationDuration)
            {
                startAnimationDuration = animationDuration;
            }
        }

        StartCoroutine(GeneralFunctions.executeAfterSec(()=> { StopSound(startingSound); PlaySound(countingLoopSound); counterStatus = CounterAnimStatus.Counting; },startAnimationDuration));
    }

    public void UpdateCounterAnimation()
    {
        if (!counterAnimating)
        {
            counterAnimating = true;

            float counterAnimationDuration = 0;
            foreach (UIAnimationObject animationObject in AnimationObjects)
            {
                float animationDuration = animationObject.PlayCommandsByGroup(UIMovementGroup.IdleMovement);
                if (counterAnimationDuration < animationDuration)
                {
                    counterAnimationDuration = animationDuration;
                }
            }

            StartCoroutine(GeneralFunctions.executeAfterSec(() => { counterAnimating = false; }, counterAnimationDuration));

        }
       
    }

    public void UpdateCounter()
    {
        CounterObject.changeText(counterPrefix + counter.ToString(),0);
    }


    public void FinishAnimation()
    {
        StopSound(countingLoopSound);
        PlaySound(finishingSound);
        animationFinished = true;

        float finishingAnimationDuration = 0;
        foreach (UIAnimationObject animationObject in AnimationObjects)
        {
            float animationDuration = animationObject.PlayCommandsByGroup(UIMovementGroup.FinishingMovement);
            if (finishingAnimationDuration < animationDuration)
            {
                finishingAnimationDuration = animationDuration;
            }
        }

        if (destroyOnFinish)
        {
            StartCoroutine(GeneralFunctions.executeAfterSec(() => { if (callbackFunction != null) { callbackFunction(); } Destroy(gameObject); }, finishingAnimationDuration));
        }

    }

    public void LoadSound(Sound s)
    {
        if(s.clip != null)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if (s.playOnAwake)
            {
                s.Play();
            }
        }
       
    }

    public void PlaySound(Sound s)
    {
        if(s.source != null)
        {
            s.source.Play();
        }
    }

    public void StopSound(Sound s)
    {
        if (s.source != null)
        {
            s.source.Stop();
        }
    }
}
