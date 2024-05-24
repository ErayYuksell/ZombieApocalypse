using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GunType
{
    AK47, MP5, ShotGun
}
[Serializable]
public class GunWrapper
{
    public GunType gunType;
    public Sprite gunImage;
    public float targetValue;
}
public class ShootingTargetController : MonoBehaviour
{
    public GunType gunType;
    public List<GunWrapper> gunList = new List<GunWrapper>();
    [SerializeField] Image gunImage;
    [SerializeField] TextMeshProUGUI targetValueText;

    PlayerController playerController;
    GameObject playerObject;
    bool canShoot = false;

    bool sequenceCloseDone = false;
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerController = playerObject.GetComponent<PlayerController>();

        GetImage();
        GetTargetValue();
    }
    private void Update()
    {
        CloseMiddleSequence();
    }
    public void SetCanShoot()
    {
        canShoot = true;
    }

    public void GetTargetValue()
    {
        var gunWrapper = GetGunClass(gunType);
        targetValueText.text = gunWrapper.targetValue.ToString("f0");
    }
    public void GetImage()
    {
        var gunWrapper = GetGunClass(gunType);
        gunImage.sprite = gunWrapper.gunImage;
    }
    public GunWrapper GetGunClass(GunType type)
    {
        return gunList.Find((gunWrapper) => gunWrapper.gunType == type);
    }
    public void DecreaseTargetValue(float value)
    {
        if (!canShoot)
        {
            return;
        }

        var gunWrapper = GetGunClass(gunType);

        //if (gunWrapper.targetValue <= 0)
        //{
        //    playerController.middleSequenceModule.ChangeWeapon(gunType);
        //    playerController.middleSequenceModule.MiddleSequenceReverse();
        //    gameObject.GetComponentInParent<MiddleSequenceController>().gameObject.SetActive(false);
        //}
        gunWrapper.targetValue -= value;

        if (gunWrapper.targetValue < 0)
        {
            gunWrapper.targetValue = 0;
        }
        targetValueText.text = gunWrapper.targetValue.ToString("f0");
    }
    public void CloseMiddleSequence() // bir takim sikintilar bulunmakta ??????????
    {
        if (!sequenceCloseDone)
        {
            var gunWrapper = GetGunClass(gunType);

            if (gunWrapper.targetValue <= 0)
            {
                sequenceCloseDone = true;
                playerController.middleSequenceModule.ChangeWeapon(gunType);
                playerController.middleSequenceModule.MiddleSequenceReverse();
                gameObject.GetComponentInParent<MiddleSequenceController>().gameObject.SetActive(false);
            }
        }
    }
}
