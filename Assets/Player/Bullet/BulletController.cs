using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    GameObject playerControllerObject;
    PlayerController playerController;
    Transform firePoint;

    private void Start()
    {
        playerControllerObject = GameObject.FindGameObjectWithTag("Player");
        playerController = playerControllerObject.GetComponent<PlayerController>();
        firePoint = playerController.fireModule.firePoint;
    }
    void BulletMovement()
    {
        transform.position += Vector3.forward * bulletSpeed * Time.deltaTime;
    }
    void CalculateBulletDistance()
    {
        var distance = Vector3.Distance(gameObject.transform.position, firePoint.position);
        var range = playerController.fireModule.range;
        if (distance > range)
        {
            gameObject.SetActive(false);
        }
    }
    void Update()
    {
        BulletMovement();
        CalculateBulletDistance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            gameObject.SetActive(false);
            var gateController = other.GetComponent<GateController>();
            gateController.IncreaseGateValue(playerController.fireModule.power);
            gateController.PLayHitAnim();
        }
        if (other.CompareTag("Zombie"))
        {
            gameObject.SetActive(false);
            var zombieController = other.GetComponent<ZombieController>();
            zombieController.ShootZombieCounter(playerController.fireModule.power);
        }
        if (other.CompareTag("BreakableWall"))
        {
            gameObject.SetActive(false);
            var breakableController = other.GetComponent<BreakableWallController>();
            //breakableController.WriteWallText();
            breakableController.PartsMovement(playerController.fireModule.power);
        }
        if (other.CompareTag("ShootingTarget"))
        {
            gameObject.SetActive(false);
            //var middleSequenceController = other.GetComponentInParent<MiddleSequenceController>();
            var shootingTargetController = other.GetComponent<ShootingTargetController>();
            shootingTargetController.DecreaseTargetValue(playerController.fireModule.power);
        }
    }
}
