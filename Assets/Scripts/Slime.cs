using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour 
{
    public float speed = 5.0f; // Adjust the speed in the Inspector.
    public float stopDistance = 5.0f; // Set the distance at which the slime should stop.
    public Shape lastGesture;

    private GameObject[] obstacles;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (CanMove())
        {
            // Move the slime if it's allowed to move.
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else {
            //EventScript eventScript = GetComponent<EventScript>();

            //eventScript.testCase();

            }
    }

    bool CanMove()
    {
        //check if there are any obstacles in the scene
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            float distance = Vector3.Distance(transform.position, obstacle.transform.position);

            // If the slime is within the stopDistance of any obstacle, return false (stop moving).
            if (distance < stopDistance)
            {
                return false;
            }
        }

        return true; 
    }
}
