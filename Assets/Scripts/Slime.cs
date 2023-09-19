using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slime : MonoBehaviour
{
    public float speed = 8.0f; // Adjust the speed in the Inspector.
    public float jumpHold = 0.2f;
    public float initialJump = 15.0f;

    public float gestureCheckDistance = 1.0f;
    public float gestureAllowedDistance = 25.0f;
    public float gestureExclamationDistance = 10.0f;
    public float gestureResetSeconds = 1.0f;

    private float resetGestureAfter = 0.0f;
    private bool isHiding = false;


    private Vector3 startPosition;
    public GameObject slime;

    public Shape lastGesture;
    public Shape savedGesture;
    public GameObject pointSystem;
    private PointSystem pointSystemScript;

    // Sprites
    public Sprite normal;
    public Sprite tall;
    public Sprite flat;
    public Sprite jump;

    public GameObject exclamationMarkPrefab;
    private GameObject exclamationMark;

    private GameObject[] obstacles;
    public ObstacleSpawner obstacleSpawner;

    //private Rigidbody rigidbody;

    void Start()
    {
        obstacleSpawner = GameObject.Find("ObstacleSpawner").GetComponent<ObstacleSpawner>();
        pointSystemScript = pointSystem.GetComponent<PointSystem>();
    }

    private void Awake()
    {
        startPosition = slime.transform.localPosition;
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    public void GameManagerOnOnGameStateChanged(GameState state)
    {
        if (state == GameState.MainMenu)
        {
            //slime.transform.position = new Vector3(-115.99f, -7.5f, 0);
            slime.transform.position = startPosition;
        }
    }

    void Update()
    {
        if (isHiding)
        {
            speed = 1.0f;

        }

        if (obstacleSpawner.tutorial == false)
        {
            //check if pointsystem is active
            if (pointSystem.activeSelf)
            {
                //if it is, show points
                pointSystemScript.showPoints();
            }
            else
            {
                //if it isn't, set it to active
                pointSystem.SetActive(true);
            }
        }

        if(GameManager.Instance.gameState == GameState.Playing && GameManager.Instance != null)
        {
            //automatic slime movement

            transform.Translate(Vector3.right * speed * Time.deltaTime);

            obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

            if (obstacles != null)
            {
                foreach (GameObject obstacle in obstacles)
                {
                    HandleExclamationMark(obstacle);

                    if (ObstacleReached(obstacle))
                    {
                        HandleObstacleReached(obstacle);
                    }

                    if (ObstaclePassed(obstacle))
                    {
                        HandleObstaclePassed(obstacle);
                    }
                }
            }

        }


        if (resetGestureAfter > 0.0f)
        {
            resetGestureAfter -= Time.deltaTime;
        } else
        {
            lastGesture = Shape.Null;
        }

        UpdateSlimeSprite();
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
                isHiding = true;
                print("Correct Gesture");
                pointSystemScript.pointCounter(obstacleSpawner.tutorial);
                obstacle.GetComponent<Obstacle>().completed = true;

            }
            else
            {
                print("Wrong Gesture");
                // SceneManager.LoadScene("GameOver");
                GameManager.Instance.UpdateGameState(GameState.GameOver);
            }
        }
    }

    void HandleObstaclePassed(GameObject obstacle)
    {
        if (ObstaclePassed(obstacle))
        {
            isHiding = false;
            speed = 8.0f;
        }
    }

    // Update the slime sprite based on the last gesture
    void UpdateSlimeSprite()
    {
        if (lastGesture == Shape.Null)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normal;
        }
        else if (lastGesture == Shape.Up)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = tall;
        }
        else if (lastGesture == Shape.Down)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = flat;
        } else if (lastGesture == Shape.Suspended) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = jump;
            this.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpHold, ForceMode2D.Impulse);
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

    bool ObstaclePassed(GameObject obstacle)
    {
        if (ObstacleDistance(obstacle) < 0)
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

 
    public void UpdateGesture(Shape newGesture)
    {
        lastGesture = newGesture;
        resetGestureAfter = gestureResetSeconds;
        if (newGesture == Shape.Suspended) {
            this.GetComponent<Rigidbody2D>().AddForce(transform.up * initialJump, ForceMode2D.Impulse);
        }
    }
}