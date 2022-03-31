using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerDragging : InputReader
{

    public Vector3 fingerDragging;
    public Vector3 previousDragging;
    // Start is called before the first frame update
    void Start()
    {
        MobileInputReader.SetInputMode(InputType.TouchControl);
    }

    // Update is called once per frame
    void Update()
    {

        currentInput = MobileInputReader.GetTouchInput();
        if (currentInput.dragging)
        {
            Vector3 currentPos = Camera.main.ScreenToViewportPoint(currentInput.draggingLastPos);
            Vector3 startPos = Camera.main.ScreenToViewportPoint(currentInput.draggingStartPos);

            //Vector3 currentPos =currentInput.draggingLastPos;
            //Vector3 startPos = currentInput.draggingStartPos;

            Vector3 currentDragging = (currentPos - startPos);
            fingerDragging = currentDragging - previousDragging;
            previousDragging = currentDragging;

        }
        else
        {
            
            fingerDragging = Vector3.zero;
            previousDragging = Vector3.zero;
        }

        /*
        if (Input.GetMouseButtonDown(0))
        {

            fingerDragging = Input.mousePosition.x / 100 - previousMousePos;

            previousMousePos = Input.mousePosition.x / 100;

        }
        */

        horizontalInput = fingerDragging.x;
        verticalInput = fingerDragging.y;

    }
}
