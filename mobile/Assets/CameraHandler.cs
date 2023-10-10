using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class CameraHandler : MonoBehaviour
{

    public RawImage rawimage;
    // Start is called before the first frame update
    void Start()
    {
        //hopefully first camera is back camera
        WebCamTexture webcamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        rawimage.texture = webcamTexture;
        rawimage.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
    }
}
