using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slime : MonoBehaviour 
{
    public float speed = 5.0f; // Adjust the speed in the Inspector.
    public float stopDistance = 5.0f; // Set the distance at which the slime should stop.
    public Shape lastGesture;

    //Sprites
    public Sprite normal;
    public Sprite horizontal;
    public Sprite vertical;

    private GameObject[] obstacles;

    public ObstacleSpawner obstacleSpawner;

    void Start()
    {
        obstacleSpawner = GameObject.Find("ObstacleSpawner").GetComponent<ObstacleSpawner>();
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

            obstacleSpawner.enabled = false;

            // checking the gesture
            if(CheckGesture())
            {
                lastGesture = Shape.Null;

                // resuming player speed
                Debug.Log("resume speed");

                //resuming obstacle spawning
                obstacleSpawner.enabled = true;

            } /* else
            {
                // Show gameover screen
                SceneManager.LoadScene("GameOver");
            } */

            

            
        }

        // Changing slime sprite
        if (lastGesture == Shape.Null)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normal;
        }
        else if (lastGesture == Shape.Horizontal)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = horizontal;
        }
        else if (lastGesture == Shape.Vertical)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = vertical;
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
