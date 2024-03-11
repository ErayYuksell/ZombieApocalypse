using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
   
    private void Start()
    {
        //StopTheTime();
    }
    void StopTheTime()
    {
        Time.timeScale = 0;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
