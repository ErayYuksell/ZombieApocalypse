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
    [SerializeField] int gateValue;
    void Start()
    {
        ChangeGateType();
    }

    void WriteGateText(GateType gateType)
    {
        gateText.text = gateValue.ToString() + " " + gateType.ToString();
    }
    void ChangeGateType()
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
            playerController.gateModule.AdjustFireSkills(gateType);
            gameObject.SetActive(false);
        }
    }
}
