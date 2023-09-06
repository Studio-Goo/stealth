using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Shape Null is the default value and used for resetting
public enum Shape { Null, Vertical, Horizontal };

public class Obstacle : MonoBehaviour
{
    public Shape shape;
    public bool completed = false;

    public Sprite verticalObstacle;
    public Sprite horizontalObstacle;

    // Start is called before the first frame update
    void Start()
    {
        if (shape == Shape.Vertical)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = verticalObstacle;
        } else if (shape == Shape.Horizontal)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = horizontalObstacle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
