using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QrReadingManager : MonoBehaviour
{
    public GameObject ClientManager;
    public GameObject PopUpPanel;
    private CameraHandler CameraHandler;

    private double TimerStartSeconds;
    private double TimerLengthSeconds;
    private bool connectionSuccess;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        PopUpPanel.SetActive(false);
        CameraHandler = GetComponent<CameraHandler>();
        CameraHandler.QrRead += OnQrRead;
        ClientManager.GetComponent<ClientManager>().Connect += OnConnect;
        ClientManager.GetComponent<ClientManager>().Disconnect += OnDisconnect;
        TimerLengthSeconds = 3.0;
        TimerStartSeconds = -1;
        connectionSuccess = false;
    }

    private void OnDisable()
    {
        ClientManager.GetComponent<ClientManager>().Connect -= OnConnect;
        ClientManager.GetComponent<ClientManager>().Disconnect -= OnDisconnect;
    }

    // Update is called once per frame
    void Update()
    {
        if(TimerStartSeconds > 0 && Time.timeAsDouble - TimerStartSeconds > TimerLengthSeconds)
        {
            if(connectionSuccess)
            {
                SceneManager.LoadScene("MovementScene");
            }
            PopUpPanel.SetActive(false);
            TimerStartSeconds = -1;
        }
    }

    void OnQrRead(string ip_text)
    {
        CameraHandler.StopScanning();
        PopUpPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Trwa łączenie urządzeń... Poczekaj chwilę"; ;
        TimerStartSeconds = -1;
        try
        {
            ClientManager.GetComponent<ClientManager>().ConnectToIp(ip_text);
        }catch (Exception)
        {
            PopUpPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Nieprawidłowy format adresu. Czy na pewno zeskanowałeś właściwy kod QR?";
            TimerStartSeconds = Time.timeAsDouble;
        }
        PopUpPanel.SetActive(true);
    }

    void OnConnect()
    {
        PopUpPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Sukces! Urządzenia zostały połączone";
        TimerStartSeconds = Time.timeAsDouble;
        connectionSuccess = true;
    }

    void OnDisconnect()
    {
        PopUpPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Błąd. Nie udało się połączyć urządzeń. Upewnij się, że znajdują się one w tej samej sieci";
        TimerStartSeconds = Time.timeAsDouble;
        connectionSuccess = false;
        CameraHandler.StartScanning();
    }
}
