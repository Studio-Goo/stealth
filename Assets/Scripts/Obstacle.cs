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

    [Header("Player")]
    [SerializeField]
    public Shape shape;
    private Slime slime = Slime.Instance;

    private bool completed = false;
    private bool passed = false;

    private GameObject obstacle;
    private GameObject obstacleShadow;
    private GameObject obstacleShadowMask;

    private float gestureCheckDistance = 0.5f;

    void Start()
    {
        obstacle = this.gameObject.transform.GetChild(0).gameObject;
        obstacleShadow = this.gameObject.transform.GetChild(1).gameObject;
        obstacleShadowMask = obstacleShadow.transform.GetChild(0).gameObject;

        ChangeObstacle(ObstacleSprite, ShadowSprite);
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


            //if (SlimeDistance() < 0f)
            //{
            //    slime.isHiding = false;
            //}
        }
        
    }

    void correctGesture()
    {
        if (!passed)
        {
            Debug.Log(shape + " hinding = true, " + SlimeDistance());
            slime.isHiding = true;
            print("Correct Gesture");
            slime.pointSystemScript.pointCounter();
            completed = true;
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

        if (distance > 0 && distance < gestureCheckDistance)
        {
            return true;
        }

        return false;
    }

    bool ObstaclePassed()
    {
        float distance = SlimeDistance();

        if (distance < 0)
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
                Debug.Log(shape + " hinding = false, " + SlimeDistance());
                slime.isHiding = false;
                passed = true;
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
        return this.gameObject.transform.position.x - slime.transform.position.x;
    }

    public void ChangeObstacle(Sprite obstacleSprite, Sprite obstacleShadowSprite)
    {
        obstacle.GetComponent<SpriteRenderer>().sprite = obstacleSprite;
        obstacleShadow.GetComponent<SpriteMask>().sprite = obstacleShadowSprite;
        obstacleShadowMask.GetComponent<SpriteRenderer>().sprite = obstacleShadowSprite;
    }
}
