using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameSectionController : MonoBehaviour
{
    [SerializeField] List<GameObject> labRoomListFalse = new List<GameObject>();
    [SerializeField] List<GameObject> labRoomListTrue = new List<GameObject>();
    [SerializeField] List<PlayerPrefs> labRoomSaveList = new List<PlayerPrefs>();

    private void Start()
    {
        GetPlayerPrefsList();
    }
    void ActivatingRandomLabStuff()
    {
        int randomNumber = Random.Range(0, labRoomListFalse.Count);
        var obj = labRoomListFalse[randomNumber];
        SavePlayerPrefsList(randomNumber);
        labRoomListFalse.Remove(obj);
        labRoomListTrue.Add(obj);
        obj.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivatingRandomLabStuff();
            var playerController = other.GetComponent<PlayerController>();
            playerController.endSectionModule.PlayerEndSectionMovement();
        }
    }
    void SavePlayerPrefsList(int objIndex)
    {
        for (int i = 0; i < labRoomListTrue.Count; i++)
        {
            PlayerPrefs.SetInt("obj" + i, objIndex);
        }
    }
    public void GetPlayerPrefsList()
    {
        for (int i = 0; i < labRoomListTrue.Count; i++)
        {
            int objIndex = PlayerPrefs.GetInt("obj" + i);
            labRoomListTrue[objIndex].SetActive(true);
        }
    }
}
