using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject gamePanel;
    public GameObject helpPanel;

    //Tutorial
    public Text tutorialText;
    public Text continueText;

    public string[] tutorialLines;
    private int currLine = 0;

    private void Start()
    {
        ShowLine();
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (currLine + 1 < tutorialLines.Length)
            {
                currLine++;
            }
            if (currLine == tutorialLines.Length - 1)
            {
                continueText.text = "Press SPACE";
            }
            ShowLine();
        }

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
