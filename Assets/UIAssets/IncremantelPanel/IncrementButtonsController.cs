using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncrementButtonsController : MonoBehaviour
{
    UIManager UImanager;
    [SerializeField] GameObject[] incrementButtons;
    bool stopLoop = false;
    private void Start()
    {
        UImanager = UIManager.Instance;
        Debug.Log(UImanager.GetAntibodyCount());

    }
    private void Update()
    {
        CheckAntibodyCount();
    }
    public void CheckAntibodyCount()
    {
        if (UImanager.GetAntibodyCount() <= 0 && !stopLoop)
        {
            foreach (var button in incrementButtons)
            {
                Debug.Log("Button Kapandi");
                button.GetComponent<IncrementsController>().PlaynotEnoughMoneyAnim();
                button.GetComponent<Button>().enabled = false;
            }
            stopLoop = true;
        }
    }
}
