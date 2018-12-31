using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{

    public string enemyName;
    public TextMeshProUGUI text;

    

    private SpriteRenderer spriteRenderer;
    private float zSpriteLocalScale;

    private Vector2 target;
    private Animator anim;
    private Rigidbody2D rb;
    private EnemyManager eM;
    private int completedLetters = 0;//The number of completed letters
    private GameObject obelisk;
    private bool move = true;
    private PlayerController player;
    
    void Start()
    {
        spriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        eM = FindObjectOfType<EnemyManager>();
        UpdateName();
        anim = transform.Find("Sprite").gameObject.GetComponent<Animator>();
        obelisk = GameObject.FindGameObjectWithTag("Obelysk");

        if (transform.position.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

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
        if (move)
        {
            //TODO: enemies don't change path, so there is no need to evaluate their path every frame
            Vector2 vec = target - new Vector2(transform.position.x, transform.position.y);
            if ((int)vec.magnitude == 0)
            {
                if (anim.GetBool("Attack") == false)
                {
                    anim.SetTrigger("Attack");
                    obelisk.GetComponent<Obelysk>().SubstractLives(0.1F);
                }
            }

            vec.Normalize();
            rb.velocity = vec;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        //If the player hits the enemy while moving, it kills it
        if (collision.gameObject.CompareTag("Player") && collision.GetComponent<PlayerController>().IsMoving())
        {
            collision.GetComponent<AudioSource>().Play();
            DestroyEnemy();
        }
    }

    public void CompleteNextLetter()
    {
        completedLetters++;
        UpdateName();
    }

    //Called each time you type a letter and when some other effects remove letters from the string
    public bool CheckDeath()
    {
        return (completedLetters >= enemyName.Length - 1);
    }

    public void StopMoving()
    {
        move = false;
        rb.velocity = Vector2.zero;
    }

    //Called at the end of the animation
    public void DestroyEnemy()
    {
        eM.enemies.Remove(this);
        player.RemoveFromFocused(this);
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
