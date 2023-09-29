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

    //[NonSerialized]
    public bool completed = false;

    private GameObject obstacle;
    private GameObject obstacleShadow;
    private GameObject obstacleShadowMask;

    private float gestureCheckDistance = 0.5f;

    // Start is called before the first frame update
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
            Debug.Log(SlimeDistance());
            if (SlimeDistance() < gestureCheckDistance && !completed)
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
                        GameManager.Instance.UpdateGameState(GameState.GameOverAnimation);
                    }
                }
            }

            if(SlimeDistance() < -0.7f)
            {
                slime.isHiding = false;
            }
        }
        
    }

    void correctGesture()
    {
        slime.isHiding = true;
        print("Correct Gesture");
        slime.pointSystemScript.pointCounter();
        completed = true;

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
        float distance = SlimeDistance();

        Debug.Log(distance);

        if (distance > 0 && distance < gestureCheckDistance)
        {
            if (shape == slime.lastGesture)
            {
                completed = true;
                return true;
            }
        }

        return false;
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
