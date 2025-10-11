using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class SceneTestManager : SceneManagerModel
{
    [SerializeField] private List<EnemySpawnData> enemiesToSpawn;
    [SerializeField] private GameObject CurrentPlayer;

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = GameManagement.MainCamera;
        }

        ConfigData();

        foreach (var enemyData in enemiesToSpawn)
        {
            if (enemyData.enemyPrefab != null)
            {
                EnemyConstructor.SpawnEnemy(enemyData.enemyPrefab, enemyData.spawnPosition, Quaternion.identity);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
            isPaused = !isPaused;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartCurrentScene();
        }
    }

    new void ConfigData()
    {
        base.ConfigData();

        if (CurrentPlayer == null) CurrentPlayer = GameManagement.CurrentPlayer;
        if (playerSpawnPoint == null) playerSpawnPoint = GameObject.Find("PlayerSpawnPoint");
    }

    public GameObject GetPlayerObject()
    {
        return this.CurrentPlayer;
    }
}