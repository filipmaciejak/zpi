using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameEndCondition
{
    TIME_RUN_OUT = 0,
    TEAM_1_WIN = 1,
    TEAM_2_WIN = 2,
    DRAW = 3
}

public class GameStateManager : MonoBehaviour
{
    bool isGameStopped = false;

    static GameStateManager instance;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameStopped)
            {
                ContinueGame();
                isGameStopped = false;
            }
            else
            {
                StopGame();
                isGameStopped = true;
            }
        }
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public static GameStateManager Instance()
    {
        return instance;
    }
}
