using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ZombieType { womanZombie, copZombie, yakuzaZombie }

[Serializable]
public class Zombie
{
    public ZombieType zombieType;
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
        var zombieClass = GetZombieClass(zombieType);
        animator.Play(zombieClass.dyingclip.name);
    }
    public void ChooseZombieAttackingAnim()
    {
        var zombieClass = GetZombieClass(zombieType);
        animator.Play(zombieClass.attackedClip.name);
    }
    public int ChoosePLayerBouncePower()
    {
        var zombieClass = GetZombieClass(zombieType);
        return zombieClass.bounceValue;
    }
    public Zombie GetZombieClass(ZombieType type)
    // enumdan secili olan zombie type ina bakarak o zombi tipini listedeki classlar arasindan bulacak ve o classtan isleme devam edicek
    {
        return zombieList.Find((zombie) => zombie.zombieType == type); // classlar arasindan Find fonksiyonu kullanarak istenilen tiptekini buluyoruz
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
