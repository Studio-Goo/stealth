using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slime : MonoBehaviour
{
    public float speed = 5.0f; // Adjust the speed in the Inspector.

    public float gestureCheckDistance = 1.0f;
    public float gestureAllowedDistance = 25.0f;
    public float gestureExclamationDistance = 10.0f;
    public float gestureResetSeconds = 1.0f;


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


        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        if (obstacles != null)
        {
            foreach (GameObject obstacle in obstacles)
            {
                HandleGestureReset(obstacle);
                HandleExclamationMark(obstacle);

                if (ObstacleReached(obstacle))
                {
                    HandleObstacleReached(obstacle);
                }
            }
        }

        UpdateSlimeSprite();
    }

    // Handle resetting the gesture if needed
    void HandleGestureReset(GameObject obstacle)
    {
        if (lastGesture != Shape.Null && !ObstacleReached(obstacle))
        {
            StartCoroutine(WaitAndResetGesture(gestureResetSeconds));
        }
    }

    // Handle spawning and destroying the exclamation mark
    void HandleExclamationMark(GameObject obstacle)
    {
        if (ObstacleDistance(obstacle) == gestureExclamationDistance && exclamationMark == null)
        {
            exclamationMark = Instantiate(exclamationMarkPrefab, transform.position, Quaternion.identity);
        }
        else if (!GestureDistance(obstacle) && exclamationMark != null)
        {
            Destroy(exclamationMark);
            exclamationMark = null;
        }
    }

    // Handle actions when an obstacle is reached
    void HandleObstacleReached(GameObject obstacle)
    {
        if (!obstacle.GetComponent<Obstacle>().completed)
        {
            if (CheckGesture(obstacle))
            {
                print("Correct Gesture");
                obstacle.GetComponent<Obstacle>().completed = true;
            }
            else
            {
                print("Wrong Gesture");
                SceneManager.LoadScene("GameOver");
            }
        }
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
    bool CheckGesture(GameObject obstacle)
    {
        float distance = ObstacleDistance(obstacle);

        if (distance > 0 && distance < gestureCheckDistance)
        {
            if (obstacle.GetComponent<Obstacle>().shape == lastGesture)
            {
                obstacle.GetComponent<Obstacle>().completed = true;
                return true;
            }
        }

        return false;
    }

    // Check if the obstacle is reached
    bool ObstacleReached(GameObject obstacle)
    {
        if (ObstacleDistance(obstacle) < gestureCheckDistance)
        {
            return true;
        }

        return false;
    }

    public GameObject GetNearestObstacle()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            return obstacle;
        }

        return null;
    }

    public float ObstacleDistance(GameObject obstacle)
    {
        return obstacle.transform.position.x - transform.position.x;
    }

    // Check if the gesture distance is reached
    bool GestureDistance(GameObject obstacle)
    {
        float distance = ObstacleDistance(obstacle);

        if (distance < gestureAllowedDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Wait for a few seconds and reset the gesture
    IEnumerator WaitAndResetGesture(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lastGesture = Shape.Null;
    }
}