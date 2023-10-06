using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gesture { Null, Up, Down, Tap };

public class Obstacle : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField]
    public Sprite ObstacleSprite;
    public Sprite ShadowSprite;
    public Sprite FloorShadowSprite;

    [Header("Player")]
    [SerializeField]
    public Gesture gesture;
    private Slime slime = Slime.Instance;
    public bool tutorial = false;

    private bool completed = false;
    private bool passed = false;

    private GameObject startAnimation;

    private float gestureCheckDistance = .9f;
    private PointSystem pointSystem = PointSystem.Instance;

    void Start()
    {
        //startAnimation = GameObject.Find("StartAnimation");
        //if (startAnimation)
        //{
        //    startAnimation.SetActive(false);
        //}
    }

    private void Update()
    {
        if(GameManager.Instance != null && GameManager.Instance.gameState == GameState.Playing)
        {
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
        }
        
    }

    void correctGesture()
    {
        if (!passed)
        {
            slime.isHiding = true;
            //print("Correct Gesture");
            completed = true;

            if (PlayerPrefs.HasKey("Tutorial") && PlayerPrefs.GetInt("Tutorial") == 1)
            {
                PlayerPrefs.SetInt("obstaclePassed", PlayerPrefs.GetInt("obstaclePassed") + 1);
            }

            if(PlayerPrefs.GetInt("obstaclePassed") == 3)
            {
                PlayerPrefs.SetInt("Tutorial", 0);
                PlayerPrefs.SetInt("obstaclePassed", 0);
            }
        }

        slime.speedUp();
    }

    bool CheckGesture()
    {
        if (gesture == slime.lastGesture)
        {
            completed = true;
            return true;
        }

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

                if (PlayerPrefs.GetInt("Tutorial") == 0)
                {
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
                if (CheatManager.Instance.GetCheatMode())
                {
                    correctGesture();
                }
                else
                {
                    print("Wrong Gesture");
                    GameManager.Instance.UpdateGameState(GameState.GameOverAnimation);
                }
            }
        }
    }

    private float SlimeDistance()
    {
        float distance = this.gameObject.transform.position.x - slime.transform.position.x;
        return distance;
    }
}
