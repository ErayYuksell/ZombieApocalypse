using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    // yorum satirlari obstaclelarin mermi ile belirli sayida temasi sonucunda yok olmasi ve animasyon girmesi idi
    //Animator animator;
    //[SerializeField] AnimationClip clip;
    //[SerializeField, Range(3, 14)] int obstacleValue = 3;
    //[SerializeField] int obstacleCount = 0;
    private void Start()
    {
        //animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.playerDamageModule.BouncedPlayer();
            gameObject.SetActive(false);
        }
    }
    //public void DestroyObstacle()
    //{
    //    if (obstacleCount >= obstacleValue)
    //    {
    //        gameObject.SetActive(false);
    //        return;
    //    }
    //    obstacleCount++;
    //}
    //public void PlayObstacleHitAnim()
    //{
    //    animator.Play(clip.name);
    //}
}
