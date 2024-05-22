using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ButtonTypes
{
    Strength,
    Rate,
    Range
}
public class IncrementsController : MonoBehaviour
{
    public ButtonTypes buttonType;

    public void HandleButtonClick(ButtonTypes buttonType)
    {
        switch (buttonType)
        {
            case ButtonTypes.Strength:
                HandleStrengthButton();
                break;
            case ButtonTypes.Rate:
                HandleRateButton();
                break;
            case ButtonTypes.Range:
                HandleRangeButton();
                break;
            default:
                Debug.LogError("Unknown ButtonType: " + buttonType);
                break;
        }
    }

    public void OnClickButton()
    {
        HandleButtonClick(buttonType);
    }
    private void HandleStrengthButton()
    {
        Debug.Log("Strength button clicked");

    }

    private void HandleRateButton()
    {
        Debug.Log("Rate button clicked");

    }

    private void HandleRangeButton()
    {
        Debug.Log("Range button clicked");

    }
}

