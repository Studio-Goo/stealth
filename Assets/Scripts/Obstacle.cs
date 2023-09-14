using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Shape Null is the default value and used for resetting
public enum Shape { Null, Up, Down, Horizontal, Vertical, Suspended };

public class Obstacle : MonoBehaviour
{
    public Shape shape;
    public bool completed = false;

    public Sprite tallObstacle;
    public Sprite flatObstacle;
    public Sprite suspendedObstacle;

    // Start is called before the first frame update
    void Start()
    {
        if (shape == Shape.Up)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = tallObstacle;
        } else if (shape == Shape.Down)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = flatObstacle;
        } else if (shape == Shape.Suspended) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = suspendedObstacle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
