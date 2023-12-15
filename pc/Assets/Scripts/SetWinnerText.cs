using UnityEngine;

public class SetWinnerText : MonoBehaviour
{
    public GameEndStateObject gameEndStateObject;

    void Start()
    {
        if (gameEndStateObject.gameResult == GameEndStateObject.GameResult.TEAM_1_WIN)
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = "Wygrywa zespół 1!";
        }
        else if (gameEndStateObject.gameResult == GameEndStateObject.GameResult.TEAM_2_WIN)
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = "Wygrywa zespół 2!";
        }
        else
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = "Remis!";
        }
    }
}