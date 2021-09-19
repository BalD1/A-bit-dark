using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
                Debug.LogError("GameManager Instance not found");

            return instance;
        }
    }

    public CheckPoint.PointTransform respawnTransform;

    #region GameState

    public enum GameState
    {
        MainMenu,
        InGame,
        Pause,
    }

    public GameState gameState;

    public void StateOfGame(GameState newState)
    {
        gameState = newState;

        switch(newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.InGame:
                break;
            case GameState.Pause:
                break;
        }

        UIManager.Instance.WindowManager(newState);
    }

    #endregion


    private void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        StateOfGame(gameState);
    }

}
