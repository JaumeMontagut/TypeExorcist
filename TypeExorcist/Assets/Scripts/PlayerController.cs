using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {

    private Enemy focusedEnemy = null;
    private EnemyManager enemyManger = null;

    private void Start()
    {
        enemyManger = FindObjectOfType<EnemyManager>();
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
        focusedEnemy = enemyToFocus;
        if (focusedEnemy != null)
        {
            focusedEnemy.text.color = new Color(255, 255, 255, 255);
            //focusedEnemy.text.havePropertiesChanged = true;
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
            FocusEnemy(enemyManger.GetCloserEnemyWithName(key));
        }
        if (focusedEnemy != null && key[0] == focusedEnemy.enemyName[0])
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
