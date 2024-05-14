using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleSequenceController : MonoBehaviour
{
    bool zombiesInside;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zombiesInside = true;
            GetShootingTargetController();

            var playerController = other.GetComponent<PlayerController>();
            playerController.middleSequenceModule.MiddleSequenceAdjustment();
        }
    }
    private void OnTriggerStay(Collider other) // player middleSequence ye girdiginde icerde zombie varsa zombileri yok et
    {
        if (other.CompareTag("Zombie"))
        {
            //Debug.Log("There is a zombie inside");
            if (zombiesInside)
            {
                //Debug.Log("Destroy Zombies");
                other.gameObject.SetActive(false);
            }
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
