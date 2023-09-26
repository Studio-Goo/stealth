using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScore : MonoBehaviour
{
    public PointSystem pointSystemScript;
    // Start is called before the first frame update
    void Start()
    {
        pointSystemScript.endPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayScore()
    {
        Debug.Log("Score");
    }
}
