using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KargaGames.Drawing;
using Dreamteck.Splines;

public class SplineGenerator : SplineByDrawing
{

    public CrowdController CrowdSpline;
    public float RoadWidth;
    public float RoadHeight;

    public float bugOffset;

    public bool RepositionToMiddle;
    public bool RepositionToBottom;

    PlayerController playerController;

    public bool inverseX;
    public bool inverseY;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void SetCrowdController(CrowdController controller)
    {
        CrowdSpline = controller;
    }

    public override void GenerateSplineFromLine(KargaGames.Drawing.Line line, bool drawingFinished = false)
    {
        Vector3[] linePoints = new Vector3[line.GetLineRenderer().positionCount];
        line.GetLineRenderer().GetPositions(linePoints);

        if (linePoints.Length > 2)
        {

            GenerateSplineFromPointList(linePoints, line, drawingFinished);


        }


    }

    public void GenerateSplineFromPointList(Vector3[] pointList, KargaGames.Drawing.Line line, bool drawingFinished = false)
    {
        Vector3[] linePoints = pointList;

        
        linePoints = RePositionSplinePoints(linePoints, DrawingArea, CrowdSpline.GetSpline(), line);
        

        SplinePoint[] splinePoints = new SplinePoint[linePoints.Length];

        int i = 0;
        foreach (Vector3 point in linePoints)
        {
            splinePoints[i].position = point;
            splinePoints[i].size = 1f;
            i++;
        }


        CrowdSpline.UpdateSpline(splinePoints, drawingFinished);

        if (!GameSceneManager.gameOver && drawingFinished)
        {
            playerController.autoMove = true;
        }
    }

    public Vector3[] RePositionSplinePoints(Vector3[] points, RectTransform DrawingArea, SplineComputer Road, KargaGames.Drawing.Line line)
    {
        Vector3[] corners = new Vector3[4];

        DrawingArea.GetLocalCorners(corners);

        Vector3 BottomLeft = Camera.main.WorldToScreenPoint(DrawingArea.TransformPoint(corners[0]));
        Vector3 TopRigth = Camera.main.WorldToScreenPoint(DrawingArea.TransformPoint(corners[2]));

        float minAreaX = BottomLeft.x;
        float maxAreaX = TopRigth.x;

        float minAreaY = BottomLeft.y;
        float maxAreaY = TopRigth.y;

        float DrawingAreaRange = maxAreaX - minAreaX;

        float DrawingAreaYRange = (maxAreaY - minAreaY);
        float DrawingAreaMidY = minAreaY + (DrawingAreaYRange / 2);

        List<Vector3> generatedPoints = new List<Vector3>();

        float totalYCoord = 0;

        float pointCount = 0;

        float offsetY = 0;


        if (RepositionToMiddle)
        {
            foreach (Vector3 point in points)
            {
                Vector3 screenPointCoord = Camera.main.WorldToScreenPoint(line.transform.TransformPoint(point));

                totalYCoord += screenPointCoord.y;
                pointCount++;

            }

            float avarageYCoord = totalYCoord / pointCount;

            offsetY = DrawingAreaMidY - avarageYCoord;
        }




        if (RepositionToBottom)
        {
            float minY = 9999f;
            foreach (Vector3 point in points)
            {
                Vector3 screenPointCoord = Camera.main.WorldToScreenPoint(line.transform.TransformPoint(point));

                if(screenPointCoord.y < minY)
                {
                    minY = screenPointCoord.y;
                }
                
            }

            offsetY = minAreaY - minY;
        }

        
        foreach (Vector3 point in points)
        {

            Vector3 screenPointCoord = Camera.main.WorldToScreenPoint(line.transform.TransformPoint(point));

            if (inverseX)
            {
                screenPointCoord.x *= -1;
            }
            if (inverseY)
            {
                screenPointCoord.y *= -1;
            }
            float PositionOnRange = screenPointCoord.x - minAreaX;
            float HeightOnRange = screenPointCoord.y - offsetY - minAreaY;

            float PercentageOnRange = PositionOnRange / DrawingAreaRange;
            float PercentageOnHeight = HeightOnRange / DrawingAreaYRange;

            float DesiredPositionOnRoad = (RoadWidth * PercentageOnRange)-(RoadWidth/2f);
            float DesiredHeightOnRoad = (RoadHeight * PercentageOnHeight) - (RoadHeight / 2f);

            generatedPoints.Add(new Vector3(DesiredPositionOnRoad,0,(DesiredHeightOnRoad + bugOffset)));
           
        }


        return generatedPoints.ToArray();
    }
}
