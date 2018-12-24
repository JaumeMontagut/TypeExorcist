using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Obelysk : MonoBehaviour {

    Slider liveSlider;
	Animator animator=null;
	GameObject slider = null;
    public GameObject GameOverPanel;
    EnemyManager eManager = null;
	void Start () {
		animator = gameObject.GetComponent<Animator>();
        slider = transform.Find("Canvas").transform.Find("Slider").gameObject;
        slider.SetActive(false);
        liveSlider = slider.GetComponent<Slider>();
        eManager = FindObjectOfType<EnemyManager>();

	}
    private void Update()
    {
       
    }
    public void SubstractLives(float damage)
    {
        slider.SetActive(true);
        liveSlider.value -= damage;
        if (liveSlider.value <= 0)
        {
            GameOverPanel.SetActive(true);
            eManager.spawnEnemies = false;
            liveSlider.value = 0;
            animator.SetBool("Live", false);
           
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("Game");

    }
}
