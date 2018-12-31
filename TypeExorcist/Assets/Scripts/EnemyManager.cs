using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Xml;

public struct RoundSpawnRate
{
    public float smallEnemyRate;
    public float mediumEnemyRate;
    public float bigEnemyRate;

    public float spawnTimeBig;
    public float spawnTimeMedium;
    public float spawnTimeSmall;
}

public class EnemyManager : MonoBehaviour
{
 
    public enum EnemyType  { none = -1, small = 0, medium = 1, big = 2}

AudioSource next_wave = null;
    [HideInInspector] public List<Enemy> enemies;       //List of all active enemies  

    [Header("Enemies prefabs")]
    public List<GameObject> smallEnemiesPrefabs;        //List of small enemy prefabs 
    public List<GameObject> mediumEnemiesPrefabs;       //List of medium enemy prefabs 
    public List<GameObject> bigEnemiesPrefabs;          //List of big enemy prefabs 

    // Spawn logic ------------------------
    private uint enemiesPerRound;
    private uint enemiesCount = 0;
    private RoundSpawnRate roundSpawnRates;
    private uint round = 1;

    [Header("Spawn Rate Logic")]

    private uint defaultEnemiesPerRound = 10;
    private uint enemiesAddedPerRound = 4;
    private float smallEnemyBaseRate = 50;
    private float mediumEnemyBaseRate = 30;
    private float bigEnemyBaseRate = 20;
    private float smallRatePercentMultiplyer = 0.0f;
    private float mediumRatePercentMultiplyer = 0.3f;
    private float bigRatePercentMultiplyer = 0.4f;
    private float smallBaseSpawnTime = 3;
    private float mediumBaseSpawnTime = 4;
    private float bigBaseSpawnTime = 5;
    private float smallSpawnTimeDecrease = 0.15f;
    private float mediumSpawnTimeDecrease =  0.15f;
    private float bigSpawnTimeDecrease = 0.16f;

    private bool onIntervalRound = false;
    private Timer intervalRoundTimer = new Timer();
    [SerializeField] private float intervalRoundTime = 10.0f;

    private Timer spawnTimer = new Timer();
    private float timeBtwSpawns = 0.0f;


    [HideInInspector] public bool spawnEnemies = true;

    //Names variables --------------------------------
  
    private List<string> enemyNamesSmall;              
    private List<string> enemyNamesMedium;
    private List<string> enemyNamesBig;
    private TextAsset allWords;

    //Colors------------------------------------------
    public Color inactiveColor;
    public Color32 activeColor;
    [HideInInspector]public string inactiveColorStr;
  
    // Randomizer with letters -------------

    [Header("Randomizer with letters")]
    public char [] availableLetters;

    private void Start()
    {
        GameObject audio = GameObject.Find("Audio Manager");
        next_wave = GetComponent<AudioSource>();

        enemies = new List<Enemy>();
        enemyNamesSmall = new List<string>();
        enemyNamesMedium = new List<string>();
        enemyNamesBig = new List<string>();
        allWords = Resources.Load("Words") as TextAsset;
        LoadWord();
        roundSpawnRates = new RoundSpawnRate();
        inactiveColorStr = "<color=#" + ColorUtility.ToHtmlStringRGB(inactiveColor) + ">";

        ChangeRound(round);
        spawnTimer.StarTimer();
    }
    

    void Update()
    {
        if(onIntervalRound)
        {
            if (intervalRoundTimer.GetCurrentTime() > intervalRoundTime)
            {
                //music.Pause();
                ChangeRound(++round);
                onIntervalRound = false;
            }
            else
            {
                return;
            }
        }

        if (enemiesCount < enemiesPerRound && spawnEnemies && spawnTimer.GetCurrentTime() > timeBtwSpawns )
        {
            GenerateRandomEnemy();
            spawnTimer.StarTimer();
        }

        if (enemiesCount == enemiesPerRound && enemies.Count == 0)
        {
            onIntervalRound = true;
            intervalRoundTimer.StarTimer();
            enemiesCount = 0;
        }
    }

