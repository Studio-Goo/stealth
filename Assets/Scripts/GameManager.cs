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
    public ObstacleSpawner obstacleSpawner; 
    public StartAnimation startAnimation;
    public EndAnimation endAnimation;

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
                HandleMainScene();
                break;

            case GameState.StartAnimation:
                startAnimation.HandleStartAnimation();
                break;

            case GameState.Playing:
                break;

            case GameState.Paused:
                break;

            case GameState.GameOverAnimation:
                endAnimation.HandleEndAnimation();
                break;

            case GameState.GameOver:
                HandleGameOver();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }

        // Only trigger event if anything is subscribed to it
        OnGameStateChanged?.Invoke(newState);
    }

    public void HandleMainScene()
    {
        //UpdateGameState(GameState.Playing);
    }

    public void HandleGameOver()
    {
        slime.setSpeed = 8.0f;
        pointSystem.ResetPoints();
        obstacleSpawner.count = 0;
        obstacleSpawner.tutorial = true;
    }

}
