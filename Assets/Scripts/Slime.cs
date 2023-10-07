using System;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [NonSerialized] public static Slime Instance;
    private float speed = 8.0f; // Adjust the speed in the Inspector.


    private float gestureResetSeconds = 1.0f;

    private float resetGestureAfter = 0.0f;
    [NonSerialized]
    public bool isHiding = false;
    // Always in-sync w/ setSpeed
    private float slowFraction = 8.0f;
    private float baseSpeed = 8.0f;
    private float setSpeed = 8.0f;
    private float maxSpeed = 50.0f;
    private float speedIncrease = 0.5f;

    private float startXPosition = -37.16592f;
    [SerializeField]
    public GameObject slime;
    public CheatManager cheatManager;

    [NonSerialized]
    public Gesture lastGesture;

    // Sprites
    public Sprite normal;
    public Sprite tall;
    public Sprite flat;
    public Sprite circle;


    private void Awake()
    {
        Instance = this;
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
            // Moving the slime to the right
            transform.Translate(Vector3.right * speed * Time.deltaTime);

        }


        if (resetGestureAfter > 0.0f)
        {
            resetGestureAfter -= Time.deltaTime;
        } else
        {
            lastGesture = Gesture.Null;
        }

        UpdateSlimeSprite();
    }

    // Update the slime sprite based on the last gesture
    void UpdateSlimeSprite()
    {
        if (!isHiding)
        {
            if (lastGesture == Gesture.Null)
            {
                ChangeSlimeSprite(normal);
            }
            else if (lastGesture == Gesture.Up)
            {
                ChangeSlimeSprite(tall);
            }
            else if (lastGesture == Gesture.Down)
            {
                ChangeSlimeSprite(flat);
            }
            else if (lastGesture == Gesture.Tap)
            {
                ChangeSlimeSprite(circle);
            }
        }
    }

    private void ChangeSlimeSprite(Sprite sprite)
    {
        slime.GetComponent<SpriteRenderer>().sprite = sprite;
        slime.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        slime.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }


    public void SetGesture(Gesture newGesture)
    {
        lastGesture = newGesture;
        resetGestureAfter = gestureResetSeconds;
    }

    public void ResetAll()
    {
        print("reset");
        setSpeed = baseSpeed;
        speed = baseSpeed;
        gameObject.SetActive(false);
        
        isHiding = false;
        cheatManager.CheatModeDisabledHandler();

        slime.transform.position = new Vector3(startXPosition, slime.transform.position.y, slime.transform.position.z);
    }

    public GameObject GetGameObject()
    {
        return slime;
    }
}