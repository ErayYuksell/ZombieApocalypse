using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject endGamePanel;

    private void Awake()
    {
        //IsTheStartPanelOpen();

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
    public void CloseStartPanel() // button icinde calisiyor 
    {
        Time.timeScale = 1.0f;
        startPanel.SetActive(false);
    }
    public void OpenEndGamePanel()
    {
        endGamePanel.SetActive(true);
    }
    public void CloseEndGamePanel()
    {
        endGamePanel.SetActive(false);
    }
    /// <summary>
    /// EndGamePanelCode
    /// </summary>
    public void GoToTheNextLevel()
    {
        GameManager.Instance.GoToTheNextLevel();
        CloseEndGamePanel();
    }
    public void RestartGame()
    {
        GameManager.Instance.LoadSavedLevel();
        CloseStartPanel();
        CloseEndGamePanel();
    }

}