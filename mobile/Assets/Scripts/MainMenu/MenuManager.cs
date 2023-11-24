using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MenuManager : MonoBehaviour
    {

        public Button connectButton;

        private void OnEnable()
        {
            connectButton.StartedPress += OnConnectButton;
        }

        private void OnDisable()
        {
            connectButton.StartedPress -= OnConnectButton;
        }

        private void OnConnectButton(string ignored)
        {
            SceneManager.LoadScene("QrReadingScene");
        }
    }
}
