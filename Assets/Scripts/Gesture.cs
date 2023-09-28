using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Gesture
{
    public Shape shape;
    public GameObject collider;
    public Sprite slimeSprite;
    public int acceptableGesturePercent;
    public int thresholdMultiplier = 1;

    public Gesture(Shape newShape)
    {
        shape = newShape;
    }
}
