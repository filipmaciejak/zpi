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

    public GameObject overlay;

    private void Awake()
    {
        instance = this;
        overlay.SetActive(false);
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
        Debug.Log("Game stopped");
        Time.timeScale = 0;
        overlay.SetActive(true);
        isGameStopped = true;
    }

    public void ContinueGame()
    {
        Debug.Log("Game continued");
        Time.timeScale = 1;
        overlay.SetActive(false);
        isGameStopped = false;
    }

    public void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static GameStateManager Instance()
    {
        return instance;
    }
}
