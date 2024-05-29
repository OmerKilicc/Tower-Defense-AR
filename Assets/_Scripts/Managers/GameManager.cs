using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameState StartState = GameState.DetectPlane;
    private void Start()
    {
        UpdateGameState(StartState);
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


    public enum GameState 
    {
        DetectPlane,
        PlaceGameScene,
        Playing,
        Lose,
        Win
    
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
