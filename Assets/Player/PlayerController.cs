using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public MovementModule movementModule;
    public GateModule gateModule;
    public FireModule fireModule;
    public PLayerDamageModule playerDamageModule;
    public EndSectionModule endSectionModule;

    Animator animator;
    GameObject m16RIfle;

    void Start()
    {
        movementModule.Init(this);
        gateModule.Init(this);
        fireModule.Init(this);
        playerDamageModule.Init(this);
        endSectionModule.Init(this);

        animator = GetComponent<Animator>();
        m16RIfle = transform.Find("M16RIfle").gameObject;

        StartCoroutine(fireModule.SetFire());
    }


    void Update()
    {
        movementModule.PlayerMovement();

    }

    public void StartFireCaroutine()
    {
        StartCoroutine(fireModule.SetFire());
    }

    [Serializable]
    public class MovementModule
    {
        PlayerController playerController;
        float xSpeed;
        public bool canMove = true;
        public float xLeftValue = -4;
        public float xRightValue = 4;
        [Range(0, 4)] public float playerSpeed = 4;
        public void Init(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void PlayerMovement()
        {
            if (!canMove)
            {
                return;
            }
            float touchX = 0;
            float newXValue;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                xSpeed = 250f;
                touchX = Input.GetTouch(0).deltaPosition.x / Screen.width;
            }
            else if (Input.GetMouseButton(0))
            {
                xSpeed = 250f;
                touchX = Input.GetAxis("Mouse X");
            }
            newXValue = playerController.transform.position.x + xSpeed * touchX * Time.deltaTime;
            newXValue = Mathf.Clamp(newXValue, xLeftValue, xRightValue);
            Vector3 playerNewPosition = new Vector3(newXValue, playerController.transform.position.y, playerController.transform.position.z + playerSpeed * Time.deltaTime);
            playerController.transform.position = playerNewPosition;
        }
    }

    [Serializable]
    public class GateModule
    {
        PlayerController playerController;

        public void Init(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void AdjustFireSkills(GateType gateType, float gateValue)
        {
            switch (gateType)
            {
                case GateType.Range:
                    playerController.fireModule.range += playerController.fireModule.rateScale * gateValue;
                    break;
                case GateType.Rate:
                    playerController.fireModule.rate -= playerController.fireModule.rangeScale * gateValue;
                    break;
                case GateType.Power:
                    playerController.fireModule.power += playerController.fireModule.powerScale * gateValue;
                    break;
                default:
                    break;
            }
        }
    }

    [Serializable]
    public class FireModule
    {
        PlayerController playerController;

        [Range(0.2f, 1)] public float rate = 0.4f;
        [Range(8, 20)] public float range = 15;
        [Range(0.8f, 2)] public float power;
        [Space]
        public float rateScale;
        public float rangeScale;
        public float powerScale;
        [Space]
        public GameObject objectPool;
        public Transform firePoint;
        public bool canFire = true;
        public void Init(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public IEnumerator SetFire()
        {
            while (canFire)
            {
                var objectPoolScript = objectPool.GetComponent<ObjectPool>();
                var obj = objectPoolScript.GetPoolObjects();
                obj.transform.position = firePoint.position;
                yield return new WaitForSeconds(rate);
            }
        }
        public void SetFireRate(float value)
        {
            rate += value;
        }
        public void CloseTheAllBullet()
        {
            var objectPoolScript = objectPool.GetComponent<ObjectPool>();

            foreach (var item in objectPoolScript.queue)
            {
                item.SetActive(false);
            }
        }

    }
    [Serializable]
    public class PLayerDamageModule
    {
        PlayerController playerController;
        [Range(0.2f, 1)] public float bounceTime = 0.5f;
        public void Init(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void BouncedPlayer(int value = 2)
        {
            playerController.movementModule.canMove = false;
            playerController.fireModule.canFire = false;
            playerController.transform.DOMoveZ(playerController.transform.position.z - value, bounceTime).OnComplete(() =>
            {
                playerController.movementModule.canMove = true;
                playerController.fireModule.canFire = true;
                playerController.StartFireCaroutine();
            });
        }
    }
    [Serializable]
    public class EndSectionModule
    {
        PlayerController playerController;
        public AnimationClip walkAnim;
        public AnimationClip victoryAnim;
        public float walkDistance = 7;
        public float walkingTime = 4;
        public void Init(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void PlayerEndSectionMovement()
        {
            playerController.fireModule.CloseTheAllBullet();
            playerController.movementModule.canMove = false;
            playerController.fireModule.canFire = false;

            playerController.m16RIfle.SetActive(false);

            playerController.animator.Play(walkAnim.name);

            playerController.transform.DOMove(new Vector3(0, playerController.transform.position.y, playerController.transform.position.z + walkDistance), walkingTime).OnComplete(() =>
            {
                playerController.animator.Play(victoryAnim.name);
                UIManager.Instance.OpenEndGamePanel();
            });
        }
    }
}

