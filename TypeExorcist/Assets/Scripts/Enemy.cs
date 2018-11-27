using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{

    public string enemyName;
    public TextMeshProUGUI text;

    private Vector2 target;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateName();
    }

    public void SetTarget(Vector2 target)
    {
        this.target = target;
    }

    public void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        Vector2 vec = target - new Vector2(transform.position.x, transform.position.y);

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
