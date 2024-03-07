using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MovementModule movementModule;
    public GateModule gateModule;
    public FireModule fireModule;
    void Start()
    {
        movementModule.Init(this);
        gateModule.Init(this);
        fireModule.Init(this);

        StartCoroutine(fireModule.SetFire());
    }


    void Update()
    {
        movementModule.PlayerMovement();

    }
    [Serializable]
    public class MovementModule
    {
        PlayerController playerController;
        float xSpeed;
        bool canMove = true;
        public float xLeftValue = -4;
        public float xRightValue = 4;
        [Range(1, 4)] public float playerSpeed = 4;
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
                    playerController.fireModule.rate += playerController.fireModule.rateScale * gateValue;
                    break;
                case GateType.Rate:
                    playerController.fireModule.range += playerController.fireModule.rangeScale * gateValue;
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

        [Range(0.4f, 1)] public float rate = 0.4f;
        [Range(10, 20)] public float range = 15;
        [Range(1, 2)] public float power;
        [Space]
        public float rateScale;
        public float rangeScale;
        public float powerScale;
        [Space]
        public GameObject objectPool;
        public Transform firePoint;
        bool canFire = true;
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


    }


}

