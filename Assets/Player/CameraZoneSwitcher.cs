using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraZoneSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera primaryCamera;
    public CinemachineVirtualCamera[] virtualCameras;
    [SerializeField] float transitionTime = 2.5f;

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
        if (other.CompareTag("LabCamCollider"))
        {
            StartCoroutine(LabCamTransition(other.gameObject));
        }
    }
    public IEnumerator LabCamTransition(GameObject other) // lab came 2 saniyede gecis yapiyor 2 saniye sonra GameOverPaneli aciyorum
    {
        CinemachineVirtualCamera targetCamera = other.GetComponentInChildren<CinemachineVirtualCamera>();
        SwitchToCamera(targetCamera);

        yield return new WaitForSeconds(transitionTime);
        UIManager.Instance.OpenGameOverPanel();
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
