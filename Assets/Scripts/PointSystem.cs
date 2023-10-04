using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    public static PointSystem Instance;

    public GameObject pointSystem;
    public GameObject endScreen;
    // Temp public to more easily debug level chunks
    private int points = 0;
    // Start is called before the first frame update
    void Start()
    {
       //disable point system
        pointSystem.SetActive(false);
        Instance = this;
    }

    public int GetPoints()
    {
        return points;
    }

    public void pointCounter()
    {
        print("tutorial: " + PlayerPrefs.GetInt("Tutorial"));
        if (PlayerPrefs.HasKey("Tutorial") && PlayerPrefs.GetInt("Tutorial") == 0)
        {
            points++;
        }
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
        endScreen.SetActive(true);

        TMPro.TextMeshProUGUI screenText = endScreen.GetComponent<TMPro.TextMeshProUGUI>();
        
        print("endpoints: " + points);
        //display score and highscore
        screenText.text = "Score: " + points.ToString();
        if (PlayerPrefs.GetInt("HighScore") > 0)
        {
            screenText.text += "\n<size=50%>Highscore: " + PlayerPrefs.GetInt("HighScore").ToString() + "</size>";
        }
    }

    public void ResetPoints()
    {
        endPoints();

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