using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool AutoLoadSounds;

    public Sound[] Sounds;

    protected bool isSoundsLoaded;

    public virtual void Awake()
    {

        if (AutoLoadSounds)
        {
            LoadSounds();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void LoadSounds(bool refresh = false)
    {
        if (!isSoundsLoaded || refresh)
        {

            foreach (Sound s in Sounds)
            {
                if (refresh)
                {
                    Destroy(s.source);
                }
                LoadSound(s);
            }
            isSoundsLoaded = true;

        }
       
    }


    public void LoadSound(Sound s)
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

    public void PlaySound(string name, bool refresh = false)
    {

        Sound s = GetSoundWithName(name);

        if(s != null)
        {
            if(!s.source.isPlaying)
            {
                s.Play();
            }
            else
            {
                if (refresh)
                {
                    s.Play();
                }
            }
            
        }

    }



    public void StopSound(string name)
    {

        Sound s = GetSoundWithName(name);
        if (s != null)
        {
            s.Stop();
        }

    }

    public Sound GetSoundWithName(string name)
    {
        return Array.Find(Sounds, sound => sound.name == name);
    }

    public void ChangePitch(Sound s , float pitch)
    {

        s.source.pitch = pitch;

    }




}
