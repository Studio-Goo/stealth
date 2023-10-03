using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shape { Null, Up, Down, Suspended };

public class Obstacle : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField]
    public Sprite ObstacleSprite;
    public Sprite ShadowSprite;
    public Sprite FloorShadowSprite;

    [Header("Player")]
    [SerializeField]
    public Shape shape;
    private Slime slime = Slime.Instance;
    public bool tutorial = false;

    private bool completed = false;
    private bool passed = false;

    //private GameObject obstacle;
    //private GameObject obstacleShadow;
    //private GameObject obstacleShadowMask;
    //private GameObject ParentFloor;
    //private GameObject FloorShadow;
    //private GameObject FloorShadowMask;

    private GameObject startAnimation;

    private float gestureCheckDistance = .9f;
    //[NonSerialized]
    //public PointSystem pointSystemScript;
    private PointSystem pointSystem = PointSystem.Instance;

    void Start()
    {
        //obstacle = this.gameObject.transform.GetChild(0).gameObject;
        //obstacleShadow = obstacle.transform.GetChild(0).gameObject;
        //obstacleShadowMask = obstacleShadow.transform.GetChild(0).gameObject;
        //ParentFloor = this.gameObject.transform.GetChild(1).gameObject;
        //FloorShadow = ParentFloor.transform.GetChild(0).gameObject;
        //FloorShadowMask = FloorShadow.transform.GetChild(0).gameObject;

<<<<<<< HEAD
        // ChangeObstacle(ObstacleSprite, ShadowSprite, FloorShadowSprite);
=======
        //ChangeObstacle(ObstacleSprite, ShadowSprite, FloorShadowSprite);
>>>>>>> f827f220242eaebadea053dda11aee922437e320

        startAnimation = GameObject.Find("StartAnimation");
        if (startAnimation)
        {
            startAnimation.SetActive(false);
        }

        //pointSystemScript = pointSystem.GetComponent<PointSystem>();
    }

    private void Update()
    {
        if(GameManager.Instance.gameState == GameState.Playing && GameManager.Instance != null)
        {

            //Debug.Log(slime.isHiding);

            if (ObstacleReached())
            {
                HandleObstacleReached();
            }

            if (ObstaclePassed())
            {
                HandleObstaclePassed();
            }

            if (!tutorial)
            {
                pointSystem.showPoints();
            }


            //if (SlimeDistance() < 0f)
            //{
            //    slime.isHiding = false;
            //}

            //if (PlayerPrefs.GetInt("Tutorial") == 0)
            //{
                //check if pointsystem is active
                //if (pointSystem.activeSelf)
                //{
                    //if it is, show points
                    //pointSystem.showPoints();
                //}
                //else
                //{
                    //if it isn't, set it to active
                    //pointSystem.SetActive(true);
                //}
            //}
        }
        
    }

    void correctGesture()
    {
        if (!passed)
        {
            slime.isHiding = true;
            print("Correct Gesture");
            completed = true;

            //if(!tutorial)
            //{
            //    // Only add point if not tutorial
            //    pointSystem.pointCounter();
            //    pointSystem.showPoints();
            //}
        }

        //increase speed
        if (slime.setSpeed < 20.0f)
        {
            slime.setSpeed = slime.setSpeed * 1.03f;
            slime.slowSpeed = slime.slowSpeed * 1.03f;

        }

        //decrease obstacle spawn interval
        //if (slime.obstacleSpawner.spawnInterval > 0.5f)
        //{
        //    obstacleSpawner.DecreaseSpawnInterval();
        //}
    }

    bool CheckGesture()
    {
        //float distance = SlimeDistance();


        //Debug.Log(distance);
        // if (distance > 0 && distance < gestureCheckDistance)
        // {
            //Debug.Log(obstacle);
            if (shape == slime.lastGesture)
            {
                completed = true;
                return true;
            }
        // }

        return false;
    }

    bool ObstacleReached()
    {
        float distance = SlimeDistance();

        if (!passed && distance < gestureCheckDistance)
        {
            return true;
        }

        return false;
    }

    bool ObstaclePassed()
    {
        float distance = SlimeDistance();


        if (distance < (gestureCheckDistance * -1))
        {
            return true;
        }

        return false;
    }

    void HandleObstaclePassed()
    {
        if (!passed)
        {
            if (ObstaclePassed())
            {
                slime.isHiding = false;
                passed = true;

                if (!tutorial)
                {
                    // todo: add point
                    pointSystem.enable();
                    pointSystem.pointCounter();
                }
            }
        }
    }

    void HandleObstacleReached()
    {
        if (!completed)
        {
            if (CheckGesture())
            {
                correctGesture();
            }
            else
            {
                if (slime.cheatOn)
                {
                    correctGesture();
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

    private float SlimeDistance()
    {
        float distance = this.gameObject.transform.position.x - slime.transform.position.x;
        //Debug.Log(distance);
        return distance;
    }

    //public void ChangeObstacle(Sprite obstacleSprite, Sprite obstacleShadowSprite, Sprite floorShadowSprite)
    //{
    //    obstacle.GetComponent<SpriteRenderer>().sprite = obstacleSprite;
    //    obstacleShadow.GetComponent<SpriteMask>().sprite = obstacleShadowSprite;
    //    obstacleShadowMask.GetComponent<SpriteRenderer>().sprite = obstacleShadowSprite;
    //    FloorShadow.GetComponent<SpriteMask>().sprite = floorShadowSprite;
    //    FloorShadowMask.GetComponent<SpriteRenderer>().sprite = floorShadowSprite;
    //}
}
