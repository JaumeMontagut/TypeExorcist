using System.Collections.Generic;
using UnityEngine;

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
    private Vector2 trgPos = Vector2.zero;
    private const float stopDist = 0.1f;//The distance in which the player will stop moving to the enemy

    //References to other entities
    private List<Enemy> focusedEnemies = null;
    private EnemyManager eM = null;
    private ScoreManager scoreManager = null;

    private void Start()
    {
        focusedEnemies = new List<Enemy>();

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        eM = FindObjectOfType<EnemyManager>();
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
        if (rb.velocity != Vector2.zero && Utilities.DistanceSquared(transform.position, trgPos) <= stopDist)
        {
            StopMoving();
        }
    }

    private void Update()
    {
        if (dead)
        {
            return;
        }

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
        currKey = KeyCode.Space;
        if (Input.GetKeyDown(currKey))
        {
            TypeLetter(' ');
        }
    }

    private void UnfocusEnemies()
    {
        foreach (Enemy enemy in focusedEnemies)
        {
            enemy.Reset();
        }
        focusedEnemies.Clear();
    }

    private void TypeLetter(char key)
    {
        bool typeCorrect;
        if (focusedEnemies.Count == 0)
        {
            typeCorrect = TypeNewEnemies(key);
        }
        else
        {
            typeCorrect = TypeFocusedEnemies(key);
        }
        //See if the player typed the correct letter
        if (typeCorrect)
        {
            Correct();
        }
        else
        {
            Mistake();
        }
    }

    private bool TypeNewEnemies(char key)
    {
        for (int i = 0; i < eM.enemies.Count; ++i)
        {
            if (eM.enemies[i].alive && eM.enemies[i].GetFirstLetter() == key)
            {
                eM.enemies[i].CompleteNextLetter();
                ChangeFacingDir(eM.enemies[i].transform.position);
                focusedEnemies.Add(eM.enemies[i]);
            }
        }
        return (focusedEnemies.Count > 0);
    }

    private bool TypeFocusedEnemies(char key)
    {
        bool someCorrect = false;
        for (int i = focusedEnemies.Count - 1; i >= 0; --i)
        {
            if (focusedEnemies[i].GetCurrentLetter() == key)
            {
                focusedEnemies[i].CompleteNextLetter();
                //Checks if it kills the enemy
                if (!focusedEnemies[i].alive)
                {
                    scoreManager.Combo++;
                    anim.SetTrigger("attack");
                    StartMoving(focusedEnemies[i].transform.position);
                    focusedEnemies.RemoveAt(i);
                }
                someCorrect = true;
            }
            else
            {
                focusedEnemies[i].Reset();
                focusedEnemies.RemoveAt(i);
            }
        }
        return someCorrect;
    }

    private void ChangeFacingDir(Vector3 target)
    {
        if (target.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (target.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void Correct()
    {
        scoreManager.Score += 1 * scoreManager.Combo;
    }

    private void Mistake()
    {
        scoreManager.Combo = 1;
    }

    private void StartMoving(Vector2 trgPos)
    {
        this.trgPos = trgPos;
        Vector2 dir = trgPos - new Vector2(transform.position.x, transform.position.y);
        rb.velocity = dir.normalized * moveSpeed;
    }

    private void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

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
