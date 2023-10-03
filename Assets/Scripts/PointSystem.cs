using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    public static PointSystem Instance;

    public GameObject pointSystem;
    public GameObject endScreen;
    private int points = 0;
    // Start is called before the first frame update
    void Start()
    {
       //disable point system
        pointSystem.SetActive(false);
        Instance = this;
    }

    public void pointCounter()
    {
        //if (!(PlayerPrefs.HasKey("Tutorial") && PlayerPrefs.GetInt("Tutorial") == 1))
        //{
            points++;
            Debug.Log(points);
        //}
    }

    public void enable()
    {
        pointSystem.SetActive(true);
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

    public void endPoints()
    {
        TMPro.TextMeshProUGUI screenText = endScreen.GetComponent<TMPro.TextMeshProUGUI>();
        
        //display score and highscore
        screenText.text = "Score: " + points.ToString();
        if (PlayerPrefs.GetInt("HighScore") > 0)
        {
            screenText.text += "\n<size=50%>Highscore: " + PlayerPrefs.GetInt("HighScore").ToString() + "</size>";
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

        pointSystem.SetActive(false);
    }
}
