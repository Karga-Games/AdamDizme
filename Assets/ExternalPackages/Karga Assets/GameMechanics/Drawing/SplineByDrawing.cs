using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KargaGames.Drawing
{
    

    public class SplineByDrawing : ScreenDrawer
    {
        public SplineComputer splineComputer;
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
            if (currentLine != null)
            {
                GenerateSplineFromLine(currentLine, false);
            }

        }

        public override void DrawingFinished()
        {

            if(currentLine != null)
            {
                GenerateSplineFromLine(currentLine,true);
            }

            DestroyLine();

        }

        public virtual void GenerateSplineFromLine(Line line, bool drawingFinished = false)
        {
            Vector3[] linePoints = new Vector3[line.GetLineRenderer().positionCount];
            line.GetLineRenderer().GetPositions(linePoints);

            SplinePoint[] splinePoints = new SplinePoint[linePoints.Length];

            int i = 0;
            foreach(Vector3 point in linePoints)
            {
                splinePoints[i].position = point;
                i++;
            }

            splineComputer.SetPoints(splinePoints);

        }


        

    }

}
