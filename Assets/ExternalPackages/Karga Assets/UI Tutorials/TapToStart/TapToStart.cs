using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToStart : MonoBehaviour
{

    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<PlayerController>();
        player.autoMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            player.autoMove = true;
            Destroy(gameObject);
        }
    }
}
