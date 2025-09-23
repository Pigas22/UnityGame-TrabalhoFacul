using System.Collections.Generic;
using UnityEngine;

public class SceneTestManager : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnData> enemiesToSpawn;
    [SerializeField] private Vector3 playerSpawnPoint;

    void Start()
    {
        playerSpawnPoint = GameObject.Find("PlayerSpawnPoint").transform.position;
        foreach (var enemyData in enemiesToSpawn)
        {
            if (enemyData.enemyPrefab != null)
            {
                EnemyConstructor.SpawnEnemy(enemyData.enemyPrefab, enemyData.spawnPosition, Quaternion.identity);
            }
        }
    }

    public Vector3 GetPlayerSpawnPoint()
    {
        return playerSpawnPoint;
    }
}