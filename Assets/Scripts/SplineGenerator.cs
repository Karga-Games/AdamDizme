using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KargaGames.Drawing;
using Dreamteck.Splines;

public class SplineGenerator : SplineByDrawing
{

    public CrowdController CrowdSpline;
    public float RoadWidth;
    public float YZFactor;

    PlayerController playerController;

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

    public override void GenerateSplineFromLine(KargaGames.Drawing.Line line)
    {
        Vector3[] linePoints = new Vector3[line.GetLineRenderer().positionCount];
        line.GetLineRenderer().GetPositions(linePoints);

        GenerateSplineFromPointList(linePoints);

    }

    public void GenerateSplineFromPointList(Vector3[] pointList)
    {
        Vector3[] linePoints = pointList;

        linePoints = RePositionSplinePoints(linePoints, DrawingArea, CrowdSpline.GetSpline());

        SplinePoint[] splinePoints = new SplinePoint[linePoints.Length];

        int i = 0;
        foreach (Vector3 point in linePoints)
        {
            splinePoints[i].position = point;
            splinePoints[i].size = 1f;
            i++;
        }


        CrowdSpline.UpdateSpline(splinePoints);

        if (!GameSceneManager.gameOver)
        {
            playerController.autoMove = true;
        }
    }

    public Vector3[] RePositionSplinePoints(Vector3[] points, RectTransform DrawingArea, SplineComputer Road)
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

        
        foreach(Vector3 point in points)
        {
            Vector3 screenPointCoord = Camera.main.WorldToScreenPoint(transform.TransformPoint(point));

            totalYCoord += screenPointCoord.y;
            pointCount++;

        }

        float avarageYCoord = totalYCoord / pointCount;

        float offsetY = DrawingAreaMidY - avarageYCoord;

        foreach (Vector3 point in points)
        {

            Vector3 screenPointCoord = Camera.main.WorldToScreenPoint(transform.TransformPoint(point));
            
            float PositionOnRange = screenPointCoord.x - minAreaX;

            float PercentageOnRange = PositionOnRange / DrawingAreaRange;

            float DesiredPositionOnRoad = (RoadWidth * PercentageOnRange)-(RoadWidth/2f);

            generatedPoints.Add(new Vector3(DesiredPositionOnRoad,0,(screenPointCoord.y + offsetY - DrawingAreaMidY) * YZFactor));

        }


        return generatedPoints.ToArray();
    }
}
