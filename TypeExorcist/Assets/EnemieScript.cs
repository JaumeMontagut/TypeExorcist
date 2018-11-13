using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieScript : MonoBehaviour {

	public
	string enemyName= "hola";
    GameObject target;
    private Rigidbody2D rb;
    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("player");
        rb = GetComponent<Rigidbody2D>();
        Vector2 vec = new Vector2(target.transform.position.x-rb.position.x , target.transform.position.y - rb.position.y);
        vec.Normalize();
        rb.velocity = vec;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
};
