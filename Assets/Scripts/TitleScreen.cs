using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public Slime slimeScript;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if title screen has been clicked 5 times
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && touch.tapCount == 5)
            {
                cheatMode();
            }
        }
    }

    void cheatMode()
    {
        //enable cheat mode
        slimeScript.cheatOn = true;

        //show text underneath title screen\
        text.text = "Cheat Mode Activated";

    }
}
