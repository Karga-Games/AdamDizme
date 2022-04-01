using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KargaGames.Drawing
{

    [RequireComponent(typeof(LineRenderer))]
    public class Line : MonoBehaviour
    {
        protected List<Vector3> linePoints = new List<Vector3>();
        LineRenderer lineRenderer;
        public float startWidth = 1.0f;
        public float endWidth = 1.0f;
        public float threshold = 0.001f;
        public int endcapvertices = 0;
        public int cornervertices = 0;
        public Material lineMaterial;
        public int maxVertexCount = 1000;
        int lineCount = 0;

        Vector3 lastPos = Vector3.one * float.MaxValue;

        public virtual void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        public virtual void FixedUpdate()
        {


        }

        public virtual void RemoveFirstPoint()
        {
            if (lineCount > maxVertexCount)
            {
                linePoints.RemoveAt(0);
            }
        }


        public virtual void DrawWithPoint(Vector3 point)
        {
            float dist = Vector3.Distance(lastPos, point);
            if (dist <= threshold)
                return;

            lastPos = point;
            if (linePoints == null)
                linePoints = new List<Vector3>();
            linePoints.Add(point);
            UpdateLine();
        }

        public virtual void DrawWithPosition()
        {
            float dist = Vector3.Distance(lastPos, transform.position);
            if (dist <= threshold)
                return;

            lastPos = transform.position;
            if (linePoints == null)
                linePoints = new List<Vector3>();
            linePoints.Add(transform.position);
            UpdateLine();
        }

        public void Clear()
        {

        }

        public virtual void UpdateLine()
        {
            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;
            lineRenderer.numCapVertices = endcapvertices;
            lineRenderer.numCornerVertices = cornervertices;
            lineRenderer.material = lineMaterial;

            RemoveFirstPoint();

            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());

            /*

            for (int i = lineCount; i < linePoints.Count; i++)
            {
                lineRenderer.SetPosition(i, linePoints[i]);
            }

            */

            lineCount = linePoints.Count;


        }

        public LineRenderer GetLineRenderer()
        {
            return lineRenderer;
        }
    }
}

