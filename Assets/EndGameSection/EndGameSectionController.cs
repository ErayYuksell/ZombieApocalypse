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
    [Space]
    public CureProgressModel cureProgressModel;
    public LabRoomListPlayerPrefs labRoomListPlayerPrefs;
    [HideInInspector]
    public Image fillImage;
    [HideInInspector]
    public TextMeshProUGUI multipleText;
    private void Start()
    {
        fillImage = UIManager.Instance.fillImage;
        multipleText = UIManager.Instance.multipleText;

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
            //labRoomListPlayerPrefs.ActivatingRandomLabStuff();
            labRoomListPlayerPrefs.ActivatingSequentialLabStuff();
            var playerController = other.GetComponent<PlayerController>();
            playerController.endSectionModule.PlayerEndSectionMovement();

            cureProgressModel.IncreaseCureBottleAmount();
        }
    }
    
    //[Serializable]
    //public class LabRoomListPlayerPrefs
    //{
    //    EndGameSectionController endGameSectionController;
    //    public List<GameObject> labRoomListFalse = new List<GameObject>();
    //    public List<int> intList = new List<int>();
    //    public void Init(EndGameSectionController endGameSectionController)
    //    {
    //        this.endGameSectionController = endGameSectionController;
    //    }

    //    public void ActivatingRandomLabStuff()
    //    {
    //        int randomNumber = UnityEngine.Random.Range(0, labRoomListFalse.Count);
    //        var obj = labRoomListFalse[randomNumber];
    //        //Debug.Log(randomNumber); 
    //        // burada listeden obj yi cikarmiyorum ayni objleri dondurebilir bir ara bak ????????
    //        SaveActiveSelfTrue(randomNumber);
    //        obj.SetActive(true);
    //    }
    //    public void SaveActiveSelfTrue(int objIndex)
    //    {
    //        intList.Add(objIndex);
    //        // gelen indexi intListe ekliyorum daha sonra intListtte kac eleman varsa kayit edip for icinde count kadar dongu sagliyorum
    //        //obj0 = 5 obj1 = 2 seklinde index degerlerini her engGameSectiona deyince kayit ediyorum

    //        PlayerPrefs.SetInt("intListCount", intList.Count);
    //        for (int i = 0; i < intList.Count; i++)
    //        {

    //            PlayerPrefs.SetInt("obj" + i, intList[i]);
    //        }
    //    }
    //    public void GetActiveSelfTrue()
    //    {
    //        // oyunu kapatip actigimda calisir sadece cunku DontDestroyOnLoad kullaniyorum 
    //        // listCountu her eleman eklerken kayit ediyordum zaten bu liste sayisi dongunun kac kere calisacagini belirleyecek
    //        // obj0 obj1 seklinde kayit ettigim elemanlardaki kayitli indexleri tek tek alarak intListe yeniden ekliyorum cunku oyun yeniden basladiginda
    //        // listeyi bellege kaydetmedigim icin 0 dan baslar 
    //        // kayit ettigim indexleri icine koyduktan sonra ayni indexleri objelerin active ligini acmak icin kullanarak bitiriyorum 
    //        int listCount = PlayerPrefs.GetInt("intListCount", 0);

    //        for (int i = 0; i < listCount; i++)
    //        {
    //            var index = PlayerPrefs.GetInt("obj" + i);
    //            intList.Add(index);
    //            //Debug.Log(index);

    //            var obj = labRoomListFalse[index];
    //            obj.SetActive(true);

    //        }
    //    }
    //}         

    [Serializable] // normalde random atiyordum atilacak olan lab objesini ustteki yorum kodu burada sirayla actiriyorum
    public class LabRoomListPlayerPrefs
    {
        EndGameSectionController endGameSectionController;
        public List<GameObject> labRoomListFalse = new List<GameObject>();
        public List<int> intList = new List<int>();
        private int currentIndex = 0;

        public void Init(EndGameSectionController endGameSectionController)
        {
            this.endGameSectionController = endGameSectionController;
            // Oyunu kapatýp açtýðýmýzda mevcut indeksi geri yükleyelim
            currentIndex = PlayerPrefs.GetInt("currentIndex", 0);
        }

        public void ActivatingSequentialLabStuff()
        {
            if (labRoomListFalse.Count == 0) return;

            if (currentIndex >= labRoomListFalse.Count)
            {
                currentIndex = 0; // Dönüp baþa döneriz
            }

            var obj = labRoomListFalse[currentIndex];
            SaveActiveSelfTrue(currentIndex);
            obj.SetActive(true);

            currentIndex++;
            PlayerPrefs.SetInt("currentIndex", currentIndex); // Güncel indeksi kaydedelim
        }

        public void SaveActiveSelfTrue(int objIndex)
        {
            intList.Add(objIndex);
            PlayerPrefs.SetInt("intListCount", intList.Count);
            for (int i = 0; i < intList.Count; i++)
            {
                PlayerPrefs.SetInt("obj" + i, intList[i]);
            }
        }

        public void GetActiveSelfTrue()
        {
            int listCount = PlayerPrefs.GetInt("intListCount", 0);
            for (int i = 0; i < listCount; i++)
            {
                var index = PlayerPrefs.GetInt("obj" + i);
                intList.Add(index);
                var obj = labRoomListFalse[index];
                obj.SetActive(true);
            }
        }
    }

    [Serializable]
    public class CureProgressModel
    {
        EndGameSectionController endGameSectionController;
        public Renderer liquedRenderer;
        [Space]
        int multipleTextValue = 1;
        float recordedCureSliderValue = 0;
        float recordedSliderValueScale = 0.05f; // kaydedilen slider degeri 4 5 falan oluyor bottle a direk o degeri yollasam fullenir kucultup yollamam lazim
        public float smoothTime = 0.5f; // Yumuþak geçiþ süresi

        public void Init(EndGameSectionController endGameSectionController)
        {
            this.endGameSectionController = endGameSectionController;
        }
        public void WriteMultipleText()
        {
            multipleTextValue++;
            endGameSectionController.multipleText.text = "x" + multipleTextValue.ToString();
        }
        public void CureSliderIncrease(float increaseValue)
        {
            endGameSectionController.StartCoroutine(SmoothIncrease(endGameSectionController.fillImage.fillAmount + increaseValue));
        }

        private IEnumerator SmoothIncrease(float targetValue)
        {
            float elapsedTime = 0;
            float startValue = endGameSectionController.fillImage.fillAmount;
            while (elapsedTime < smoothTime)
            {
                endGameSectionController.fillImage.fillAmount = Mathf.Lerp(startValue, targetValue, elapsedTime / smoothTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            endGameSectionController.fillImage.fillAmount = targetValue;

            recordedCureSliderValue += endGameSectionController.fillImage.fillAmount; // bu deger en son beherglass da artacak olan cure sivisininin miktarini belirleyecek

            if (endGameSectionController.fillImage.fillAmount >= 0.99f)
            {
                endGameSectionController.fillImage.fillAmount = 0;
                WriteMultipleText();
            }
        }
        public void ResetSliderValue()
        {
            recordedCureSliderValue = 0;
            endGameSectionController.fillImage.fillAmount = 0;
            endGameSectionController.multipleText.text = " ";
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
            SetRecordedSliderValue(recordedCureSliderValue);
            recordedCureSliderValue *= recordedSliderValueScale;
            //Debug.Log("slider" + recordedCureSliderValue);
            endGameSectionController.StartCoroutine(SmoothFillIncrease(recordedCureSliderValue));
        }

        private IEnumerator SmoothFillIncrease(float targetValue)
        {
            float elapsedTime = 0;
            float startValue = liquedRenderer.material.GetFloat("_Fill");
            while (elapsedTime < smoothTime)
            {
                float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / smoothTime);
                liquedRenderer.material.SetFloat("_Fill", newValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            liquedRenderer.material.SetFloat("_Fill", targetValue);
            //Debug.Log("SetLiquid" + liquedRenderer.material.GetFloat("_Fill"));
        }
        public void BeginSavedCureAmount()
        {
            var cureSliderValue = GetRecordedSliderValue() * recordedSliderValueScale;
            liquedRenderer.material.SetFloat("_Fill", cureSliderValue);
            //Debug.Log("Getliquid" + liquedRenderer.material.GetFloat("_Fill"));
        }
    }

}