    void ChangeRound(float new_round)
    {
        enemiesPerRound = defaultEnemiesPerRound + enemiesAddedPerRound * (uint)new_round;

        roundSpawnRates.smallEnemyRate = smallEnemyBaseRate + new_round * smallRatePercentMultiplyer * smallEnemyBaseRate;
        roundSpawnRates.mediumEnemyRate = mediumEnemyBaseRate + new_round * mediumRatePercentMultiplyer * mediumEnemyBaseRate;
        roundSpawnRates.bigEnemyRate = bigEnemyBaseRate + new_round * bigRatePercentMultiplyer * bigEnemyBaseRate;

        roundSpawnRates.spawnTimeSmall = smallBaseSpawnTime - smallBaseSpawnTime * smallSpawnTimeDecrease * new_round;
        roundSpawnRates.spawnTimeMedium = mediumBaseSpawnTime - mediumBaseSpawnTime * mediumSpawnTimeDecrease * new_round;
        roundSpawnRates.spawnTimeBig = bigBaseSpawnTime - bigBaseSpawnTime * bigSpawnTimeDecrease * new_round;


        //Don't make negative time (but yes impossible to win)
        if (roundSpawnRates.spawnTimeSmall < 0.01f)
            roundSpawnRates.spawnTimeSmall = 0.01f;

        if (roundSpawnRates.spawnTimeMedium < 0.01f)
            roundSpawnRates.spawnTimeMedium = 0.01f;

        if (roundSpawnRates.spawnTimeBig < 0.01f)
            roundSpawnRates.spawnTimeBig = 0.01f;

        next_wave.Play();
    }

    void CreateEnemy(Vector3 enemyPos, GameObject enemyPrefab, string enemyName)
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

        float probability_range = roundSpawnRates.smallEnemyRate + roundSpawnRates.mediumEnemyRate + roundSpawnRates.bigEnemyRate;
        float random_num = Random.Range(0, probability_range);

        if (random_num >= 0 && random_num < roundSpawnRates.smallEnemyRate)
        { type = EnemyType.small; }
        else if (random_num > roundSpawnRates.smallEnemyRate && random_num <= roundSpawnRates.smallEnemyRate + roundSpawnRates.mediumEnemyRate)
        { type = EnemyType.medium; }
        else if(random_num > roundSpawnRates.smallEnemyRate + roundSpawnRates.mediumEnemyRate && random_num <= roundSpawnRates.smallEnemyRate + roundSpawnRates.mediumEnemyRate + roundSpawnRates.bigEnemyRate)
        { type = EnemyType.big; }

        int index = 0;

        // Create enemy  ---------------------------------------------------
        switch (type)
        {
            case EnemyType.small:
                timeBtwSpawns = roundSpawnRates.spawnTimeSmall;
                index = Random.Range(0, smallEnemiesPrefabs.Count);
                CreateEnemy(position, smallEnemiesPrefabs[index], enemyNamesSmall[Random.Range(0, enemyNamesSmall.Count)]);
                break;

            case EnemyType.medium:
                timeBtwSpawns = roundSpawnRates.spawnTimeMedium;
                index = Random.Range(0, mediumEnemiesPrefabs.Count);
                CreateEnemy(position, mediumEnemiesPrefabs[index], enemyNamesMedium[Random.Range(0, enemyNamesMedium.Count)]);
                break;

            case EnemyType.big:
                timeBtwSpawns = roundSpawnRates.spawnTimeBig;
                index = Random.Range(0, bigEnemiesPrefabs.Count);
                CreateEnemy(position, bigEnemiesPrefabs[index], enemyNamesSmall[Random.Range(0, enemyNamesSmall.Count)]);
                break;
        }

        ++enemiesCount;
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
