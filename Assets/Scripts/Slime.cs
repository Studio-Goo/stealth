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
    public float slowSpeed = 1.0f;
    public float setSpeed = 8.0f;

    private bool cheatOn = false;


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

    //audio
    public GameObject jumpAudio;
    public GameObject walkAudio;
    public GameObject stretchAudio;


    private List<GameObject> obstacles;
    public ObstacleSpawner obstacleSpawner;

    //private Rigidbody rigidbody;

    void Start()
    {
        //obstacleSpawner = GameObject.Find("ObstacleSpawner").GetComponent<ObstacleSpawner>();
        pointSystemScript = pointSystem.GetComponent<PointSystem>();

        //walkAudio.SetActive(true);
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
            speed = slowSpeed;
            slime.GetComponent<Rigidbody2D>().gravityScale = 0.05f;

        }
        else
        {
            speed = setSpeed;
            slime.GetComponent<Rigidbody2D>().gravityScale = 2f;
        }

        if (PlayerPrefs.GetInt("Tutorial") == 0)
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

            //obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            obstacles = obstacleSpawner.getObstacles();

            if (obstacles != null)
            {
                foreach (GameObject obstacle in obstacles)
                {
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

    // Handle actions when an obstacle is reached
    void HandleObstacleReached(GameObject obstacle)
    {
        if (!obstacle.GetComponent<Obstacle>().completed)
        {
            if (CheckGesture(obstacle))
            {
                correctGesture(obstacle);
            }
            else
            {
                if (cheatOn)
                {
                    correctGesture(obstacle);
                }
                else
                {
                    print("Wrong Gesture");
                    // SceneManager.LoadScene("GameOver");
                    GameManager.Instance.UpdateGameState(GameState.GameOverAnimation);
                }
            }
        }
    }

    void correctGesture(GameObject obstacle)
    {
        isHiding = true;
        print("Correct Gesture");
        pointSystemScript.pointCounter();
        obstacle.GetComponent<Obstacle>().completed = true;

        //increase speed
        if (setSpeed < 20.0f)
        {
            setSpeed = setSpeed * 1.03f;
            slowSpeed = slowSpeed * 1.03f;

        }

        //decrease obstacle spawn interval
        if (obstacleSpawner.spawnInterval > 0.5f)
        {
            obstacleSpawner.DecreaseSpawnInterval();
        }
    }

    void HandleObstaclePassed(GameObject obstacle)
    {
        if (ObstaclePassed(obstacle))
        {
            isHiding = false;
        }
    }

    // Update the slime sprite based on the last gesture
    void UpdateSlimeSprite()
    {
        if (lastGesture == Shape.Null)
        {
            ChangeSlimeSprite(normal);
        }
        else if (lastGesture == Shape.Up)
        {
            ChangeSlimeSprite(tall);
        }
        else if (lastGesture == Shape.Down)
        {
            ChangeSlimeSprite(flat);
        } else if (lastGesture == Shape.Suspended) {

            ChangeSlimeSprite(jump);
            if (!isHiding)
            {
                this.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpHold, ForceMode2D.Impulse);
            }
        }
    }

    private void ChangeSlimeSprite(Sprite sprite)
    {
        slime.GetComponent<SpriteRenderer>().sprite = sprite;
        slime.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        slime.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    
    // Check if the gesture is correct
    bool CheckGesture(GameObject obstacle)
    {
        float distance = ObstacleDistance(obstacle.transform.GetChild(1).gameObject);


        Debug.Log(distance);
        if (distance > 0 && distance < gestureCheckDistance)
        {
            //Debug.Log(obstacle);
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
        if (ObstacleDistance(obstacle.transform.GetChild(1).gameObject) < gestureCheckDistance)
        {
            return true;
        }

        return false;
    }

    bool ObstaclePassed(GameObject obstacle)
    {
        if (ObstacleDistance(obstacle.transform.GetChild(1).gameObject) < 0)
        {
            return true;
        }

        return false;
    }


    public GameObject GetNearestObstacle()
    {
        obstacles = obstacleSpawner.getObstacles();

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
        if (newGesture == Shape.Suspended && transform.position.y <= -7.31f) { //could maybe put it between 2 heights for the double jump spawning
            this.GetComponent<Rigidbody2D>().AddForce(transform.up * initialJump, ForceMode2D.Impulse);
        }
    }

    public void ResetAll()
    {
        slowSpeed = 1.0f;
        setSpeed = 8.0f;
    }
}