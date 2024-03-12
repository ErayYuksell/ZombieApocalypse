using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameSectionController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GameManager.Instance.GoToTheNextLevel();
            var playerController = other.GetComponent<PlayerController>();
            playerController.endSectionModule.PlayerEndSectionMovement();
        }
    }
}
