using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] GameObject IncrementalMenu;
    [Space]
    [SerializeField] TextMeshProUGUI gameOverLevelText;
    [SerializeField] TextMeshProUGUI gameOverAntibodyText;
    [Space]
    public Image fillImage; // DontDestroyOnLoad ile kullanirken sikinti cikiyordu o yuzden bu sekilde kullandim
    public TextMeshProUGUI multipleText;
    [Space]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI antibodyCountText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        FirstLevelCheck();
        StartInfo();
    }

    // Start Panel
    public void CloseStartPanel() // button icinde calisiyor 
    {
        Time.timeScale = 1.0f;
        startPanel.SetActive(false);
        cureProgressPanel.SetActive(true);
        IncrementalMenu.SetActive(false);
    }
    public void StartInfo()
    {
        levelText.text = SceneManager.GetActiveScene().name;
        antibodyCountText.text = GetAntibodyCount().ToString();
    }
    public void UpdateAntibodyText()
    {
        antibodyCountText.text = GetAntibodyCount().ToString();
    }
    public void SetAntibodyCount(float value)
    {
        var antibodyCount = GetAntibodyCount();
        antibodyCount += value;

        if (antibodyCount < 0)
        {
            antibodyCount = 0;
        }
        PlayerPrefs.SetFloat("AntibodyCount", antibodyCount);
    }
    public float GetAntibodyCount()
    {
        return PlayerPrefs.GetFloat("AntibodyCount");
    }
    // GameOverPanelCode

    public void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        cureProgressPanel.SetActive(false);

        //var antibody = Mathf.Round(PlayerPrefs.GetFloat("SliderValue") * 20);
        var antibody = PlayerPrefs.GetFloat("SliderValue");
        SetAntibodyCount(antibody);

        gameOverAntibodyText.text = antibody.ToString();
        gameOverLevelText.text = SceneManager.GetActiveScene().name;
    }
    public void GoToTheNextLevel()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f;
        EndGameSectionController.Instance.cureProgressModel.ResetSliderValue();
        GameManager.Instance.GoToTheNextLevel();
    }

    // PausePanel

    public void OpenPausePanel()
    {
        cureProgressPanel.SetActive(false);
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
        cureProgressPanel.SetActive(true);
        Time.timeScale = 1f;
    }
    public void ContinueGame()
    {
        ClosePausePanel();
        Time.timeScale = 1f;
    }
    public void RestartGame()
    {
        GameManager.Instance.LoadSavedLevel();
    }
    // Incremental Menu

    public void FirstLevelCheck()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            IncrementalMenu.SetActive(false);
        }
    }
}
