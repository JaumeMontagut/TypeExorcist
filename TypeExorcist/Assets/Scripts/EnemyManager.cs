using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public GameObject enemyPrefab;
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
            CreateEnemy(enemyPos);
        }
    }

    void CreateEnemy(Vector3 enemyPos)
    {
        Enemy newEnemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity).GetComponent<Enemy>();
        enemies.Add(newEnemy);
    }

    //Returns the closest enemy to the center of the screen that starts with the specified letter
    public Enemy GetCloserEnemyWithName (string firstLetter)
    {
        Enemy closerEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            if (enemy.enemyName.StartsWith(firstLetter))
            {
                //The position that is closer to the center wins
                if (closerEnemy == null || Utilities.DistanceSquared(enemy.transform.position, Vector2.zero) < Utilities.DistanceSquared(closerEnemy.transform.position, Vector2.zero))
                {
                    closerEnemy = enemy;
                }
            }
        }
        return closerEnemy;
    }

}
