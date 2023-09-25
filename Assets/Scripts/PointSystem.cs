using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    public GameObject pointSystem;
    private int points = 0;
    // Start is called before the first frame update
    void Start()
    {
       //disable point system
        pointSystem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void pointCounter()
    {
        if (PlayerPrefs.HasKey("Tutorial") && PlayerPrefs.GetInt("Tutorial") == 1)
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

        pointSystem.SetActive(false);
    }
}
