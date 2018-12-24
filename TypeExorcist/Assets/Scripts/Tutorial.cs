using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public Text tutorialText;
    public Text continueText;

    public string [] tutorialLines;
    private int currLine = 0;

    private void Start()
    {
        ShowLine();
    }

    private void Update () {
        if (Input.anyKeyDown)
        {
            currLine++;
            if (currLine == tutorialLines.Length - 1)
            {
                continueText.text = "Press SPACE";
            }
            ShowLine();
        }
	}

    private void ShowLine()
    {
        tutorialText.text = tutorialLines[currLine];
    }
}
