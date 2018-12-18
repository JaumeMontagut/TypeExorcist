﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EnemyManager : MonoBehaviour {


    public List<GameObject> enemiesPrefabs;             //List of enemy types
    public List<int> enemiesSpawnRate;                  //List of enemy types spawnrate
    [HideInInspector] public List<Enemy> enemies;       //List of all enemy entities
    private List<string> enemyNamesSmall;                    //List of all enemy entities names
    private List<string> enemyNamesMedium;
    private List<string> enemyNamesBig;

    public Color inactiveColor;
    public Color32 activeColor;
    [HideInInspector]public string inactiveColorStr;

    private Timer spawn_timer = new Timer();
    private float time_btw_spawns = 2.0f;

    private void Start()
    {
        spawn_timer.StarTimer();

        enemies = new List<Enemy>();
        enemyNamesSmall = new List<string>();
        enemyNamesSmall.Add("ulvok");
        enemyNamesSmall.Add("ogima");
        enemyNamesSmall.Add("ragin");
        enemyNamesSmall.Add("agran");
        enemyNamesSmall.Add("eglog");
        enemyNamesSmall.Add("sozer");
        enemyNamesSmall.Add("tornar");

        enemyNamesMedium.Add("dralvoth");
        enemyNamesMedium.Add("tholmith");
        enemyNamesMedium.Add("lucifer");
        enemyNamesMedium.Add("kizzozil");
        enemyNamesMedium.Add("arromak");
        enemyNamesMedium.Add("xorgich");
        enemyNamesMedium.Add("golguner");

        enemyNamesBig.Add("brargarak");
        enemyNamesBig.Add("balgreren");
        enemyNamesBig.Add("xuzgemoth");
        enemyNamesBig.Add("tralgromas");
        enemyNamesBig.Add("jallmokuch");
        enemyNamesBig.Add("brallmomath");
        enemyNamesBig.Add("thirnumith");
        inactiveColorStr = "<color=#" + ColorUtility.ToHtmlStringRGB(inactiveColor) + ">";
    }
    

    void Update()
    {
        print(spawn_timer.GetCurrentTime());

        if (spawn_timer.GetCurrentTime() > time_btw_spawns)
        {
            GenerateRandomEnemy("small");
            
            spawn_timer.StarTimer();
        }

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
                CreateEnemy(position, enemiesPrefabs[index], enemyNamesSmall[Random.Range(0, enemyNamesSmall.Count)]);
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
                CreateEnemy(position, enemiesPrefabs[index], enemyNamesMedium[Random.Range(0, enemyNamesMedium.Count)]);
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

                CreateEnemy(position, enemiesPrefabs[index], enemyNamesBig[Random.Range(0, enemyNamesBig.Count)]);
                break;
            default:
                break;
        }
        
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
