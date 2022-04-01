using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KargaGames.Drawing
{

    public class ScreenDrawer : MonoBehaviour
    {
        public Line linePrefab;
        public RectTransform DrawingArea;

        TouchInput currentInput;
        Line currentLine;

        Vector2 lastfingerPosition;

        bool limitArea =false;
        public void Start()
        {
            MobileInputReader.SetInputMode(InputType.TouchControl);

            if(DrawingArea!= null)
            {
                limitArea = true;

            }


        }

        public void Update()
        {
            currentInput = MobileInputReader.GetTouchInput();
            if (currentInput.dragging)
            {

                Vector2 fingerPosition = currentInput.draggingLastPos;

                if (DrawingArea != null)
                {

                    if (!RectTransformUtility.RectangleContainsScreenPoint(DrawingArea, fingerPosition, Camera.main))
                    {

                        if(currentLine != null)
                        {
                            bool generated = false;

                            Vector2 generatedPosByX = new Vector2(lastfingerPosition.x, fingerPosition.y);

                            Vector2 generatedPosByY = new Vector2(fingerPosition.x, lastfingerPosition.y);


                            if (RectTransformUtility.RectangleContainsScreenPoint(DrawingArea, generatedPosByX, Camera.main))
                            {

                                fingerPosition = generatedPosByX;
                                generated = true;
                            }

                            if (RectTransformUtility.RectangleContainsScreenPoint(DrawingArea, generatedPosByY, Camera.main))
                            {

                                fingerPosition = generatedPosByY;
                                generated = true;
                            }

                            if (!generated)
                            {
                                return;
                            }

                        }
                        else
                        {
                            return;
                        }

                    }

                }

                lastfingerPosition = fingerPosition;

                Vector3 currentPos = Camera.main.ScreenToWorldPoint(new Vector3(fingerPosition.x,fingerPosition.y,1f));

                if (currentLine == null)
                {
                    currentLine = Instantiate(linePrefab);
                }

                currentLine.DrawWithPoint(currentPos);

            }
            else
            {
                if (currentLine != null)
                {
                    Destroy(currentLine.gameObject);
                }
            }

        }





    }
}

