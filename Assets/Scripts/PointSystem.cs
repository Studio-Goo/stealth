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
        TMPro.TextMeshProUGUI pointText = pointSystem.GetComponent<TMPro.TextMeshProUGUI>();
        pointText.text = "Score: " + points.ToString();

        if (PlayerPrefs.GetInt("HighScore") > 0)
        {
            pointText.text += "\n<size=50%>Highscore: " + PlayerPrefs.GetInt("HighScore").ToString() + "</size>";
        }
    }



    public void ResetPoints()
    {
        //save highscore
        if (points > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", points);
            PlayerPrefs.Save();
        }

        //reset points
        points = 0;
    }
}
