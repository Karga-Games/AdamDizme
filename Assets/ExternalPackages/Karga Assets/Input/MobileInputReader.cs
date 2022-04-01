using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    Start,
    Left,
    Right,
    Front,
    Back
}

public enum InputType
{
    Gyroscope,
    Accelerometer,
    TouchControl,
    ButtonControl

};

public class TouchInput
{

    public float horizontalInput;
    public float verticalInput;
    public bool dragging = false;
    public bool touching = false;
    public Vector3 draggingStartPos = Vector3.zero;
    public Vector3 draggingLastPos = Vector3.zero;
    public Vector3 draggingDirection = Vector3.zero;
    public Direction draggingDirectionName = Direction.Start;
    public List<Touch> currentTouch;
    public TouchInput()
    {
        horizontalInput = 0;
        verticalInput = 0;
    }

    public void SetInput(float horizontal, float vertical)
    {
        horizontalInput = horizontal;
        verticalInput = vertical;
    }

}

public class ButtonInput
{
    public string buttonName;
    public float scalarValue;
    public bool boolValue;

    public void setBool(bool val)
    {
        boolValue = val;
    }

    public void setValue(float val)
    {
        scalarValue = val;
    }
}

public static class MobileInputReader
{

    private static TouchInput input = new TouchInput();
    public static List<ButtonInput> buttons;
    //Current input mode
    public static InputType inputMode = InputType.TouchControl; // Touch Control Default

    //InputType-Method Pairs Dictionary
    private static Dictionary<InputType, System.Action> inputMethods = new Dictionary<InputType, System.Action>() {

        {InputType.Gyroscope , GetInputGyroscope },

        {InputType.TouchControl, GetInputTouchControl },

        {InputType.Accelerometer, GetInputAccelerometer },

        {InputType.ButtonControl, GetInputButton }

    };

    public static void SetInputMode(InputType type)
    {
        inputMode = type;
    }

    public static InputType GetInputMode()
    {
        return inputMode;
    }

    public static TouchInput GetTouchInput()
    {
        UpdateInput();
        return input;
    }

    public static TouchInput ReadLastInput()
    {
        return input;
    }

    public static void UpdateInput()
    {
        inputMethods[inputMode]();
    }

    private static void GetInputGyroscope()
    {

    }

    private static void GetInputButton()
    {
        if (buttons != null)
        {
            foreach (ButtonInput buttonInput in buttons)
            {

                switch (buttonInput.buttonName)
                {

                    case "Vertical":

                        input.verticalInput = buttonInput.scalarValue;

                        break;
                    case "Horizontal":

                        input.horizontalInput = buttonInput.scalarValue;

                        break;
                    default:

                        break;
                }

            }
        }
    }

    private static void GetInputAccelerometer()
    {

        input.horizontalInput = Input.acceleration.x;

        int validFingers = 0;
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    validFingers++;
                }
                else if (touch.position.x > Screen.width / 2)
                {
                    validFingers++;
                }
            }
        }

        if (validFingers > 0)
        {
            input.touching = true;
        }
        else
        {
            input.touching = false;
        }

    }

    private static void GetInputTouchControl()
    {

        input.verticalInput = Input.GetAxisRaw("Vertical");
        input.horizontalInput = Input.GetAxisRaw("Horizontal");

        int validFingers = 0;

        input.currentTouch = new List<Touch>(Input.touches);

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    if (!input.touching)
                    {
                        input.horizontalInput = -1;
                    }

                    validFingers++;
                }
                else if (touch.position.x > Screen.width / 2)
                {
                    if (!input.touching)
                    {
                        input.horizontalInput = 1;
                    }
                    validFingers++;
                }
            }

            Touch touchHolder = Input.GetTouch(0);

            switch (touchHolder.phase)
            {
                case TouchPhase.Began:

                    input.draggingStartPos = touchHolder.position;
                    input.draggingLastPos = touchHolder.position;

                    break;
                case TouchPhase.Moved:

                    input.draggingLastPos = touchHolder.position;
                    input.draggingDirection = input.draggingLastPos - input.draggingStartPos;
                    input.dragging = true;

                    break;
                case TouchPhase.Ended:

                    if (Mathf.Abs(input.draggingDirection.x) > Mathf.Abs(input.draggingDirection.y))
                    {
                        //left-right

                        if (input.draggingDirection.x > 0)
                        {
                            input.draggingDirectionName = Direction.Right;
                        }
                        else
                        {
                            input.draggingDirectionName = Direction.Left;
                        }

                        if (input.draggingDirection.x == 0)
                        {
                            input.draggingDirectionName = Direction.Start;
                        }
                    }
                    else
                    {
                        //up-down

                        if (input.draggingDirection.y > 0)
                        {
                            input.draggingDirectionName = Direction.Front;
                        }
                        else
                        {
                            input.draggingDirectionName = Direction.Back;
                        }


                        if (input.draggingDirection.y == 0)
                        {
                            input.draggingDirectionName = Direction.Start;
                        }

                    }

                    input.dragging = false;

                    break;
            }

        }

        if (validFingers > 0)
        {
            input.touching = true;
        }
        else
        {
            input.touching = false;
        }

    }


}
