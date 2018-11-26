using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {

    //Player components
    private Animator anim = null; 
    private Rigidbody2D rb = null;

    //Move to enemy
    public float moveSpeed;
    private Vector2 trgPos = Vector2.zero;
    private const float stopDist = 0.1f;//The distance in which the player will stop moving to the enemy

    //References to other entities
    private Enemy focusedEnemy = null;
    private EnemyManager enemyManger = null;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        enemyManger = FindObjectOfType<EnemyManager>();
    }

    private void FixedUpdate()
    {
        //Stop moving when it reaches a point
        if (rb.velocity != Vector2.zero && Utilities.DistanceSquared(transform.position, trgPos) <= stopDist)
        {
            StopMoving();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UnfocusEnemy();
        }

        for (KeyCode currKey = KeyCode.A; currKey < KeyCode.Z + 1; currKey++)
        {
            if (Input.GetKeyDown(currKey))
            {
                TypeLetter(currKey.ToString().ToLower());
            }
        }
    }

    private void FocusEnemy(Enemy enemyToFocus)
    {
        if (enemyToFocus == null)
        {
            return;
        }

        focusedEnemy = enemyToFocus;
        focusedEnemy.text.color = new Color(255, 255, 255, 255);

        if (focusedEnemy.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (focusedEnemy.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void UnfocusEnemy()
    {
        if (focusedEnemy != null)
        {
            focusedEnemy.text.color = new Color(128, 128, 128, 255);
            //focusedEnemy.text.havePropertiesChanged = true;
            focusedEnemy = null;
        }
    }

    private void TypeLetter(string key)
    {
        if (focusedEnemy == null)
        {
            FocusEnemy(enemyManger.GetCloserEnemyWithName(key, transform.position));
        }
        if (focusedEnemy != null && key[0] == focusedEnemy.enemyName[0])
        {
            focusedEnemy.enemyName = focusedEnemy.enemyName.Remove(0, 1);
            focusedEnemy.UpdateName();
            if (focusedEnemy.CheckEnemyDeath())
            {
                anim.SetTrigger("attack");
                StartMoving(focusedEnemy.transform.position);
                focusedEnemy.DestroyEnemy();
                focusedEnemy = null;
            }
        }
    }

    private void StartMoving(Vector2 trgPos)
    {
        this.trgPos = trgPos;
        Vector2 dir = trgPos - new Vector2(transform.position.x, transform.position.y);
        rb.velocity = dir.normalized * moveSpeed;
    }

    private void StopMoving()
    {
        rb.velocity = new Vector2(0.0f, 0.0f);
    }
}
