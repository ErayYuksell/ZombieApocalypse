using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleSequenceController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //var shootingTarget = transform.Find("ShootingTarget").gameObject;
            //shootingTarget.GetComponent<ShootingTargetController>().SetCanShoot();
            GetShootingTargetController();

            var playerController = other.GetComponent<PlayerController>();
            playerController.middleSequenceModule.MiddleSequenceAdjustment();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.middleSequenceModule.MiddleSequenceReverse();
            gameObject.SetActive(false);
        }
    }

    public void GetShootingTargetController()
    {
        var body = transform.Find("Body").gameObject;

        for (int i = 0; i < body.transform.childCount; i++)
        {
            body.transform.GetChild(i).GetComponent<ShootingTargetController>().SetCanShoot();
        }
    }
}
