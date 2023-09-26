using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Gesture : PropertyDrawer
{
    public Shape shape;
    public GameObject collider;
    public Sprite slimeSprite;
    public int acceptableGesturePercent;
}
