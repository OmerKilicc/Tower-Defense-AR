using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion


    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        UpdateGameState(GameState.DetectPlane);
    }

    public void UpdateGameState(GameState state) 
    {
        State = state;

        switch (State) 
        {
            case GameState.DetectPlane:
                break;
            case GameState.PlaceGameScene:
                break;
            case GameState.Playing: 
                break;
            case GameState.Lose: 
                break;
            case GameState.Win: 
                break;
        
        }

        OnGameStateChanged?.Invoke(State);
    }

    public void StartPlaying() 
    {
        UpdateGameState(GameState.Playing);
    }

    public enum GameState 
    {
        DetectPlane,
        PlaceGameScene,
        Playing,
        Lose,
        Win
    
    }
}
