using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CureProgressController : MonoBehaviour
{
    [SerializeField] Image slider;
    [SerializeField] TextMeshProUGUI multipleText;
    int multipleTextValue = 1;
    float recordedCureSliderValue = 0;
    public void WriteMultipleText()
    {
        multipleTextValue++;
        multipleText.text = "x" + multipleTextValue.ToString();
    }
    public void CureSliderIncrease(float increaseValue)
    {
        slider.fillAmount += increaseValue;
        if (slider.fillAmount >= 0.99f)
        {
            recordedCureSliderValue += slider.fillAmount; // bu deger en son beherglass da artacak olan cure sivisininin miktarini belirleyecek
            slider.fillAmount = 0;
            WriteMultipleText();
        }
    }

}
