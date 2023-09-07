using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slime : MonoBehaviour 
{
    public float speed = 5.0f; // Adjust the speed in the Inspector.
    public float stopDistance = 3.0f; // Set the distance at which the slime should stop.
    public float gestureDistance = 10.0f; // Distance in which a gesture can be made.
    public float waitSeconds = 1.0f; // Seconds to wait before resetting gesture.
    public Shape lastGesture;
    public Shape savedGesture;

    // Sprites
    public Sprite normal;
    public Sprite horizontal;
    public Sprite vertical;
    
    public GameObject exclamationMarkPrefab; 
    private GameObject exclamationMark; 

    private GameObject[] obstacles;
    public ObstacleSpawner obstacleSpawner;

    void Start()
    {
        obstacleSpawner = GameObject.Find("ObstacleSpawner").GetComponent<ObstacleSpawner>();
    }

    void Update()
    {
        //automatic slime movement
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        HandleGestureReset();
        HandleExclamationMark();

        if (ObstacleReached())
        {
            HandleObstacleReached();
        }

        UpdateSlimeSprite();
    }

    // Handle resetting the gesture if needed
    void HandleGestureReset()
    {
        if (lastGesture != Shape.Null && !ObstacleReached())
        {
            StartCoroutine(WaitAndResetGesture(waitSeconds));
        }
    }

    // Handle spawning and destroying the exclamation mark
    void HandleExclamationMark()
    {
        if (GestureDistance() && exclamationMark == null)
        {
            exclamationMark = Instantiate(exclamationMarkPrefab, transform.position, Quaternion.identity);
        }
        else if (!GestureDistance() && exclamationMark != null)
        {
            Destroy(exclamationMark);
            exclamationMark = null;
        }
    }

    // Handle actions when an obstacle is reached
    void HandleObstacleReached()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        if (CheckGesture())
        {
            print("Correct Gesture");
        }
        else
        {
            print("Wrong Gesture");
            SceneManager.LoadScene("GameOver");
        }

        StartCoroutine(WaitAndResetGesture(waitSeconds));
    }

    // Update the slime sprite based on the last gesture
    void UpdateSlimeSprite()
    {
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

    // Check if the gesture is correct
    bool CheckGesture()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            float distance = Vector3.Distance(transform.position, obstacle.transform.position);

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

    // Check if the obstacle is reached
    bool ObstacleReached()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            float distance = Vector3.Distance(transform.position, obstacle.transform.position);

            if (distance < stopDistance)
            {
                return true;
            }
        }

        return false;
    }

    // Check if the gesture distance is reached
    bool GestureDistance()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            float distance = Vector3.Distance(transform.position, obstacle.transform.position);

            if (distance < gestureDistance)
            {
                return true;
            }
        }

        return false;
    }

    // Wait for a few seconds and reset the gesture
    IEnumerator WaitAndResetGesture(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lastGesture = Shape.Null;
    }
}
