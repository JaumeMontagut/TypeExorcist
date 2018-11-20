using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemieScript : MonoBehaviour {

	public string enemyName= "hola";
    public TextMeshProUGUI text;

    private GameObject player;
    private Rigidbody2D rb;

    void Start () {
        player = GameObject.FindGameObjectWithTag("player");
        rb = GetComponent<Rigidbody2D>();
        Vector2 vec = new Vector2(player.transform.position.x-rb.position.x , player.transform.position.y - rb.position.y);
        vec.Normalize();
        rb.velocity = vec;
        text.text = "Hello";
    }

	void Update () {
		
	}
};
