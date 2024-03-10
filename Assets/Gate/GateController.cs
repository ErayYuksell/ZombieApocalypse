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
    Animator animator;
    [Space]
    [SerializeField] AnimationClip clip;
    [Space]
    [SerializeField] GameObject gateGlass;
    Renderer gateGlassRenderer;
    [SerializeField] Material[] gateMat;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        gateGlassRenderer = gateGlass.GetComponent<Renderer>();
    }
    private void Update()
    {
        ChangeGateTypeAndValue();
        ChangeGateColor();
    }
    void WriteGateText(GateType gateType)
    {
        gateText.text = gateValue.ToString() + " " + gateType.ToString();
    }
    void ChangeGateColor()
    {
        if (gateValue <= 0)
        {
            gateGlassRenderer.material = gateMat[0];
        }
        else
        {
            gateGlassRenderer.material = gateMat[1];
        }
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
