using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{

    [HideInInspector] public string enemyName;

    private TextMeshProUGUI text;
    private Vector2 target;
    private Animator anim;
    private Rigidbody2D rb;
    private EnemyManager eM;
    private int completedLetters = 0;//The number of completed letters

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        rb = GetComponent<Rigidbody2D>();
        eM = FindObjectOfType<EnemyManager>();
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player hits the enemy while moving, it kills it
        if (collision.gameObject.CompareTag("Player") && collision.GetComponent<PlayerController>().IsMoving())
        {
            anim.SetTrigger("death");
        }
    }

    public void CompleteNextLetter()
    {
        completedLetters++;
        UpdateName();
    }

    public void ResetLetter()
    {
        completedLetters = 0;
        UpdateName();
    }

    //Called each time you type a letter and when some other effects remove letters from the string
    public bool CheckDeath()
    {
        if (completedLetters == enemyName.Length)
        {
            anim.SetTrigger("death");
            return true;
        }
        return false;
    }

    //Called at the end of the animation
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public char GetCurrentLetter()
    {
        return enemyName[completedLetters];
    }

    private void UpdateName()
    {
        string newText = enemyName;
        newText = newText.Insert(completedLetters, eM.inactiveColorStr);

        text.text = newText;
    }

};
