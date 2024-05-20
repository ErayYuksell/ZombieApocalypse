using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject cureProgressPanel;
    [Space]
    [SerializeField] TextMeshProUGUI gameOverLevelText;
    [SerializeField] TextMeshProUGUI gameOverAntibodyText;
    [Space]
    int gameOverTextValue = 1;
    public Image fillImage;
    public TextMeshProUGUI multipleText;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
        }
    }

    private void Start()
    {
        gameOverLevelText.text = "Level " + gameOverTextValue.ToString();
    }
    public void CloseStartPanel() // button icinde calisiyor 
    {
        Time.timeScale = 1.0f;
        startPanel.SetActive(false);
        cureProgressPanel.SetActive(true);
    }
    /// <summary>
    /// GameOverPanelCode
    /// </summary>
    public void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        cureProgressPanel.SetActive(false);
        gameOverAntibodyText.text = Mathf.Round(PlayerPrefs.GetFloat("SliderValue") * 20).ToString();
        gameOverLevelText.text = SceneManager.GetActiveScene().name;
    }
    public void CloseGameOverPanel()
    {
        gameOverPanel.SetActive(false);
        cureProgressPanel.SetActive(true);
    }

    public void GoToTheNextLevel()
    {
        CloseGameOverPanel();
        EndGameSectionController.Instance.cureProgressModel.ResetSliderValue();
        GameManager.Instance.GoToTheNextLevel();
    }
    /// <summary>
    /// PausePanel
    /// </summary>
    public void OpenPausePanel()
    {
        cureProgressPanel.SetActive(false);
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    public void ClosePausePanel()
    {
        cureProgressPanel.SetActive(true);
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    public void ContinueGame()
    {
        Time.timeScale = 1f;
        ClosePausePanel();
    }
    public void RestartGame()
    {
        GameManager.Instance.LoadSavedLevel();
        pausePanel.SetActive(false);
        startPanel.SetActive(false);
        Time.timeScale = 1f;
    }
  
}
