using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] float obstacleValue = 0.05f;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.playerDamageModule.BouncedPlayer();
            playerController.fireModule.SetFireRate(obstacleValue);
            gameObject.SetActive(false);
        }
    }
   
}
