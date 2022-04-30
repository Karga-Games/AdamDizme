using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float movementSpeed;
    public Vector3 desiredPosition;



    public CrowdColumn column;
    public int columnCoordinate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LerpToDesiredPosition();
    }


    public void SetColumn(CrowdColumn column, int coordinate)
    {
        this.column = column;
        columnCoordinate = coordinate;

        desiredPosition = column.transform.localPosition;
        desiredPosition.y = columnCoordinate * column.crowd.VerticalDistanceBetweenBalls;

    }

    public void LerpToDesiredPosition()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPosition, Time.deltaTime * movementSpeed);
    }
}
