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
            DontDestroyOnLoad(gameObject);
            //nesnemin levellar arasi yok olmasini engelleyerek oyunu kapatana kadar liste icinde tutarak objeleri aciyorum

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
        //[SerializeField] GameObject cureBottle;
        //Renderer cureBottleRenderer;
        //Shader liquidShadder;
        public Image slider;
        public TextMeshProUGUI multipleText;
        int multipleTextValue = 1;
        float recordedCureSliderValue = 0;
        public GameObject cureBottle;

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
            // kaydedilen degeri endGameSectiona aktarmakta sikinti cekiyorum
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
            //cureBottleRenderer = cureBottle.GetComponentInChildren<Renderer>();
            //liquidShadder = cureBottleRenderer.material.shader;
            var scaleValue = recordedCureSliderValue / 10;
            SetRecordedSliderValue((float)scaleValue);
            //cureBottle.transform.localScale = new Vector3(cureBottle.transform.localScale.x, cureBottle.transform.localScale.y + scaleValue, cureBottle.transform.localScale.z);
            cureBottle.transform.DOScaleY(cureBottle.transform.localScale.y + scaleValue, 1).OnComplete(() =>
            {
                ResetSliderValue();
            });
        }
        public void BeginSavedCureAmount()
        {
            var scaleValue = GetRecordedSliderValue();
            cureBottle.transform.localScale = new Vector3(cureBottle.transform.localScale.x, cureBottle.transform.localScale.y + scaleValue, cureBottle.transform.localScale.z);
        }
    }

}
