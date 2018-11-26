using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{

    public string enemyName;
    public TextMeshProUGUI text;

    private GameObject player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        UpdateName();
    }

    public void Update()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        Vector2 vec = new Vector2(player.transform.position.x - rb.position.x, player.transform.position.y - rb.position.y);
        vec.Normalize();
        rb.velocity = vec;
    }

    public bool CheckEnemyDeath()
    {
        return (enemyName.Length == 0);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void UpdateName()
    {
        text.text = enemyName;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player hits the enemy while moving, it kills it
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            DestroyEnemy();
        }
    }

};
