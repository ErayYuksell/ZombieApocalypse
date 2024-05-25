using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    float increaseStrength = 1;
    float increaseRange = 9;
    float increaseRate = 0.6f;

    PlayerController playerController;

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI costText;

    int levelValue = 1;
    int costValue = 1;

    [SerializeField] AnimationClip notEnoughMoneyAnim;
    [SerializeField] AnimationClip upgradeEffectAnim;

    UIManager UImanager;
    Animator animator;

    bool checkCost = false;
    //bool checkZero = false;
    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

        levelText.text = "Level " + levelValue.ToString();
        costText.text = costValue.ToString();

        UImanager = UIManager.Instance;

        GetButtonText();
        levelText.text = "Level " + levelValue.ToString();
        costText.text = costValue.ToString();
    }
    private void Update()
    {
        CheckButtonCost();
    }
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
        if (UImanager.GetAntibodyCount() >= costValue)
        {
            animator.Play(upgradeEffectAnim.name);
            UImanager.SetAntibodyCount(-costValue);

            //Debug.Log("Strength button clicked");
            var strength = PlayerPrefs.GetFloat("FireStrength", 1);
            strength += increaseStrength;
            PlayerPrefs.SetFloat("FireStrength", strength);
            playerController.fireModule.BasesWeaponValues();

            WriteButtonText();
        }
    }

    private void HandleRateButton()
    {
        if (UImanager.GetAntibodyCount() >= costValue)
        {
            animator.Play(upgradeEffectAnim.name);
            UImanager.SetAntibodyCount(-costValue);

            //Debug.Log("Rate button clicked");
            var rate = PlayerPrefs.GetFloat("FireRate", 0.6f);
            rate += increaseRate;
            PlayerPrefs.SetFloat("FireRate", rate);
            playerController.fireModule.BasesWeaponValues();

            WriteButtonText();
        }
    }

    private void HandleRangeButton()
    {
        if (UImanager.GetAntibodyCount() >= costValue)
        {
            animator.Play(upgradeEffectAnim.name);
            UImanager.SetAntibodyCount(-costValue);

            //Debug.Log("Range button clicked");
            var range = PlayerPrefs.GetFloat("FireRange", 9);
            range += increaseRange;
            PlayerPrefs.SetFloat("FireRange", range);
            playerController.fireModule.BasesWeaponValues();

            WriteButtonText();
        }
    }

    public void WriteButtonText()
    {
        levelValue++;
        costValue++;
        SetButtonText();
        levelText.text = "Level " + levelValue.ToString();
        costText.text = costValue.ToString();
        UImanager.UpdateAntibodyText();
    }

    // her button texti icin farkli kayit olusturmam lazim
    private string GetLevelKey()
    {
        return "LevelValue" + buttonType.ToString();
    }

    private string GetCostKey()
    {
        return "CostValue" + buttonType.ToString();
    }

    public void SetButtonText()
    {
        PlayerPrefs.SetInt(GetLevelKey(), levelValue);
        PlayerPrefs.SetInt(GetCostKey(), costValue);
    }

    public void GetButtonText()
    {
        levelValue = PlayerPrefs.GetInt(GetLevelKey(), 1);
        costValue = PlayerPrefs.GetInt(GetCostKey(), 1);
    }

    // button cost kontrol

    public void CheckButtonCost() // bu fonksiyonda antibody icin hem cost value hemde 0 olma durumunu kontrol ediyorum 
    {
        //if (UImanager.GetAntibodyCount() <= 0 && !checkZero) // gerek yok gibi zaten costValue en az 1 olacagi icin toplam miktar 0 sa hicbiri acilmaz 
        //{
        //    Debug.Log("Tuh amk para 0");
        //    gameObject.GetComponent<Button>().enabled = false;
        //    animator.Play(notEnoughMoneyAnim.name);
        //    checkZero = true;
        //}
        if (UImanager.GetAntibodyCount() < costValue && !checkCost)
        {
            Debug.Log("not enough antibody");
            gameObject.GetComponent<Button>().enabled = false;
            animator.Play(notEnoughMoneyAnim.name);
            checkCost = true;
        }
    }

    public void PlaynotEnoughMoneyAnim()
    {
        animator.Play(notEnoughMoneyAnim.name);
    }

}

