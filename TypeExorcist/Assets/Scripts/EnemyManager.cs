using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Xml;


public class EnemyManager : MonoBehaviour
{

    public enum EnemyType  { none = -1, small = 0, medium = 1, big = 2}

    [HideInInspector] public List<Enemy> enemies;       //List of all active enemies  
    public List<GameObject> smallEnemiesPrefabs;        //List of small enemy prefabs 
    public List<GameObject> mediumEnemiesPrefabs;       //List of medium enemy prefabs 
    public List<GameObject> bigEnemiesPrefabs;          //List of big enemy prefabs 

    public float smallEnemieRate = 0.0f;
    public float mediumEnemieRate = 0.0f;
    public float bigEnemieRate = 0.0f;

    //Names variables --------------------------------
    private List<string> enemyNamesSmall;              
    private List<string> enemyNamesMedium;
    private List<string> enemyNamesBig;
  
    public uint Level = 1;
    //Colors------------------------------------------
    public Color inactiveColor;
    public Color32 activeColor;
    [HideInInspector]public string inactiveColorStr;



    // Spawn logic ------------------------

    private Timer spawnTimer = new Timer();
    private float timeBtwSpawns = 0.0f;
    public float spawnTimeBig = 0.0f;
    public float spawnTimeMedium = 0.0f;
    public float spawnTimeSmall = 0.0f;

    // Randomizer with letters -------------

    [Header("Randomizer with letters")]
    public char [] availableLetters;

    private void Start()
    {
        spawnTimer.StarTimer();
       
            //SelectNode(levelNodePath);

        enemies = new List<Enemy>();

        enemyNamesSmall = new List<string>();
        enemyNamesMedium = new List<string>();
        enemyNamesBig = new List<string>();

        LoadWord();
        
        inactiveColorStr = "<color=#" + ColorUtility.ToHtmlStringRGB(inactiveColor) + ">";
    }
    

    void Update()
    {
        if (spawnTimer.GetCurrentTime() > timeBtwSpawns)
        {
            GenerateRandomEnemy();
            spawnTimer.StarTimer();
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
                timeBtwSpawns = spawnTimeSmall;
                index = Random.Range(0, smallEnemiesPrefabs.Count);
                CreateEnemy(position, smallEnemiesPrefabs[index], enemyNamesSmall[Random.Range(0, enemyNamesSmall.Count)]);
                break;

            case EnemyType.medium:
                timeBtwSpawns = spawnTimeMedium;
                index = Random.Range(0, mediumEnemiesPrefabs.Count);
                CreateEnemy(position, mediumEnemiesPrefabs[index], enemyNamesSmall[Random.Range(0, enemyNamesMedium.Count)]);
                break;

            case EnemyType.big:
                timeBtwSpawns = spawnTimeBig;
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

    //Randomly combines available letters to create a string with the specified length
    public string GenerateNameLetters(int numChars)
    {
        string enemyName = "";
        for (int i = 0; i < numChars; ++i)
        {
            int randIndex = Random.Range(0, availableLetters.Length);
            enemyName = enemyName.Insert(i, availableLetters[randIndex].ToString());
        }
        return enemyName;
    }
     void LoadWord()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/txtDoc/EnemiesWords.xml");
        
        XmlNode LevelNode = doc.DocumentElement.SelectSingleNode("level"+ Level.ToString());
        for (XmlNode ThisNode = LevelNode.SelectSingleNode("easy").FirstChild; ThisNode != null; ThisNode = ThisNode.NextSibling)
        {
            enemyNamesSmall.Add(ThisNode.InnerText);
        }
        for (XmlNode ThisNode = LevelNode.SelectSingleNode("medium").FirstChild; ThisNode != null; ThisNode = ThisNode.NextSibling)
        {
            enemyNamesMedium.Add(ThisNode.InnerText);
        }
        for (XmlNode ThisNode = LevelNode.SelectSingleNode("hard").FirstChild; ThisNode != null; ThisNode = ThisNode.NextSibling)
        {
            enemyNamesBig.Add(ThisNode.InnerText);
        }
    }
}
