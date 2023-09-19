using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    private GameObject pointSystem;
    private int points = 0;
    // Start is called before the first frame update
    void Start()
    {
       //set pointSystem text to 0
        pointSystem = GameObject.Find("pointSystem");
        pointSystem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void pointCounter(bool tutorial)
    {
        if (tutorial)
        {
            //do nothing lol
        }
        else{
            points++;
        }
    }

    public void showPoints()
    {
        pointSystem.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + points.ToString();
    }
}
