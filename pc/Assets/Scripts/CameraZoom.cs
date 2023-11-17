using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private int minSize = 4;

    [SerializeField]
    private int maxSize = 20;

    [Range(1f, 2f)]
    [SerializeField]
    private float cameraPadding = 1.2f;

    [SerializeField]
    private GameObject mech1;

    [SerializeField]
    private GameObject mech2;


    private Camera mainCamera;


    void Start()
    {
        mainCamera = GetComponent<Camera>();
        mainCamera.orthographicSize = minSize;
        mainCamera.aspect = 1;
    }

    // Update is called once per frame
    void Update()
    {
        SetCameraPosition();
        CheckForCameraZoomChange();
    }

    private void CheckForCameraZoomChange()
    {
        float mechDistance = Math.Max(Math.Abs(mech1.transform.position.x - mech2.transform.position.x), Math.Abs(mech1.transform.position.y - mech2.transform.position.y));
        mainCamera.orthographicSize = Mathf.Lerp(minSize, maxSize, mechDistance/maxSize) / 2f * cameraPadding;
    }

    private void SetCameraPosition()
    {
        gameObject.transform.position = new Vector3((mech1.transform.position.x + mech2.transform.position.x) / 2, (mech1.transform.position.y + mech2.transform.position.y) / 2, -10);
    }
}
