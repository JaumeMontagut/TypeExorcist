using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Obelysk : MonoBehaviour {

    Slider liveSlider;
	Animator animator=null;
	GameObject slider = null;
    
	void Start () {
		animator = gameObject.GetComponent<Animator>();
        slider = transform.Find("Canvas").transform.Find("Slider").gameObject;
        liveSlider = slider.GetComponent<Slider>();
       
	}
	
    public void SubstractLives(float damage)
    {
        liveSlider.value -= damage;
        if (liveSlider.value <= 0)
        {
            liveSlider.value = 0;
            animator.SetBool("Live", false);
        }
    }
}
