using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject player;
    public Slime slimeScript;
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
        gameoverMenuPanel.SetActive(state == GameState.GameOver);
    }


    public void HandleStartGameClick()
    {
        mainMenuPanel.SetActive(false);
        gameMenuPanel.SetActive(true);
        pausedMenuPanel.SetActive(false);
        
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

    public void HandlePauseClick()
    {
        gameMenuPanel.SetActive(false);
        pausedMenuPanel.SetActive(true);
        GameManager.Instance.UpdateGameState(GameState.Paused);
    }

    public void HandleReplayClick()
    {
        gameoverMenuPanel.SetActive(false);
        pausedMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        slimeScript.ResetAll();
        player.SetActive(false);

        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }
}
