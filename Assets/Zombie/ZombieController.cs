using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ZombieType { womanZombie, copZombie, yakuzaZombie }
public class ZombieController : MonoBehaviour
{
    bool canZombieMove = true;
    [SerializeField, Range(1, 3)] int zombieSpeed = 2;
    [SerializeField, Range(3, 10)] int zombieValue = 3;
    [SerializeField] int zombieShootCount = 0;
    Animator animator;
    [Space]
    [SerializeField] AnimationClip[] dyingclips;
    [SerializeField] AnimationClip[] attackedClips;
    //[SerializeField] AnimationClip dyingclip;
    //[SerializeField] AnimationClip attackedClip;
    BoxCollider myCollider;

    public ZombieType zombieType;
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
                animator.Play(dyingclips[0].name);
                break;
            case ZombieType.copZombie:
                animator.Play(dyingclips[1].name);
                break;
            case ZombieType.yakuzaZombie:
                animator.Play(dyingclips[2].name);
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
                animator.Play(attackedClips[0].name);
                break;
            case ZombieType.copZombie:
                animator.Play(attackedClips[1].name);
                break;
            case ZombieType.yakuzaZombie:
                animator.Play(attackedClips[2].name);
                break;
            default:
                break;
        }
    }
    public void ShootZombieCounter() //ZombieDeath icin farkli
    {
        zombieShootCount++;

        if (zombieShootCount >= zombieValue)
        {
            canZombieMove = false;
            myCollider.enabled = false;
            //animator.Play(dyingclip.name);
            ChooseZombieDyingAnim();
        }
    }
    private void OnTriggerEnter(Collider other) // ZombieAttack icin farkli
    {
        if (other.CompareTag("Player"))
        {
            canZombieMove = false;
            myCollider.enabled = false;
            //animator.Play(attackedClip.name);
            ChooseZombieAttackingAnim();
            var playerController = other.GetComponent<PlayerController>();
            playerController.playerDamageModule.BouncedPlayer();
        }
    }
}
