using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, StartAnimation, Playing, Paused, GameOverAnimation, GameOver };

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;
    public Slime slime;
    public PointSystem pointSystem;
    public StartAnimation startAnimation;
    public EndAnimation endAnimation;
    public AudioSource backgroundMusic;
    public GameObject endScreen;
    public SlimePosition slimePosition;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.MainMenu);
    }

    public void UpdateGameState(GameState newState)
    {
        gameState = newState;

        switch (newState)
        {
            case GameState.MainMenu:

                endScreen.SetActive(false);
                slime.GetGameObject().SetActive(false);

                break;

            case GameState.StartAnimation:

                slimePosition.ResetPosition();
                startAnimation.HandleStartAnimation();

                break;

            case GameState.Playing:

                endScreen.SetActive(false); //just in case
                backgroundMusic.Play();

                break;

            case GameState.Paused:
                break;

            case GameState.GameOverAnimation:

                backgroundMusic.Stop();
                endAnimation.HandleEndAnimation();

                break;

            case GameState.GameOver:

                slime.ResetAll();
                slimePosition.ResetPosition();
                pointSystem.ResetPoints();
                slime.GetGameObject().SetActive(false);
                PlayerPrefs.SetInt("obstaclePassed", 0);

                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        // Only trigger event if anything is subscribed to it
        OnGameStateChanged?.Invoke(newState);
    }

}
