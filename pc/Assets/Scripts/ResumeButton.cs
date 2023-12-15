using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnResumeClicked);
    }

    void OnResumeClicked()
    {
        GameStateManager.Instance().ContinueGame();
    }
}