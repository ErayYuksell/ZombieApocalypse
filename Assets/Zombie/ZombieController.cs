using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    bool canZombieMove = true;
    [SerializeField, Range(1, 3)] int zombieSpeed = 2;
    [SerializeField, Range(3, 10)] int zombieValue = 3;
    [SerializeField] int zombieShootCount = 0;
    [Space]
    Animator animator;
    [SerializeField] AnimationClip dyingclip;
    [SerializeField] AnimationClip attackedClip;
    BoxCollider myCollider;
    private void Start()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        ZombieMovement();
    }

    void ZombieMovement()
    {
        if (canZombieMove)
        {
            transform.position += -Vector3.forward * zombieSpeed * Time.deltaTime;
        }
    }
    public void DestroyZombie()
    {
        gameObject.SetActive(false);
    }
    public void ShootZombieCounter()
    {
        zombieShootCount++;

        if (zombieShootCount >= zombieValue)
        {
            canZombieMove = false;
            myCollider.enabled = false;
            animator.Play(dyingclip.name);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canZombieMove = false;
            myCollider.enabled = false;
            animator.Play(attackedClip.name);
            var playerController = other.GetComponent<PlayerController>();
            playerController.playerDamageModule.BouncedPlayer();
        }
    }
}
