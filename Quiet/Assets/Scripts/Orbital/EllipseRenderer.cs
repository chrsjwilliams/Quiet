using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EllipseRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer m_lineRenderer;

    [Range(3, 36)]
    public int segments;
    public Ellipse ellipse;

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        CalculateEllipse();
    }

    void CalculateEllipse()
    {
        Vector3[] points = new Vector3[segments + 1];
        for(int i  = 0; i < segments; i++)
        {
            points[i] = ellipse.Evaluate(i / (float)segments);
        }
        points[segments] = points[0];

        m_lineRenderer.positionCount = segments + 1;
        m_lineRenderer.SetPositions(points);
    }

    private void OnValidate()
    {
        CalculateEllipse();
    }
}
