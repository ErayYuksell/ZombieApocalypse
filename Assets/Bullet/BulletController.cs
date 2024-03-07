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
            gateController.IncreaseGateValue();
            gateController.PLayHitAnim();
        }
    }
}
