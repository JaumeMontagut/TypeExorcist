using System.Collections;
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

    public List<GameObject> enemiesPrefabs;
    public List<int> enemiesSpawnRate;
    private List<Enemy> enemies;
    private List<string> enemyNames;

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
        int totalChance = 0;
        for (int i =0;i< enemiesSpawnRate.Count; i++)
        {
            totalChance += enemiesSpawnRate[i];
        }
        for (int i = 0; i < enemiesSpawnRate.Count; i++)
        {
            enemiesSpawnRate[i] = (enemiesSpawnRate[i] / totalChance) * 100;
        }
        inactiveColorStr = "<color=#" + ColorUtility.ToHtmlStringRGB(inactiveColor) + ">";
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

    public void GenerateRandomEnemy()
    {
        int probabilityManager = Random.Range(0, 100);
        int enemyIndex = Random.Range(0, enemiesPrefabs.Count);
        Vector3 position = new Vector3(0, 0, 0);

        int startingPosition = Random.Range(1, 5);
        int cameraHeight = (int)Camera.main.orthographicSize;
        int cameraWidth = (int)(Camera.main.orthographicSize * Camera.main.aspect);

        switch (startingPosition)
        {
            case 1:
                position.x = -cameraWidth;
                position.y = Random.Range(-cameraHeight, cameraHeight + 1); 
                break;
            case 2:
                position.x = cameraWidth;
                position.y = Random.Range(-cameraHeight, cameraHeight + 1);
                break;
            case 3:
                position.y = -cameraHeight;
                position.x = Random.Range(-cameraWidth, cameraWidth + 1);
                break;
            case 4:
                position.y = cameraHeight;
                position.x = Random.Range(-cameraWidth, cameraWidth + 1);
                break;

            default:
                break;
        }
        Debug.Log(enemyIndex);
        CreateEnemy(position, enemiesPrefabs[enemyIndex],enemyNames[Random.Range(0, enemyNames.Count)]);
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
