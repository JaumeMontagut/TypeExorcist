﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EnemyManager : MonoBehaviour {


    public List<GameObject> enemiesPrefabs;      //List of enemy types
    public List<int> enemiesSpawnRate;           //List of enemy types spawnrate
    [HideInInspector] public List<Enemy> enemies;                 //List of all enemy entities
    private List<string> enemyNames;             //List of all enemy entities names

    public Color inactiveColor;
    public Color32 activeColor;
    [HideInInspector]public string inactiveColorStr;

    private void Start()
    {
        enemies = new List<Enemy>();
        enemyNames = new List<string>();
        enemyNames.Add("holy water");
        enemyNames.Add("bible");
        enemyNames.Add("randomlygeneratedstring a");
        enemyNames.Add("randomlygeneratedstring b");
        inactiveColorStr = "<color=#" + ColorUtility.ToHtmlStringRGB(inactiveColor) + ">";
    }
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 enemyPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            enemyPos.z = 0;
            GenerateRandomEnemy("small");
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


    //Recieves the type of enemy to spawn ANY enemy by default. 
    // "small" spawns only small enemies
    // "mid"   spawns only mid size enemies
    // "big"   spawns only big enemies
    public void GenerateRandomEnemy(string type)
    {
        int index = -1;
        

        Vector3 position = new Vector3(0, 0, 0);


        // camera data------------------------------------------------------------
        //------------------------------------------------------------------------
        int startingPosition = Random.Range(1, 3);
        int cameraHeight = (int)Camera.main.orthographicSize;
        int cameraWidth = (int)(Camera.main.orthographicSize * Camera.main.aspect);
        //------------------------------------------------------------------------
        //------------------------------------------------------------------------

        //Position randomizer
        switch (startingPosition)
        {
            case 1:
                position.x = -cameraWidth;
                position.y = Random.Range(-cameraHeight, cameraHeight); 
                break;
            case 2:
                position.x = cameraWidth;
                position.y = Random.Range(-cameraHeight, cameraHeight);
                break;
            default:
                break;
        }

        //Enemy type randomizer
        switch (type)
        {
            case "small":
                int totalchance = enemiesSpawnRate[0] + enemiesSpawnRate[1];
                int unitvalue = totalchance / 100;
                enemiesSpawnRate[0] = enemiesSpawnRate[0] / unitvalue;
                enemiesSpawnRate[1] = enemiesSpawnRate[1] / unitvalue;
                float randomIndex = Random.Range(0.0f, 100.0f);
                if (randomIndex <= enemiesSpawnRate[0])
                {
                    index = 0;
                }
                else index = 1;
                break;
            case "mid":
                int totalchance2 = enemiesSpawnRate[2] + enemiesSpawnRate[3];
                int unitvalue2 = totalchance2 / 100;
                enemiesSpawnRate[2] = enemiesSpawnRate[2] / unitvalue2;
                enemiesSpawnRate[3] = enemiesSpawnRate[3] / unitvalue2;
                float randomIndex2 = Random.Range(0.0f, 100.0f);
                if (randomIndex2 <= enemiesSpawnRate[2])
                {
                    index = 2;
                }
                else index = 3;
                break;
            case "big":
                int totalchance3 = enemiesSpawnRate[4] + enemiesSpawnRate[5] + enemiesSpawnRate[6];
                int unitvalue3 = totalchance3 / 100;
                enemiesSpawnRate[4] = enemiesSpawnRate[0] / unitvalue3;
                enemiesSpawnRate[1] = enemiesSpawnRate[1] / unitvalue3;
                float randomIndex3 = Random.Range(0.0f, 100.0f);
                if (randomIndex3 <= enemiesSpawnRate[4])
                {
                    index = 4;
                }
                else if (randomIndex3 > enemiesSpawnRate[4] && randomIndex3 < enemiesSpawnRate[6])
                    index = 5;
                else index = 6;
                break;
            default:
                break;
        }
        CreateEnemy(position, enemiesPrefabs[index],enemyNames[Random.Range(0, enemyNames.Count)]);
    }

    //Returns a list of all the enemies whose name starts with the specified letter
    public List<Enemy> GetEnemiesStartingWith (char firstLetter)
    {
        List<Enemy> enemiesStartingWith = new List<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy.GetCurrentLetter() == firstLetter)
            {
                enemiesStartingWith.Add(enemy);
            }
        }
        return enemiesStartingWith;
    }
}
