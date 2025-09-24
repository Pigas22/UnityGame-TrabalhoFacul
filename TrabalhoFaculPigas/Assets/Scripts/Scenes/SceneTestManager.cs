using System.Collections.Generic;
using UnityEngine;

public class SceneTestManager : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnData> enemiesToSpawn;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector2 camLimits = new Vector2(2.5f, 1f);

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        mainCamera.GetComponent<CameraManager>().SetLevelLimits(camLimits);

        playerSpawnPoint = GameObject.Find("PlayerSpawnPoint").transform;


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
        return playerSpawnPoint.position;
    }
}