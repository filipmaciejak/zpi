using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZXing;

public class CameraHandler : MonoBehaviour
{
    public GameObject clientManagerObject;
    public RawImage rawimage;

    ClientManager clientManager;
    IBarcodeReader reader;
    WebCamTexture webcamTexture;

    // Start is called before the first frame update
    void Start()
    {
        clientManager = clientManagerObject.GetComponent<ClientManager>();

        //hopefully first camera is back camera
        webcamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        rawimage.texture = webcamTexture;
        rawimage.material.mainTexture = webcamTexture;
        webcamTexture.Play();

        reader = new BarcodeReader();
    }

    // Update is called once per frame
    void Update()
    {
        var result = reader.Decode(webcamTexture.GetPixels32(), webcamTexture.width, webcamTexture.height);
        if (result != null)
        {
            Debug.Log(result.BarcodeFormat.ToString());
            Debug.Log(result.Text);

            //will throw exception for bad ip format
            clientManager.ConnectToIp(result.Text);
            //change to next scene
            Destroy(this);
        }
    }



}
