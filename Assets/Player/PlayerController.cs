using System;
using System.Collections;
using System.Collections.Generic;
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
        public float playerSpeed = 4;
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
                xSpeed = 350f;
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

        public void AdjustFireSkills(GateType gateType)
        {
            switch (gateType)
            {
                case GateType.Range:
                    playerController.fireModule.rate *= playerController.fireModule.rateScale;
                    break;
                case GateType.Rate:
                    playerController.fireModule.range *= playerController.fireModule.rangeScale;
                    break;
                case GateType.Power:
                    playerController.fireModule.rate *= playerController.fireModule.powerScale;
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

        public float rate;
        public float range;
        public float power;
        [Space]
        public float rateScale;
        public float rangeScale;
        public float powerScale;
        public void Init(PlayerController playerController)
        {
            this.playerController = playerController;
        }


    }
}

