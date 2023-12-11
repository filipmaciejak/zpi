using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine.SceneManagement;

public class QrCodeScript : MonoBehaviour
{
    [SerializeField]
    private RawImage rawImage;
    [SerializeField]
    private TextMeshProUGUI textLabelIp;
    [SerializeField]
    private TextMeshProUGUI textLabelNumberOfConnections;
    [SerializeField]
    private Button buttonNext;
    [SerializeField]
    private Button buttonPrevious;
    [SerializeField]
    private Button buttonRefresh;

    private Texture2D encodedTexture;
    private List<IPAddress> ipAddresses;
    private int currentIpIndex;
    const int MAX_NUMBER_OF_PLAYERS = 4;

    // Start is called before the first frame update
    void Start()
    {
        encodedTexture = new Texture2D(256, 256);
        OnRefresh();

        buttonNext.onClick.AddListener(OnNext);
        buttonPrevious.onClick.AddListener(OnPrevious);
        buttonRefresh.onClick.AddListener(OnRefresh);
    }


    private Color32[] Encode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width,
            }
        };

        return writer.Write(textForEncoding);
    }

    private void AcquireIpAddresses()
    {
        foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            IPInterfaceProperties ipProps = netInterface.GetIPProperties();
            //if no dhcp then it is probably wrong interface
            if (ipProps.DhcpServerAddresses.Count == 0)
            {
                continue;
            }
            foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
            {
                //loopback is 100% wrong address
                if (IPAddress.IsLoopback(addr.Address))
                    continue;

                if (addr.Address.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    ipAddresses.Add(addr.Address);
                    Debug.Log(addr.Address.ToString());
                }
            }
        }
    }

    private void ShowCurrentIp()
    {
        string textToEncode = ipAddresses[currentIpIndex].ToString();
        encodedTexture.SetPixels32(Encode(textToEncode, encodedTexture.width, encodedTexture.height));
        encodedTexture.Apply();
        rawImage.texture = encodedTexture;
        textLabelIp.text = textToEncode;
    }

    void OnRefresh()
    {
        ipAddresses = new List<IPAddress>();
        AcquireIpAddresses();
        currentIpIndex = 0;
        ShowCurrentIp();
        if(ipAddresses.Count == 0) 
        {
            //no nie wiem, coś wtedy nie tak jest
        }
        else if(ipAddresses.Count == 1) 
        {
            buttonNext.gameObject.SetActive(false);
            buttonPrevious.gameObject.SetActive(false);
        }
        else
        {
            buttonNext.gameObject.SetActive(true);
            buttonPrevious.gameObject.SetActive(true);
        }
    }

    void OnNext()
    {
        currentIpIndex = Math.Min(currentIpIndex + 1, ipAddresses.Count - 1);
        ShowCurrentIp();
    }

    void OnPrevious()
    {
        currentIpIndex = Math.Max(currentIpIndex - 1, 0);
        ShowCurrentIp();
    }

    private void Update()
    {
        int numberOfConnectedPlayers = ServerManager.Instance.NumberOfConnectedPlayers();
        textLabelNumberOfConnections.text = "Połączono " + numberOfConnectedPlayers + "/" + MAX_NUMBER_OF_PLAYERS;
        if(numberOfConnectedPlayers == MAX_NUMBER_OF_PLAYERS)
        {
            SceneManager.LoadScene(2);
        }

    }
}
