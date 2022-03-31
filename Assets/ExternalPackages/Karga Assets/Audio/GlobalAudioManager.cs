using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioManager : AudioManager
{

    private static GlobalAudioManager instance;
    // Start is called before the first frame update

    public override void Awake()
    {
        base.Awake();

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

}
