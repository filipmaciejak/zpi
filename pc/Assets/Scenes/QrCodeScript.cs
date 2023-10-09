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

public class QrCodeScript : MonoBehaviour
{
    [SerializeField]
    private RawImage rawImage;

    private Texture2D encodedTexture;
    private List<IPAddress> ipAddresses;


    // Start is called before the first frame update
    void Start()
    {
        ipAddresses = new List<IPAddress>();
        encodedTexture = new Texture2D(256, 256);
        AcquireIpAddresses();

        string textToEncode = ipAddresses[0].ToString();
        encodedTexture.SetPixels32(Encode(textToEncode, encodedTexture.width, encodedTexture.height));
        encodedTexture.Apply();
        rawImage.texture = encodedTexture;
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
}
