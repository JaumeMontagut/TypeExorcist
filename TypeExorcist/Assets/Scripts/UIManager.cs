using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject gamePanel;
    public GameObject helpPanel;
    public GameObject pausePanel;//Panel that lets you go to the main menu

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        GameUpdate();
    }

    private void GameUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gamePanel.SetActive(helpPanel.activeInHierarchy);
            helpPanel.SetActive(!gamePanel.activeInHierarchy);
            if (helpPanel.activeInHierarchy)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePanel.SetActive(pausePanel.activeInHierarchy);
            pausePanel.SetActive(!gamePanel.activeInHierarchy);
            if (pausePanel.activeInHierarchy)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void Resume()
    {
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
