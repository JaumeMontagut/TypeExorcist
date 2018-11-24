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
        player = GameObject.FindGameObjectWithTag("player");
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

    public bool EnemyDeath()
    {
        if (enemyName.Length == 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public void UpdateName()
    {
        text.text = enemyName;
    }

};
