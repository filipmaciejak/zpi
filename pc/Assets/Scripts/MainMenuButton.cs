using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnMainMenuClicked);
    }

    void OnMainMenuClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
        Destroy(GameObject.Find("EventManager"));
        Destroy(GameObject.Find("ServerManager"));
        Time.timeScale = 1;
    }
}
