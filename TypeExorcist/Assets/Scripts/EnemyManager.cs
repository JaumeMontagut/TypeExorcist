using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Xml;

public struct LevelSpawnRate
{
    public float smallEnemieRate;
    public float mediumEnemieRate;
    public float bigEnemieRate;
}

public class EnemyManager : MonoBehaviour
{
 
    public enum EnemyType  { none = -1, small = 0, medium = 1, big = 2}

    [HideInInspector] public List<Enemy> enemies;       //List of all active enemies  

    [Header("Enemies prefabs")]
    public List<GameObject> smallEnemiesPrefabs;        //List of small enemy prefabs 
    public List<GameObject> mediumEnemiesPrefabs;       //List of medium enemy prefabs 
    public List<GameObject> bigEnemiesPrefabs;          //List of big enemy prefabs 

    // Spawn logic ------------------------
    public LevelSpawnRate[] levelsSpawnRates;
    [Header("Spawn Rate Logic")]
    public float smallEnemieBaseRate = 0.0f;
    public float smallRatePercentMultiplyer = 0.0f;
    public float mediumEnemieBaseRate = 0.0f;
    public float mediumRatePercentMultiplyer = 0.0f;
    public float bigEnemieBaseRate = 0.0f;
    public float bigRatePercentMultiplyer = 0.0f;
  
    private Timer spawnTimer = new Timer();
    private float timeBtwSpawns = 0.0f;
    public float spawnTimeBig = 0.0f;
    public float spawnTimeMedium = 0.0f;
    public float spawnTimeSmall = 0.0f;

    [HideInInspector] public bool spawnEnemies = true;

    //Names variables --------------------------------
  
    private List<string> enemyNamesSmall;              
    private List<string> enemyNamesMedium;
    private List<string> enemyNamesBig;
    private TextAsset allWords;
    [Header("Enemies Names")]
    public string charsWanted="hola";
    public uint level = 1;
    public uint maxLevels = 9;

    //Colors------------------------------------------
    public Color inactiveColor;
    public Color32 activeColor;
    [HideInInspector]public string inactiveColorStr;
  
    // Randomizer with letters -------------

    [Header("Randomizer with letters")]
    public char [] availableLetters;

    private void Start()
    {
        spawnTimer.StarTimer();
        enemies = new List<Enemy>();
        enemyNamesSmall = new List<string>();
        enemyNamesMedium = new List<string>();
        enemyNamesBig = new List<string>();
        allWords = Resources.Load("Words") as TextAsset;

        LoadWord();

        levelsSpawnRates = new LevelSpawnRate[maxLevels];

        for (uint i = 0; i < maxLevels; ++i)
        {
            levelsSpawnRates[i].smallEnemieRate = smallEnemieBaseRate + smallEnemieBaseRate * i * smallRatePercentMultiplyer;
            levelsSpawnRates[i].mediumEnemieRate = mediumEnemieBaseRate + mediumEnemieBaseRate * i * mediumRatePercentMultiplyer;
            levelsSpawnRates[i].bigEnemieRate = bigEnemieBaseRate + bigEnemieBaseRate * i * bigRatePercentMultiplyer;
        }

        inactiveColorStr = "<color=#" + ColorUtility.ToHtmlStringRGB(inactiveColor) + ">";
    }
    

    void Update()
    {
        if (spawnEnemies && spawnTimer.GetCurrentTime() > timeBtwSpawns)
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

        float probability_range = levelsSpawnRates[level -1].smallEnemieRate + levelsSpawnRates[level - 1].mediumEnemieRate + levelsSpawnRates[level - 1].bigEnemieRate;
        float random_num = Random.Range(0, probability_range);

        if (random_num >= 0 && random_num < levelsSpawnRates[level - 1].smallEnemieRate)
        { type = EnemyType.small; }
        else if (random_num > levelsSpawnRates[level - 1].smallEnemieRate && random_num <= levelsSpawnRates[level - 1].smallEnemieRate + levelsSpawnRates[level - 1].mediumEnemieRate)
        { type = EnemyType.medium; }
        else if(random_num > levelsSpawnRates[level - 1].smallEnemieRate + levelsSpawnRates[level - 1].mediumEnemieRate && random_num <= levelsSpawnRates[level - 1].smallEnemieRate + levelsSpawnRates[level - 1].mediumEnemieRate + levelsSpawnRates[level - 1].bigEnemieRate)
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
                CreateEnemy(position, mediumEnemiesPrefabs[index], enemyNamesMedium[Random.Range(0, enemyNamesMedium.Count)]);
                break;

            case EnemyType.big:
                timeBtwSpawns = spawnTimeBig;
                index = Random.Range(0, bigEnemiesPrefabs.Count);
                CreateEnemy(position, bigEnemiesPrefabs[index], enemyNamesSmall[Random.Range(0, enemyNamesSmall.Count)]);
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
        //Goes through all the characters in the txt
        //It keeps them in wordToAdd
        //When it finds a \n (end of line) it adds the word in one of the lists, depending on the number of characters (thre is only one word in each line in the txt)
        string wordToAdd = "";
        for (int num = 0; num < allWords.text.Length; ++num)
        {
            if (allWords.text[num] != '\n')
            {
                wordToAdd += allWords.text[num];
            }
            else 
            {
                if (wordToAdd.Length <= 5)
                {
                    enemyNamesSmall.Add(wordToAdd);
                }
                else if (wordToAdd.Length <= 8)
                {
                    enemyNamesMedium.Add(wordToAdd);
                }
                else
                {
                    enemyNamesBig.Add(wordToAdd);
                }
                wordToAdd = "";
            }

            
        }

    }
}
