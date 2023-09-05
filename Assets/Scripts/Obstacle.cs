using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shape { Vertical, Horizontal };

public class Obstacle : MonoBehaviour
{
    public Shape shape;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(shape);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
