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
    public int targetValue;
}
public class ShootingTargetController : MonoBehaviour
{
    public GunType gunType;
    public List<GunWrapper> gunList = new List<GunWrapper>();
    [SerializeField] Image gunImage;
    [SerializeField] TextMeshProUGUI targetValueText;

    private void Start()
    {
        GetImage();
        GetTargetValue();
    }
    public void GetTargetValue()
    {
        var gunWrapper = GetGunClass(gunType);
        targetValueText.text = gunWrapper.targetValue.ToString();
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
    public void DecreaseTargetValue()
    {
        var gunWrapper = GetGunClass(gunType);
        if (gunWrapper.targetValue == 0)
        {
            //player controller middle sequence module ulasarak silahi falan degismen lazim 
            
        }
        gunWrapper.targetValue -= 1;
        Debug.Log(gunWrapper.targetValue);
        targetValueText.text = gunWrapper.targetValue.ToString();
    }
}
