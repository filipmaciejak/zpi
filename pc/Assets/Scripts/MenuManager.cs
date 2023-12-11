using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Button StartButton;
    [SerializeField]
    private Button ExitButton;


    void Start()
    {
        StartButton.onClick.AddListener(StartGame);
        ExitButton.onClick.AddListener(ExitGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene("QRCodeScene");
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
