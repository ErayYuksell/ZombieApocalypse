using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BreakableWallController : MonoBehaviour
{
    [SerializeField] List<GameObject> wallParts = new List<GameObject>();
    [Space]
    [SerializeField] TextMeshProUGUI wallText;
    int wallTextCount;
    [SerializeField, Range(4, 15)] int wallValue = 4;
    int wallCount;
    private void Start()
    {
        wallTextCount = wallValue;
        wallText.text = wallTextCount.ToString();
    }
    public void PartsMovement()
    {
        if (wallParts.Count <= 0)
        {
            return;
        }
        var part = wallParts[Random.Range(0, wallParts.Count)];
        wallParts.Remove(part);

        part.transform.DOJump(new Vector3(part.transform.position.x + Random.Range(-1, 1), 0, part.transform.position.z + Random.Range(-2, 2)), 2, 1, 1).OnComplete(() =>
        {
            WriteWallText();
            DestroyWall();
        });
    }
    public void DestroyWall()
    {
        wallCount++;
        if (wallCount >= wallValue)
        {
            gameObject.SetActive(false);
        }
    }
    public void WriteWallText()
    {
        wallTextCount--;
        wallText.text = wallTextCount.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.playerDamageModule.BouncedPlayer();
            gameObject.SetActive(false);
        }
    }
}
