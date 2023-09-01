using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour 
{
    public float speed = 5.0f; // Adjust the speed in the Inspector.

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
