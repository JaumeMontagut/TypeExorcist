using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject gamePanel;
    public GameObject helpPanel;
    public GameObject tutorialPanel;

    //Tutorial
    public Text tutorialText;
    public Text continueText;

    public string[] tutorialLines;
    private int currLine = 0;

    private bool inTutorial = true;

    private void Start()
    {
        ShowLine();
        //Time.timeScale = 0;
    }

    private void Update()
    {
        if (inTutorial)
        {
            TutorialUpdate();
        }
        else
        {
            GameUpdate();
        }
    }

    private void TutorialUpdate()
    {
        if (Input.anyKeyDown)
        {
            if (currLine + 1 < tutorialLines.Length)
            {
                currLine++;
                ShowLine();
                if (currLine == tutorialLines.Length - 1)
                {
                    continueText.text = "Press SPACE";
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                tutorialPanel.SetActive(false);
                helpPanel.SetActive(true);
            }
        }
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
    }

    private void ShowLine()
    {
        tutorialText.text = tutorialLines[currLine];
    }

}
