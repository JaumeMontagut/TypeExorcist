﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour {

    enum enemyIndex:int
    {
        TRIANGLE,
        SQUARE,
        CIRCLE,
		DEMONIC_ARCHANGEL
    }

    private List<string> enemyNames;
    public List<GameObject> enemiesPrefabs;
    private List<Enemy> enemies;

    public Color inactiveColor;
    public Color32 activeColor;
    private string inactiveColorString;

    private void Start()
    {
        enemies = new List<Enemy>();
        enemyNames = new List<string>();
        enemyNames.Add("holy water");
        enemyNames.Add("bible");
        enemyNames.Add("randomlygeneratedstring a");
        enemyNames.Add("randomlygeneratedstring b");
        inactiveColorString = "<color=#" + ColorUtility.ToHtmlStringRGB(inactiveColor) + ">";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 enemyPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            enemyPos.z = 0;
            GenerateRandomEnemy();
        }
    }

    void CreateEnemy(Vector3 enemyPos,GameObject enemyPrefab,string enemyName)
    {
        Enemy newEnemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity).GetComponent<Enemy>();
        newEnemy.SetTarget(Vector2.zero);//Move to the center
        newEnemy.enemyName = enemyName;
        newEnemy.GetComponentInChildren<TextMeshProUGUI>().color = activeColor;
        enemies.Add(newEnemy);
    }

    //Returns the closest enemy to the center of the screen that starts with the specified letter
    public Enemy GetCloserEnemyWithName (string firstLetter, Vector2 point)
    {
        Enemy closerEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            if (enemy.enemyName.StartsWith(firstLetter))
            {
                //The position that is closer to the center wins
                if (closerEnemy == null || Utilities.DistanceSquared(enemy.transform.position, point) < Utilities.DistanceSquared(closerEnemy.transform.position, point))
                {
                    closerEnemy = enemy;
                }
            }
        }
        return closerEnemy;
    }
    public void GenerateRandomEnemy()
    {

        int index = Random.Range(0, 100);
        int enemyIndexType = -1;
        Vector3 position = new Vector3(0, 0, 0);

        int startingPosition = Random.Range(1, 5);

        switch (startingPosition)
        {
            case 1:
                position.x = -9;
                position.y = Random.Range(-5, 6); 
                break;
            case 2:
                position.x = 9;
                position.y = Random.Range(-5, 6);
                break;
            case 3:
                position.y = -5;
                position.x = Random.Range(-9, 10);
                break;
            case 4:
                position.y = 5;
                position.x = Random.Range(-9, 10);
                break;

            default:
                break;
        }
        
        if (index<25)
        {
            enemyIndexType = (int)enemyIndex.TRIANGLE;
        }
        if (index >= 25 && index< 50)
        {
            enemyIndexType = (int)enemyIndex.SQUARE;
        }
        if (index >= 50 && index <= 75)
        {
            enemyIndexType = (int)enemyIndex.CIRCLE;
        }
		if (index >= 75 && index <= 100)
		{
			enemyIndexType = (int)enemyIndex.DEMONIC_ARCHANGEL;
		}

        CreateEnemy(position, enemiesPrefabs[enemyIndexType], inactiveColorString + enemyNames[Random.Range(0, 4)]);
    }
}
