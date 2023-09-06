using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            // checking the gesture
            if(CheckGesture())
            {
                lastGesture = Shape.Null;

                // resuming player speed
                Debug.Log("resume speed");
            } else
            {
                // Show gameover screen
                SceneManager.LoadScene("GameOver");
            }

            
        }
    }

    bool CheckGesture()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            float distance = Vector3.Distance(transform.position, obstacle.transform.position);

            // If the slime is within the stopDistance of any obstacle, return false (stop moving).
            if (distance < stopDistance)
            {
                if (obstacle.GetComponent<Obstacle>().shape == lastGesture)
                {
                    obstacle.GetComponent<Obstacle>().completed = true;
                    return true;
                }
            }
        }

        return false;
    }

    bool CanMove()
    {
        //check if there are any obstacles in the scene
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        

        foreach (GameObject obstacle in obstacles)
        {

            if (obstacle.GetComponent<Obstacle>().completed)
            {
                return true;
            }

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
