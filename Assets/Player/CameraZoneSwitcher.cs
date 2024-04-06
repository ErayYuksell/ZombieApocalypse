using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera primaryCamera;
    public CinemachineVirtualCamera[] virtualCameras;


    private void Start()
    {
        SwitchToCamera(primaryCamera);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MiddleSequence"))
        {
            CinemachineVirtualCamera targetCamera = other.GetComponentInChildren<CinemachineVirtualCamera>();
            SwitchToCamera(targetCamera);
        }
        if (other.CompareTag("EndGameSection"))
        {
            CinemachineVirtualCamera targetCamera = other.GetComponentInChildren<CinemachineVirtualCamera>();
            SwitchToCamera(targetCamera);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MiddleSequence"))
        {
            SwitchToCamera(primaryCamera);
        }
    }
    public void SwitchToCamera(CinemachineVirtualCamera targetCamera)
    {
        foreach (var camera in virtualCameras)
        {
            camera.enabled = camera == targetCamera;
        }
    }

    //// kullanirsin diye bir ipucu
    //[ContextMenu("Get All Virtual Cameras")]
    //void GetAllVirtualCameras()
    //{
    //    virtualCameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
    //}
}
