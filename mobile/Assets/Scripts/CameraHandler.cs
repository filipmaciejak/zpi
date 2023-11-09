using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZXing;

public class CameraHandler : MonoBehaviour
{
    public RawImage rawimage;

    public event Action<string> QrRead;

    IBarcodeReader reader;
    WebCamTexture webcamTexture;
    bool isScanning;


    // Start is called before the first frame update
    void Start()
    {
        //hopefully first camera is back camera
        webcamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        rawimage.texture = webcamTexture;
        rawimage.material.mainTexture = webcamTexture;
        webcamTexture.Play();

        reader = new BarcodeReader();
        isScanning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isScanning)
        {
            var result = reader.Decode(webcamTexture.GetPixels32(), webcamTexture.width, webcamTexture.height);
            if (result != null)
            {
                Debug.Log(result.BarcodeFormat.ToString());
                Debug.Log(result.Text);
                QrRead?.Invoke(result.Text);
            }
        }
    }

    public void StopScanning() { isScanning=false; }

    public void StartScanning() { isScanning=true; }

}
