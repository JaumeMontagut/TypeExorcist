using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Obelysk : MonoBehaviour {

    Slider liveSlider;
	Animator animator=null;
	GameObject slider = null;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator>();
        slider = transform.Find("Canvas").transform.Find("Slider").gameObject;
       slider.SetActive(false);
        liveSlider = slider.GetComponent<Slider>();
       
	}
	
	// Update is called once per frame
	void Update () {
        animator.SetFloat("Lives", liveSlider.value);
	}
}
