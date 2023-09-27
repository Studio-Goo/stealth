using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Gesture))]
public class GesturePropertyDrawer : PropertyDrawer
{
    //public override void OnGUI(Shape shape, GameObject collider, Sprite slimeSprite, int acceptableGesturePercent)
    //{
    //EditorGUI.BeginProperty(shape, collider, slimeSprite, acceptableGesturePercent);
    //public Shape shape;
    //public GameObject collider;
    //public Sprite slimeSprite;
    //public int acceptableGesturePercent;
    //}

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();

        // Create property fields.
        var shapeField = new PropertyField(property.FindPropertyRelative("shape"));
        //var unitField = new PropertyField(property.FindPropertyRelative("unit"));
        //var nameField = new PropertyField(property.FindPropertyRelative("name"), "Fancy Name");

        // Add fields to the container.
        container.Add(shapeField);
        //container.Add(unitField);
        //container.Add(nameField);

        return container;
    }


    //public void Gesture(Shape newShape)
    //{
    //    shape = newShape;
    //}
}
