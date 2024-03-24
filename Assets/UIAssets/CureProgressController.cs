using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CureProgressController : MonoBehaviour
{
    [SerializeField] Image slider;
    

    public void CureSliderIncrease(float increaseValue)
    {
        slider.fillAmount += increaseValue;
    }

}
