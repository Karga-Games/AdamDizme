using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{

    public string name;

    public AudioClip clip;

    public bool playOnAwake;

    public bool loop;

    public bool refreshWithDelay;
    public float minRefreshTime;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

    protected float LastPlayedTime;

    public void Play()
    {
        if(source != null)
        {   
            
            if (refreshWithDelay)
            {
                if ((Time.time - LastPlayedTime) > minRefreshTime)
                {
                    LastPlayedTime = Time.time;
                    source.Play();
                }
            }
            else
            {
                source.Play();
            }
            
        }
        else
        {
            Debug.Log("Can't play. Sound ("+name+") is not loaded!");
        }
    }


    public void Stop()
    {
        if (source != null)
        {
            source.Stop();
        }
        else
        {
            Debug.Log("Can't stop. Sound (" + name + ") is not loaded!");
        }
    }

}
