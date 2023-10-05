using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slime : MonoBehaviour
{
    public static Slime Instance;
    public float speed = 8.0f; // Adjust the speed in the Inspector.
    public float jumpHold = 0.2f;
    public float initialJump = 15.0f;


    public float gestureResetSeconds = 1.0f;

    private float resetGestureAfter = 0.0f;
    [NonSerialized]
    public bool isHiding = false;
    // Always in-sync w/ setSpeed
    public float slowFraction = 8.0f;
    public float baseSpeed = 8.0f;
    public float setSpeed = 8.0f;
    public float maxSpeed = 30.0f;
    public float speedIncrease = 0.3f;

    [NonSerialized]
    public bool cheatOn = false;
    public GameObject cheatText;


    private Vector3 startPosition;
    public GameObject slime;

    public Shape lastGesture;
    public Shape savedGesture;
    //public GameObject pointSystem;


    // Sprites
    public Sprite normal;
    public Sprite tall;
    public Sprite flat;
    public Sprite jump;



    void Start()
    {
        //pointSystemScript = pointSystem.GetComponent<PointSystem>();

        //walkAudio.SetActive(true);
    }

    private void Awake()
    {
        Instance = this;
        startPosition = slime.transform.localPosition;
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;

        print("Slime awake");
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    public void GameManagerOnOnGameStateChanged(GameState state)
    {
        if (state == GameState.MainMenu)
        {
            slime.transform.position = startPosition;
        }
    }

    public void speedUp()
    {
        if (setSpeed < maxSpeed)
        {
            setSpeed = setSpeed + speedIncrease;
        }
        
    }

    void Update()
    {
        if (isHiding)
        {
            speed = setSpeed / slowFraction;
            slime.GetComponent<Rigidbody2D>().gravityScale = 0.05f;

        }
        else
        {
            speed = setSpeed;
            slime.GetComponent<Rigidbody2D>().gravityScale = 2f;
        }

        

        if (GameManager.Instance.gameState == GameState.Playing && GameManager.Instance != null)
        {
            //automatic slime movement
            transform.Translate(Vector3.right * speed * Time.deltaTime);

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

    // Update the slime sprite based on the last gesture
    void UpdateSlimeSprite()
    {
        if (!isHiding)
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
            }
            else if (lastGesture == Shape.Suspended)
            {

                ChangeSlimeSprite(jump);
                if (!isHiding)
                {
                    // this.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpHold, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void ChangeSlimeSprite(Sprite sprite)
    {
        slime.GetComponent<SpriteRenderer>().sprite = sprite;
        slime.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        slime.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }


    public void UpdateGesture(Shape newGesture)
    {
        lastGesture = newGesture;
        resetGestureAfter = gestureResetSeconds;
        if (newGesture == Shape.Suspended && transform.position.y <= -7.31f) { //could maybe put it between 2 heights for the double jump spawning
            // this.GetComponent<Rigidbody2D>().AddForce(transform.up * initialJump, ForceMode2D.Impulse);
        }
    }

    public void ResetAll()
    {
        print("reset");
        setSpeed = baseSpeed;
        speed = baseSpeed;
        cheatOn = false;
        cheatText.SetActive(false);
    }
}