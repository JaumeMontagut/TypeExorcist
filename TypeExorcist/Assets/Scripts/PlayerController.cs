using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private bool dead = false;
    private GameObject particles = null;
    private GameObject TextGameOver = null;
    //Player components
    private Animator anim = null;
    private Rigidbody2D rb = null;

    //Move to enemy
    public float moveSpeed;
    private List<Vector2> trgPos;
    private const float stopDist = 0.1f;//The distance in which the player will stop moving to the enemy

    //References to other entities
    private List<Enemy> focusedEnemies = null;
    private EnemyManager enemyManger = null;
    private ScoreManager scoreManager = null;

    private void Start()
    {
        focusedEnemies = new List<Enemy>();

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        trgPos = new List<Vector2>();
        enemyManger = FindObjectOfType<EnemyManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        particles = transform.Find("Particle System").gameObject;
        TextGameOver = GameObject.Find("GameOver");
        if (TextGameOver)
        {
            TextGameOver.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        //Stop moving when it reaches a point
        if (rb.velocity != Vector2.zero && Utilities.DistanceSquared(transform.position, trgPos[0]) <= stopDist)
        {
            trgPos.RemoveAt(0);
            if (trgPos.Count == 0)
            {
                //Stop moving
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void Update()
    {
        if (!dead && Time.timeScale != 0)
        {
            KeyCode currKey;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                UnfocusEnemies();
            }

            for (currKey = KeyCode.A; currKey < KeyCode.Z + 1; currKey++)
            {
                if (Input.GetKeyDown(currKey))
                {
                    TypeLetter(currKey.ToString().ToLower()[0]);
                }
            }
        }
    }

    private void FocusEnemy(List<Enemy> enemiesToFocus)
    {
        //Security check
        if (enemiesToFocus.Count == 0)
        {
            return;
        }

        //Add enemy to focused list
        UnfocusEnemies();
        focusedEnemies = enemiesToFocus;

        //Change face direction depending on where the first selected enemy is
        if (focusedEnemies[0].transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (focusedEnemies[0].transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void UnfocusEnemies()
    {
        foreach (Enemy enemy in focusedEnemies)
        {
            enemy.text.color = enemyManger.inactiveColor;
            enemy.text.havePropertiesChanged = true;
        }
        focusedEnemies.Clear();
    }

    private void TypeLetter(char key)
    {
        //If hadn't enemies focused
        if (focusedEnemies.Count == 0)
        {
            FocusEnemy(enemyManger.GetEnemiesStartingWith(key));
            //If it didn't find an enemy after focusing, you made a typing mistake
            if (focusedEnemies.Count == 0)
            {
                Mistake();
                return;
            }
        }
        //If it had enemies focused
        else
        {
            //And you didn't type its letter correctly
            if (!TypeInFocusedEnemies(key))
            {
                Mistake();
                return;
            }
        }
        //If it reaches this point letters have been typed correctly, reduce the letter
        scoreManager.Score += 1 * scoreManager.Combo;
        for (int i = focusedEnemies.Count - 1; i >= 0; --i)
        {
            focusedEnemies[i].CompleteNextLetter();
            if (focusedEnemies[i].CheckDeath())
            {
                scoreManager.Combo++;
                anim.SetTrigger("attack");
                StartMoving(focusedEnemies[i].transform.position);
                focusedEnemies[i].DestroyEnemy();//Change for anim.settrigger die and die at the end of the animation
                focusedEnemies.RemoveAt(i);
            }
        }
    }

    //Returns false if none of the enemies in which it was typed had that letter
    private bool TypeInFocusedEnemies(char letter)
    {
        foreach (Enemy enemy in focusedEnemies)
        {
            if (enemy.GetCurrentLetter() == letter)
            {
                return true;
            }
        }
        return false;
    }

    private void Mistake()
    {
        scoreManager.Combo = 1;
    }

    private void StartMoving(Vector2 trgPos)
    {
        this.trgPos.Add(trgPos);
        Vector2 dir = trgPos - new Vector2(transform.position.x, transform.position.y);
        rb.velocity = dir.normalized * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    //trgPos.RemoveAt(0);
    //if (trgPos.Count != 0)
    //{
    //    rb.velocity =
    //}
    //else
    //{
    //    rb.velocity = Vector2.zero;
    //}
    //private void OnTriggerEnter2D(Collider2D collision) 
    //{
    //    if (!IsMoving() && !death)
    //    {
    //        death = true;
    //        particles.SetActive(true);
    //        TextGameOver.SetActive(true);
    //    }
    //}


    public bool IsMoving()
    {
        return (rb.velocity != Vector2.zero);
    }
}


