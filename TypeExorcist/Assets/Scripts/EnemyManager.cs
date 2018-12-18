using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EnemyManager : MonoBehaviour {

    public enum EnemyType  { none = -1, small = 0, medium = 1, big = 2}

    [HideInInspector] public List<Enemy> enemies;       //List of all active enemies  
    public List<GameObject> smallEnemiesPrefabs;        //List of small enemy prefabs 
    public List<GameObject> mediumEnemiesPrefabs;       //List of medium enemy prefabs 
    public List<GameObject> bigEnemiesPrefabs;          //List of big enemy prefabs 

    public float smallEnemieRate = 0.0f;
    public float mediumEnemieRate = 0.0f;
    public float bigEnemieRate = 0.0f;

    private List<string> enemyNamesSmall;              
    private List<string> enemyNamesMedium;
    private List<string> enemyNamesBig;

    public Color inactiveColor;
    public Color32 activeColor;
    [HideInInspector]public string inactiveColorStr;


    // Spawn logic ------------------------

    private Timer spawn_timer = new Timer();
    private float time_btw_spawns = 0.0f;
    public float spawn_time_big = 0.0f;
    public float spawn_time_medium = 0.0f;
    public float spawn_time_small = 0.0f;

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

        enemyNamesMedium = new List<string>();
        enemyNamesMedium.Add("dralvoth");
        enemyNamesMedium.Add("tholmith");
        enemyNamesMedium.Add("lucifer");
        enemyNamesMedium.Add("kizzozil");
        enemyNamesMedium.Add("arromak");
        enemyNamesMedium.Add("xorgich");
        enemyNamesMedium.Add("golguner");

        enemyNamesBig = new List<string>();
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
            GenerateRandomEnemy();
            spawn_timer.StarTimer();
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 enemyPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    enemyPos.z = 0;
        //    GenerateRandomEnemy("small");
        //}
    }

    void CreateEnemy(Vector3 enemyPos,GameObject enemyPrefab,string enemyName)
    {
        Enemy newEnemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity).GetComponent<Enemy>();
        newEnemy.SetTarget(Vector2.zero);//Move to the center
        newEnemy.enemyName = enemyName;
        newEnemy.GetComponentInChildren<TextMeshProUGUI>().color = activeColor;
        enemies.Add(newEnemy);
    }

    public void GenerateRandomEnemy()
    {
        Vector3 position = new Vector3(0, 0, 0);

        // camera data------------------------------------------------------------
        //------------------------------------------------------------------------
        int startingPosition = Random.Range(1, 3);
        int cameraHeight = (int)Camera.main.orthographicSize;
        int cameraWidth = (int)(Camera.main.orthographicSize * Camera.main.aspect);
        //------------------------------------------------------------------------
        //------------------------------------------------------------------------

        // Position randomizer ---------------------------------------------------
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
        }

        // Select enemy type ------------------------------------------------
        EnemyType type = EnemyType.none;

        float probability_range = smallEnemieRate + mediumEnemieRate + bigEnemieRate;
        float random_num = Random.Range(0, probability_range);

        if (random_num >= 0 && random_num < smallEnemieRate)
        { type = EnemyType.small; }
        else if (random_num > smallEnemieRate && random_num <= smallEnemieRate + mediumEnemieRate)
        { type = EnemyType.medium; }
        else if(random_num >  smallEnemieRate + mediumEnemieRate && random_num <= smallEnemieRate + mediumEnemieRate + bigEnemieRate)
        { type = EnemyType.big; }

        int index = 0;

        // Create enemy  ---------------------------------------------------
        switch (type)
        {
            case EnemyType.small:
                time_btw_spawns = spawn_time_small;
                index = Random.Range(0, smallEnemiesPrefabs.Count);
                CreateEnemy(position, smallEnemiesPrefabs[index], enemyNamesSmall[Random.Range(0, enemyNamesSmall.Count)]);
                break;

            case EnemyType.medium:
                time_btw_spawns = spawn_time_medium;
                index = Random.Range(0, mediumEnemiesPrefabs.Count);
                CreateEnemy(position, mediumEnemiesPrefabs[index], enemyNamesSmall[Random.Range(0, enemyNamesMedium.Count)]);
                break;

            case EnemyType.big:
                time_btw_spawns = spawn_time_big;
                index = Random.Range(0, bigEnemiesPrefabs.Count);
                CreateEnemy(position, bigEnemiesPrefabs[index], enemyNamesBig[Random.Range(0, enemyNamesBig.Count)]);
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
