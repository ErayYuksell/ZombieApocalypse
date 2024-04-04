using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleSequenceController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
        }
    }
}
