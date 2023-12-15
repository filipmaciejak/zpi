using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaveGameButton : MonoBehaviour
{
    void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnLeaveClicked);
    }

    void OnLeaveClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        Destroy(GameObject.Find("EventManager"));
        Destroy(GameObject.Find("ServerManager"));
        Time.timeScale = 1;
    }
}