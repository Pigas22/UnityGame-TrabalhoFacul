using UnityEngine;

public static class EnemyConstructor
{
    public static void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPosition, Quaternion rotation, Transform parent = null) 
    {
        spawnPosition.z = 0; // garante o plano 2D
        GameObject enemy;

        if (parent == null)
        {
            enemy = Object.Instantiate(enemyPrefab, spawnPosition, rotation);
        }
        else
        {
            enemy = Object.Instantiate(enemyPrefab, spawnPosition, rotation, parent);
            //enemy.transform.position = spawnPosition; // força posição global
        }

        enemy.transform.localScale = enemyPrefab.transform.localScale;
    }
}