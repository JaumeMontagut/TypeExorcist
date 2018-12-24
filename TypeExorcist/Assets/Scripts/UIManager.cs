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
    private GameObject em;
    public List<char> letters;//The letters that are going to be tested in this level

    private void Start()
    {
        em = FindObjectOfType<EnemyManager>().gameObject;
        em.SetActive(false);
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
        //Phase 1: Introduction
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
        //Phase 2: Show letters
        //TODO: Show letters above and make them turn white when they are written
        if (helpPanel.activeInHierarchy == true)
        {
            for (KeyCode currKey = KeyCode.A; currKey < KeyCode.Z + 1; currKey++)
            {
                if (Input.GetKeyDown(currKey))
                {
                    letters.Remove(currKey.ToString().ToLower()[0]);
                    if (letters.Count == 0)
                    {
                        //Start game
                        inTutorial = false;
                        em.SetActive(true);
                        helpPanel.SetActive(false);
                        gamePanel.SetActive(true);
                    }
                }
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
