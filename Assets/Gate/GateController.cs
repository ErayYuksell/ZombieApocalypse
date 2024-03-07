using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GateType { Range, Rate, Power, };
public class GateController : MonoBehaviour
{
    public GateType gateType;
    [SerializeField] TextMeshProUGUI gateText;
    [SerializeField] float gateValue;
    [Space]
    Animator animator;
    [SerializeField] AnimationClip clip;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        ChangeGateTypeAndValue();
    }
    void WriteGateText(GateType gateType)
    {
        gateText.text = gateValue.ToString() + " " + gateType.ToString();
    }
    void ChangeGateTypeAndValue()
    {
        switch (gateType)
        {
            case GateType.Range:
                WriteGateText(gateType);
                break;
            case GateType.Rate:
                WriteGateText(gateType);
                break;
            case GateType.Power:
                WriteGateText(gateType);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.gateModule.AdjustFireSkills(gateType, gateValue);
            gameObject.SetActive(false);
        }
    }

    public void IncreaseGateValue()
    {
        gateValue++;
    }
    public void PLayHitAnim()
    {
        animator.Play(clip.name);
    }
}
