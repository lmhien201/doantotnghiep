using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSwitch : MonoBehaviour
{
    public Camera fullView;
    public Camera camtruoc;
    public Camera camsau;

    public void ShowFullViewCamera()
    {
        camtruoc.enabled = false;
        camsau.enabled = false;
        fullView.enabled = true;
    }

    public void ShowCamSau()
    {
        camtruoc.enabled = false;
        camsau.enabled = true;
        fullView.enabled = false;
    }

    public void ShowCamTruoc()
    {
        camtruoc.enabled = true;
        camsau.enabled = false;
        fullView.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            ShowCamSau(); 
        else if (Input.GetKeyDown(KeyCode.X))
            ShowCamTruoc();
        else if (Input.GetKeyDown(KeyCode.C))
            ShowFullViewCamera();
    }
}
