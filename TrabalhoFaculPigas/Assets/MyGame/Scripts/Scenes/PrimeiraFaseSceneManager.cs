using System.Collections.Generic;
using UnityEngine;

public class PrimeiraFaseSceneManager : SceneManagerModel
{
    [SerializeField] private GameObject sceneEnimies;
    [SerializeField] private List<EnemySpawnData> enemiesToSpawn;

    void Awake()
    {
        foreach (var enemyData in enemiesToSpawn)
        {
            if (enemyData.enemyPrefab != null)
            {
                EnemyConstructor.SpawnEnemy(enemyData.enemyPrefab, enemyData.spawnPosition, Quaternion.identity, sceneEnimies.GetComponent<Transform>());
            }
        }
    }
}