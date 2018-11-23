using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Enemy focusedEnemy = null;
    private EnemyManager enemyManger = null;

    private void Start()
    {
        enemyManger = FindObjectOfType<EnemyManager>();
    }

    private void Update()
    {
        for (KeyCode currKey = KeyCode.A; currKey < KeyCode.Z + 1; currKey++)
        {
            if (Input.GetKeyDown(currKey))
            {
                TypeLetter(currKey.ToString().ToLower());
            }
        }
    }

    private void TypeLetter(string key)
    {
        if (focusedEnemy == null)
        {
            //Goes through all the list and picks the enemy that is closer to the center of the screen
            focusedEnemy = enemyManger.GetCloserEnemyWithName(key);
        }
        if (focusedEnemy != null)
        {
            if (key[0] == focusedEnemy.enemyName[0])
            {
                focusedEnemy.enemyName = focusedEnemy.enemyName.Remove(0, 1);
                focusedEnemy.UpdateName();
                if (focusedEnemy.EnemyDeath())
                {
                    focusedEnemy = null;
                }
            }
        }
    }
}
