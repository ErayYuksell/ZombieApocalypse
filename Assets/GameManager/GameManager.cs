using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // gameManageri her levela koydugun icin sonsuz donguye girip oyun cokuyordu zaten dontDestroyOnLoad ile calisiyor bir kere oyun basladiginda 
    // olusturulan obje her level icin kalicak unutma 

    void StopTheTime()
    {
        Time.timeScale = 0;
    }
    private void Awake()
    {
        StopTheTime();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (GetLevelIndex() > 0)
        {
            LoadSavedLevel();
        }


    }
    // butun fonksiyonlardaki default degerleri 0 yaptim sikinti olursa ilk leveldan devam
    public void SaveLevelIndex(int value = 0)
    {
        PlayerPrefs.SetInt("LevelIndex", value);
    }
    public int GetLevelIndex()
    {
        return PlayerPrefs.GetInt("LevelIndex", 0);
    }
    public void GoToTheNextLevel()
    {
        var levelIndex = GetLevelIndex() + 1;
        SaveLevelIndex(levelIndex);
        LoadScene(levelIndex);
    }
    public void LoadScene(int value = 0)
    {
        SceneManager.LoadScene(value);
    }
    public void LoadSavedLevel()
    {
        int levelIndex = GetLevelIndex();
        LoadScene(levelIndex);
    }

}
