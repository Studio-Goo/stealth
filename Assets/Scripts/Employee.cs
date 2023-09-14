using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{

    //public float speed = 5.0f;
    public Sprite notLooking;
    public float notLookingDistance = 20.0f;
    public Sprite halfLooking;
    public float halfLookingDistance = 10.0f;
    public Sprite fullLooking;
    public float fullLookingDistance = 1.0f;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        float distance = this.transform.position.x - player.transform.position.x;
        //Debug.Log(this.name);
        //Debug.Log(distance);
        
        if (distance > halfLookingDistance)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = notLooking;
        }
        else if (distance > fullLookingDistance && distance < halfLookingDistance)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = halfLooking;
        }
        else if (distance > 0 && distance < fullLookingDistance)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = fullLooking;
        }

        //transform.Translate(Vector3.left * speed * Time.deltaTime);

        //if(player.GetComponent<Slime>().CanMove())
        //{
        //    speed = 5.0f;
        //} else
        //{
        //   speed = 0;
        //}
    }
}
