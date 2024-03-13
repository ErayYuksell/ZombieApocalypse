using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameSectionController : MonoBehaviour
{
    private bool control = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (control) return;
            control = true;
            GameManager.Instance.GoToTheNextLevel();
            var playerController = other.GetComponent<PlayerController>();
            playerController.endSectionModule.PlayerEndSectionMovement();
        }
    }
}
