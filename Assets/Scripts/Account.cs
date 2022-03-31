using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Account
{
    public static int Diamonds { get { return PlayerPrefs.GetInt("Diamonds", 0); } set { PlayerPrefs.SetInt("Diamonds", value); } }

    public static int Level { get { return PlayerPrefs.GetInt("Level", 1); } set { PlayerPrefs.SetInt("Level", value); } }


}
