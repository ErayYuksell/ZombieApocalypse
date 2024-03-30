using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameSectionController : MonoBehaviour
{
    public static EndGameSectionController Instance;

    [SerializeField] List<GameObject> labRoomListFalse = new List<GameObject>();
    [SerializeField] List<int> intList = new List<int>();
    //[SerializeField] List<GameObject> labRoomListTrue = new List<GameObject>();
    [SerializeField] GameObject cureBottle;
    Renderer cureBottleRenderer;
    Shader liquidShadder;
    private void Awake()
    {
        GetActiveSelfTrue();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //nesnemin levellar arasi yok olmasini engelleyerek oyunu kapatana kadar liste icinde tutarak objeleri aciyorum

        }
        else
        {
            Destroy(this);
        }
    }

    void ActivatingRandomLabStuff()
    {
        int randomNumber = Random.Range(0, labRoomListFalse.Count);
        var obj = labRoomListFalse[randomNumber];
        //Debug.Log(randomNumber); 
        // burada listeden obj yi cikarmiyorum ayni objleri dondurebilir bir ara bak ????????
        SaveActiveSelfTrue(randomNumber);
        obj.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivatingRandomLabStuff();
            var playerController = other.GetComponent<PlayerController>();
            playerController.endSectionModule.PlayerEndSectionMovement();
            IncreaseCureBottleAmount();
        }
    }
    public void SaveActiveSelfTrue(int objIndex)
    {
        intList.Add(objIndex);
        // gelen indexi intListe ekliyorum daha sonra intListtte kac eleman varsa kayit edip for icinde count kadar dongu sagliyorum
        //obj0 = 5 obj1 = 2 seklinde index degerlerini her engGameSectiona deyince kayit ediyorum

        PlayerPrefs.SetInt("intListCount", intList.Count);
        for (int i = 0; i < intList.Count; i++)
        {

            PlayerPrefs.SetInt("obj" + i, intList[i]);
        }
    }
    public void GetActiveSelfTrue()
    {
        // oyunu kapatip actigimda calisir sadece cunku DontDestroyOnLoad kullaniyorum 
        // listCountu her eleman eklerken kayit ediyordum zaten bu liste sayisi dongunun kac kere calisacagini belirleyecek
        // obj0 obj1 seklinde kayit ettigim elemanlardaki kayitli indexleri tek tek alarak intListe yeniden ekliyorum cunku oyun yeniden basladiginda
        // listeyi bellege kaydetmedigim icin 0 dan baslar 
        // kayit ettigim indexleri icine koyduktan sonra ayni indexleri objelerin active ligini acmak icin kullanarak bitiriyorum 
        int listCount = PlayerPrefs.GetInt("intListCount", 0);

        for (int i = 0; i < listCount; i++)
        {
            var index = PlayerPrefs.GetInt("obj" + i);
            intList.Add(index);
            //Debug.Log(index);

            var obj = labRoomListFalse[index];
            obj.SetActive(true);

        }
    }


    public void IncreaseCureBottleAmount()
    {
        cureBottleRenderer = cureBottle.GetComponentInChildren<Renderer>();
        liquidShadder = cureBottleRenderer.material.shader;
        
       
       

    }
}
