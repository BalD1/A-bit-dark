using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if(instance == null)
                Debug.LogError("UIManager instance not found");

            return instance;
            ;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void WindowManager(GameManager.GameState state)
    {
        switch(state)
        {
            case GameManager.GameState.MainMenu:
                break;
            case GameManager.GameState.InGame:
                break;
            case GameManager.GameState.Pause:
                break;
        }
    }


}
