using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Shape { Null, Up, Down, Suspended };

public class Obstacle : MonoBehaviour
{
    public Shape shape;
    public Sprite ObstacleSprite;
    public Sprite ShadowSprite;

    public bool completed = false;

    private GameObject obstacle;
    private GameObject obstacleShadow;
    private GameObject obstacleShadowMask;

    //private GameObject Employee;


    // Start is called before the first frame update
    void Start()
    {
        obstacle = this.gameObject.transform.GetChild(1).gameObject;
        obstacleShadow = this.gameObject.transform.GetChild(2).gameObject;
        obstacleShadowMask = obstacleShadow.transform.GetChild(0).gameObject;

        ChangeObstacle(ObstacleSprite, ShadowSprite);
    }

    public void ChangeObstacle(Sprite obstacleSprite, Sprite obstacleShadowSprite)
    {
        obstacle.GetComponent<SpriteRenderer>().sprite = obstacleSprite;
        obstacleShadow.GetComponent<SpriteMask>().sprite = obstacleShadowSprite;
        obstacleShadowMask.GetComponent<SpriteRenderer>().sprite = obstacleShadowSprite;
    }
}
