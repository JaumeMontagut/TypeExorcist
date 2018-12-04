using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<int> enemiesSpawnRate;
    private List<Enemy> enemies;

    private void Start()
    {
        enemies = new List<Enemy>();
        enemyNames = new List<string>();
        enemyNames.Add("holy water");
        enemyNames.Add("bible");
        enemyNames.Add("randomlygeneratedstring a");
        enemyNames.Add("randomlygeneratedstring b");
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

        int probabilityManager = Random.Range(0, 100);
        int enemyIndex = -1;
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
        
  //      if (index<25)
  //      {
  //          enemyIndexType = (int)enemyIndex.TRIANGLE;
  //      }
  //      if (index >= 25 && index< 50)
  //      {
  //          enemyIndexType = (int)enemyIndex.SQUARE;
  //      }
  //      if (index >= 50 && index <= 75)
  //      {
  //          enemyIndexType = (int)enemyIndex.CIRCLE;
  //      }
		//if (index >= 75 && index <= 100)
		//{
		//	enemyIndexType = (int)enemyIndex.DEMONIC_ARCHANGEL;
		//}




        CreateEnemy(position, enemiesPrefabs[enemyIndex],enemyNames[Random.Range(0, 4)]);
    }
}
