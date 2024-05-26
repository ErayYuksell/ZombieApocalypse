using DG.Tweening;
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
    public int antiBodyNumber;
}
public class ZombieController : MonoBehaviour
{
    bool canZombieMove = true;
    [SerializeField, Range(1, 3)] int zombieSpeed = 2;
    [SerializeField, Range(3, 10)] int zombieValue = 3;
    [SerializeField] float zombieShootCount = 0;
    Animator animator;
    BoxCollider myCollider;
    [Space]
    public ZombieType zombieType;
    [Space]
    public List<Zombie> zombieList = new List<Zombie>();
    [Space]
    public AntiBodyDropModule antiBodyDropModule;

    Rigidbody rb;

    GameObject bloodEffectObject;
    ParticleSystem BlodParticleSystem;
    //GameObject antibodyEffectObject; // antibody effecti eklemeye calistim olmadi saldim
    //ParticleSystem antibodyEffect;


    private void Start()
    {
        antiBodyDropModule.Init(this);

        animator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider>();

        antiBodyDropModule.CreateAntiBody();

        rb = GetComponent<Rigidbody>();

        bloodEffectObject = transform.Find("BloodEffect_1").gameObject;
        BlodParticleSystem = bloodEffectObject.GetComponent<ParticleSystem>();

        //antibodyEffectObject = transform.Find("AntiBody").gameObject;
        //antibodyEffect = antibodyEffectObject.GetComponentInChildren<ParticleSystem>();
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
        antiBodyDropModule.GetAntiBody();
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
    public void ShootZombieCounter(float value) //ZombieDeath icin farkli
    {
        zombieShootCount += value;
        BlodParticleSystem.Play();

        if (zombieShootCount >= zombieValue)
        {
            canZombieMove = false;
            myCollider.enabled = false;
            ChooseZombieDyingAnim();

        }
    }

    public void OvercomeObstacles() // zombiler engellere denk geldiginde bulundugu konuma gore engeli atlatacak
    {
        if (transform == null) // Dotween hata verdigi icin yazmak zorunda kaldim 
        {
            return;
        }

        if (transform.position.x > 0)
        {
            transform.DOMoveX(transform.position.x - UnityEngine.Random.Range(3, 5), 3);
        }
        else
        {
            transform.DOMoveX(transform.position.x + UnityEngine.Random.Range(3, 5), 3);
        }
    }
    private void OnTriggerEnter(Collider other) // ZombieAttack icin farkli
    {
        if (other.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll; // zombi hareketini tamamen durdurmak icin

            canZombieMove = false;
            myCollider.enabled = false;
            ChooseZombieAttackingAnim();
            var playerController = other.GetComponent<PlayerController>();
            playerController.playerDamageModule.BouncedPlayer(ChoosePLayerBouncePower());
        }
        if (other.CompareTag("Obstacle"))
        {
            OvercomeObstacles();
        }
        if (other.CompareTag("BreakableWall"))
        {
            OvercomeObstacles();
        }
    }

    [Serializable]
    public class AntiBodyDropModule
    {
        ZombieController zombieController;
        public GameObject antiBody;
        public float cureSliderValue = 1;
        public List<GameObject> antiBodyList = new List<GameObject>();

        public void Init(ZombieController zombieController)
        {
            this.zombieController = zombieController;
        }

        public void CreateAntiBody() // starttta hangi zombinin kac antikoru varsa uret set activlerini kapat 
        {
            var zombieClass = zombieController.GetZombieClass(zombieController.zombieType);

            for (int i = 0; i < zombieClass.antiBodyNumber; i++)
            {
                var obj = Instantiate(antiBody, zombieController.transform);
                obj.transform.position = zombieController.transform.position;
                obj.SetActive(false);
                antiBodyList.Add(obj);
            }
        }
        // antibodyleri ac ve hareketlerini sagla daha sonra kapa ????
        public void GetAntiBody()
        {
            foreach (var item in antiBodyList)
            {
                item.SetActive(true);
                //zombieController.antibodyEffect.Play();

                item.transform.DOJump(new Vector3(item.transform.position.x + UnityEngine.Random.Range(-1.5f, 1.5f), item.transform.position.y + 0.28f, item.transform.position.z + UnityEngine.Random.Range(-1.5f, 1.5f)), 2, 1, 1).OnComplete(() =>
                {
                    EndGameSectionController.Instance.cureProgressModel.CureSliderIncrease(cureSliderValue);
                });
            }
        }
    }

}
