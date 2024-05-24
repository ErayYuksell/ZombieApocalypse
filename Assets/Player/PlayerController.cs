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
    public MiddleSequenceModule middleSequenceModule;

    Animator animator;


    void Start()
    {
        movementModule.Init(this);
        gateModule.Init(this);
        fireModule.Init(this);
        playerDamageModule.Init(this);
        endSectionModule.Init(this);
        middleSequenceModule.Init(this);

        animator = GetComponent<Animator>();

        fireModule.BasesWeaponValues();
        StartCoroutine(fireModule.SetFire());
    }


    void Update()
    {
        movementModule.PlayerMovement();
        Editor();
    }

    [Serializable]
    public class MovementModule
    {
        PlayerController playerController;
        public float xSpeed;
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
                xSpeed = 100f;
                touchX = Input.GetTouch(0).deltaPosition.x / Screen.width;
            }
            else if (Input.GetMouseButton(0))
            {
                xSpeed = 20f;
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
                    playerController.fireModule.SetFireRate(playerController.fireModule.rateScale * gateValue);
                    break;
                case GateType.Rate:
                    playerController.fireModule.SetFireRange(playerController.fireModule.rangeScale * gateValue);
                    break;
                case GateType.Power:
                    playerController.fireModule.SetFireStrength(playerController.fireModule.powerScale * gateValue);
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

        [Range(0.1f, 1), SerializeField] float rate = 0.4f;
        [Range(8, 20), SerializeField] float range = 15;
        [Range(0.5f, 2), SerializeField] float strength = 1;
        [Space]
        public float rateScale;
        public float rangeScale;
        public float powerScale;
        [Space]
        public GameObject objectPool;
        public Transform firePoint;
        public Transform firePoint2;
        public bool canFire = true;
        public bool isShotGunActive = false;
        public void Init(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void BasesWeaponValues()
        {
            rate = PlayerPrefs.GetFloat("FireRate", 0.6f);
            range = PlayerPrefs.GetFloat("FireRange", 9);
            strength = PlayerPrefs.GetFloat("FireStrength", 1);
        }
        public void SetFireRate(float value)
        {
            rate += value;
        }
        public float GetFireRate()
        {
            return rate;
        }
        public void SetFireRange(float value)
        {
            range += value;
        }
        public float GetFireRange()
        {
            return range;
        }
        public void SetFireStrength(float value)
        {
            strength += value;
        }
        public float GetFireStrength()
        {
            return strength;
        }
        public IEnumerator SetFire()
        {
            while (canFire)
            {
                var objectPoolScript = objectPool.GetComponent<ObjectPool>();
                var obj = objectPoolScript.GetPoolObjects();
                obj.transform.position = firePoint.position;

                if (isShotGunActive) // shotgun 2 li mermi atsin
                {
                    var obj2 = objectPoolScript.GetPoolObjects();
                    obj2.transform.position = firePoint2.position;
                }

                yield return new WaitForSeconds(rate);
            }
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
                playerController.StartCoroutine(playerController.fireModule.SetFire());
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

            playerController.middleSequenceModule.CloseAllWeapon();

            playerController.animator.Play(walkAnim.name);

            playerController.transform.DOMove(new Vector3(0, playerController.transform.position.y, playerController.transform.position.z + walkDistance), walkingTime).OnComplete(() =>
            {
                playerController.animator.Play(victoryAnim.name);
                UIManager.Instance.OpenGameOverPanel();
            });
        }
    }

    [Serializable]
    public class MiddleSequenceModule
    {
        PlayerController playerController;
        public List<GameObject> weaponList = new List<GameObject>();
        [Space]
        public float AKPower = 1;
        public float MP5Rate = 0.5f;
        public float MP5Power = 0.5f;
        public float ShotGunPower = 1f;
        public float ShotGunRate = 2f;

        public void Init(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void MiddleSequenceAdjustment()
        {
            playerController.movementModule.playerSpeed = 2;
            playerController.movementModule.xSpeed = 20f;
            playerController.movementModule.xLeftValue = -2.30f;
            playerController.movementModule.xRightValue = 2.30f;
        }
        public void MiddleSequenceReverse()
        {
            playerController.movementModule.playerSpeed = 4;
            playerController.movementModule.xSpeed = 250f;
            playerController.movementModule.xLeftValue = -4f;
            playerController.movementModule.xRightValue = 4f;
        }
        public void CloseAllWeapon()
        {
            foreach (var item in weaponList)
            {
                item.SetActive(false);
            }
        }
        public void ChangeWeapon(GunType gunType)
        {
            CloseAllWeapon();

            switch (gunType)
            {
                case GunType.AK47:
                    playerController.fireModule.SetFireStrength(AKPower);
                    weaponList[0].SetActive(true);
                    break;
                case GunType.MP5:
                    playerController.fireModule.SetFireRate(MP5Rate); // carpiydi normalde duzelt
                    playerController.fireModule.SetFireStrength(MP5Power); // buda eksiydi
                    weaponList[1].SetActive(true);
                    break;
                case GunType.ShotGun:
                    playerController.fireModule.SetFireRate(ShotGunRate); // carpi
                    playerController.fireModule.SetFireStrength(ShotGunPower);
                    weaponList[2].SetActive(true);

                    playerController.fireModule.isShotGunActive = true;
                    break;
            }
        }
    }


    public void Editor()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 3;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Time.timeScale = 1;
        }
    }
}

