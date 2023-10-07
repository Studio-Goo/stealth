using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;
    public GameObject cheatText;
    public TextMeshProUGUI text;

    private bool cheatMode = false;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Tap screen 5 times to enable the cheatMode
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && touch.tapCount == 5)
            {
                CheatModeEnabledHandler();
                cheatMode = true;
            }
        }
    }

    public bool GetCheatMode()
    {
        return cheatMode;
    }

    private void CheatModeEnabledHandler()
    {
        print("cheat on");

        cheatText.SetActive(true);
        text.text = "Cheat Mode Activated";
    }

    public void CheatModeDisabledHandler()
    {
        cheatMode = false;
        cheatText.SetActive(false);
        text.text = "";
    }
}
