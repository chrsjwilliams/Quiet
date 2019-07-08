﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ellipse 
{
    public float xAxis;
    public float yAxis;

    public Ellipse(float _xAxis, float _yAxis)
    {
        xAxis = _xAxis;
        yAxis = _yAxis;
    }

    public Vector2 Evaluate(float t)
    {
        float angle = Mathf.Deg2Rad * 360 * t;
        float xPos = Mathf.Sin(angle) * xAxis;
        float yPos = Mathf.Cos(angle) * yAxis;
        return new Vector2(xPos, yPos);
    }
    
}
