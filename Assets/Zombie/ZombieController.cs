using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ZombieType { womanZombie, copZombie, yakuzaZombie }

[Serializable]
public class Zombie
{
    public AnimationClip dyingclip;
    public AnimationClip attackedClip;
    public int bounceValue;
}
public class ZombieController : MonoBehaviour
{
    bool canZombieMove = true;
    [SerializeField, Range(1, 3)] int zombieSpeed = 2;
    [SerializeField, Range(3, 10)] int zombieValue = 3;
    [SerializeField] int zombieShootCount = 0;
    Animator animator;
    BoxCollider myCollider;
    int bounceValue;
    [Space]
    public ZombieType zombieType;
    [Space]
    public List<Zombie> zombieList = new List<Zombie>();

    private void Start()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        ZombieMovement();
    }

    void ZombieMovement() // GenelZombie
    {
        if (canZombieMove)
        {
            transform.position += -Vector3.forward * zombieSpeed * Time.deltaTime;
        }
    }
    public void DestroyZombie() // GenelZombie
    {
        gameObject.SetActive(false);
    }
    public void ChooseZombieDyingAnim()
    {
        switch (zombieType)
        {
            case ZombieType.womanZombie:
                animator.Play(zombieList[0].dyingclip.name);
                break;
            case ZombieType.copZombie:
                animator.Play(zombieList[1].dyingclip.name);
                break;
            case ZombieType.yakuzaZombie:
                animator.Play(zombieList[2].dyingclip.name);
                break;
            default:
                break;
        }
    }
    public void ChooseZombieAttackingAnim()
    {
        switch (zombieType)
        {
            case ZombieType.womanZombie:
                animator.Play(zombieList[0].attackedClip.name);
                break;
            case ZombieType.copZombie:
                animator.Play(zombieList[1].attackedClip.name);
                break;
            case ZombieType.yakuzaZombie:
                animator.Play(zombieList[2].attackedClip.name);
                break;
            default:
                break;
        }
    }
    public int ChoosePLayerBouncePower()
    {
        switch (zombieType)
        {
            case ZombieType.womanZombie:
                bounceValue = zombieList[0].bounceValue;
                break;
            case ZombieType.copZombie:
                bounceValue = zombieList[1].bounceValue;
                break;
            case ZombieType.yakuzaZombie:
                bounceValue = zombieList[2].bounceValue;
                break;
        }
        return bounceValue;
    }

  
    public void ShootZombieCounter() //ZombieDeath icin farkli
    {
        zombieShootCount++;

        if (zombieShootCount >= zombieValue)
        {
            canZombieMove = false;
            myCollider.enabled = false;
            ChooseZombieDyingAnim();

        }
    }
    private void OnTriggerEnter(Collider other) // ZombieAttack icin farkli
    {
        if (other.CompareTag("Player"))
        {
            canZombieMove = false;
            myCollider.enabled = false;
            ChooseZombieAttackingAnim();
            var playerController = other.GetComponent<PlayerController>();
            playerController.playerDamageModule.BouncedPlayer(ChoosePLayerBouncePower());
        }
    }
}
