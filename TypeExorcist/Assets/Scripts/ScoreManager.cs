using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour {

    private int score = 0;
    private int combo = 1;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            //Update score text
            scoreText.text = score.ToString();
        }
    }

    public int Combo
    {
        get
        {
            return combo;
        }
        set
        {
            combo = value;
            //Update combo text
            comboText.text = comboText.ToString();
        }
    }

}
