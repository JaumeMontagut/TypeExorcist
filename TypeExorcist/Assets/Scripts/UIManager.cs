using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject gamePanel;
    public GameObject helpPanel;

    private void Update()
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

}
