using UnityEngine;

public static class EnemyConstructor
{
    public static void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPosition, Quaternion rotation)
    {
        Object.Instantiate(enemyPrefab, spawnPosition, rotation);
    }
}