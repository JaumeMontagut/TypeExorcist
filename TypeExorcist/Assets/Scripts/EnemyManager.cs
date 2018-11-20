using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public GameObject enemyPrefab;
    private List<GameObject> enemies;

    private void Start()
    {
        enemies = new List<GameObject>();
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
        GameObject newEnemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
        enemies.Add(newEnemy);
    }

}
