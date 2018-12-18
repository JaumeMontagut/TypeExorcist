using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObelyskManager : MonoBehaviour {


    GameObject HealthBar=null;
    float max = 0.031F;
    float min = -3.31F;
    int life = 100; 
    // Use this for initialization
    void Start () {
        HealthBar = transform.Find("HealthBar").transform.Find("GreenBar").gameObject;
        HealthBar.transform.position = new Vector3(max, HealthBar.transform.position.y, HealthBar.transform.position.z);
        HealthBar.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
    void ModifyLife(int value)
    {

    }
}
