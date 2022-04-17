using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float movementSpeed;
    public float minX;
    public float maxX;
    public float currentX;
    protected bool direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (currentX <= minX)
        {
            direction = true;

        }
        else if (currentX >= maxX)
        {
            direction = false;
        }



        if (direction)
        {
            currentX += movementSpeed * Time.deltaTime;
        }
        else
        {
            currentX -= movementSpeed * Time.deltaTime;
        }

        transform.localPosition = new Vector3(currentX, transform.localPosition.y,transform.localPosition.z);
    }
}
