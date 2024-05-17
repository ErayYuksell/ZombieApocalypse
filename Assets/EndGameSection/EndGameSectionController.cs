using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EndGameSectionController : MonoBehaviour
{
    public static EndGameSectionController Instance;

    public CureProgressModel cureProgressModel;
    public LabRoomListPlayerPrefs labRoomListPlayerPrefs;
    private void Start()
    {
        cureProgressModel.Init(this);
        labRoomListPlayerPrefs.Init(this);
    }
    private void Awake()
    {
        labRoomListPlayerPrefs.GetActiveSelfTrue();
        cureProgressModel.BeginSavedCureAmount();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            labRoomListPlayerPrefs.ActivatingRandomLabStuff();
            var playerController = other.GetComponent<PlayerController>();
            playerController.endSectionModule.PlayerEndSectionMovement();

            cureProgressModel.IncreaseCureBottleAmount();
        }
    }

    [Serializable]
    public class LabRoomListPlayerPrefs
    {
        EndGameSectionController endGameSectionController;
        public List<GameObject> labRoomListFalse = new List<GameObject>();
        public List<int> intList = new List<int>();
        public void Init(EndGameSectionController endGameSectionController)
        {
            this.endGameSectionController = endGameSectionController;
        }

        public void ActivatingRandomLabStuff()
        {
            int randomNumber = UnityEngine.Random.Range(0, labRoomListFalse.Count);
            var obj = labRoomListFalse[randomNumber];
            //Debug.Log(randomNumber); 
            // burada listeden obj yi cikarmiyorum ayni objleri dondurebilir bir ara bak ????????
            SaveActiveSelfTrue(randomNumber);
            obj.SetActive(true);
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
    }

    [Serializable]
    public class CureProgressModel
    {
        EndGameSectionController endGameSectionController;
        [Space]
        public Renderer liquedRenderer;
        [Space]
        public Image slider;
        public TextMeshProUGUI multipleText;
        int multipleTextValue = 1;
        float recordedCureSliderValue = 0;


        public void Init(EndGameSectionController endGameSectionController)
        {
            this.endGameSectionController = endGameSectionController;
        }

        public void WriteMultipleText()
        {
            multipleTextValue++;
            multipleText.text = "x" + multipleTextValue.ToString();
        }
        public void CureSliderIncrease(float increaseValue)
        {
            slider.fillAmount += increaseValue;
            recordedCureSliderValue += slider.fillAmount; // bu deger en son beherglass da artacak olan cure sivisininin miktarini belirleyecek

            if (slider.fillAmount >= 0.99f)
            {
                slider.fillAmount = 0;
                WriteMultipleText();
            }
        }
        public void ResetSliderValue()
        {
            recordedCureSliderValue = 0;
            slider.fillAmount = 0;
            multipleText.text = " ";
        }

        public void SetRecordedSliderValue(float value)
        {
            PlayerPrefs.SetFloat("SliderValue", value);
        }
        public float GetRecordedSliderValue()
        {
            return PlayerPrefs.GetFloat("SliderValue", 0);
        }
        public void IncreaseCureBottleAmount()
        {
            recordedCureSliderValue *= 0.2f;
            //Debug.Log("sliderValue" + recordedCureSliderValue.ToString());
            //Debug.Log("liquid" + liquedRenderer.material.GetFloat("_Fill"));
            liquedRenderer.material.SetFloat("_Fill", recordedCureSliderValue);
            //Debug.Log("Fill liquid" + liquedRenderer.material.GetFloat("_Fill"));
            SetRecordedSliderValue(recordedCureSliderValue);
        }
        public void BeginSavedCureAmount()
        {
            var cureSliderValue = GetRecordedSliderValue();
            liquedRenderer.material.SetFloat("_Fill", cureSliderValue);
        }
    }

}
