using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public GameObject enemyBasic;
    public GameObject enemySquare;
    public GameObject enemyCircle;
    private List<Enemy> enemies;

    private void Start()
    {
        enemies = new List<Enemy>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 enemyPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            enemyPos.z = 0;
            CreateEnemy(enemyPos, enemyBasic);
        }
    }

    void CreateEnemy(Vector3 enemyPos,GameObject enemyPrefab)
    {
        Enemy newEnemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity).GetComponent<Enemy>();
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
        Enemy generatedEnemy = null;

        int index = Random.Range(0, 100);
        Vector3 position = new Vector3(0, 0, 0);

        int positionRandomX = Random.Range(1, Screen.width);
        int positionRandomY = Random.Range(1, Screen.height);

        position.x = positionRandomX;
        position.y = positionRandomY;

        CreateEnemy(position, enemyBasic);
    }
}
