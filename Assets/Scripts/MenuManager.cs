using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject player;
    public Slime slime;
    [SerializeField] private GameObject mainMenuPanel, gameMenuPanel, pausedMenuPanel, gameoverMenuPanel;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    private void GameManagerOnOnGameStateChanged(GameState state)
    {
        mainMenuPanel.SetActive(state == GameState.MainMenu);
        gameMenuPanel.SetActive(state == GameState.Playing);
        pausedMenuPanel.SetActive(state == GameState.Paused);
        gameoverMenuPanel.SetActive(state == GameState.GameOver);
    }


    public void HandleStartGameClick()
    {
        mainMenuPanel.SetActive(false);
        pausedMenuPanel.SetActive(false);
        gameoverMenuPanel.SetActive(false); 

        gameMenuPanel.SetActive(true);
        
        //check if player is active
        if (player.activeSelf)
        {
            GameManager.Instance.UpdateGameState(GameState.Playing);
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.StartAnimation);
        }
    }

    public void HandleResumeClick()
    {
        mainMenuPanel.SetActive(false);
        pausedMenuPanel.SetActive(false);
        gameoverMenuPanel.SetActive(false);

        gameMenuPanel.SetActive(true);

        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    public void HandlePauseClick()
    {
        mainMenuPanel.SetActive(false);
        gameoverMenuPanel.SetActive(false);
        gameMenuPanel.SetActive(false);

        pausedMenuPanel.SetActive(true);

        GameManager.Instance.UpdateGameState(GameState.Paused);
    }

    public void HandleReplayClick()
    {
        gameoverMenuPanel.SetActive(false);
        gameMenuPanel.SetActive(false);
        pausedMenuPanel.SetActive(true);

        mainMenuPanel.SetActive(true);
        slime.ResetAll();
        

        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }
}
