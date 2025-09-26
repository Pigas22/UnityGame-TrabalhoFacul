using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTestManager : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnData> enemiesToSpawn;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject pauseMenuManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector2 camLimits = new Vector2(2.5f, 1f);
    [SerializeField] private bool isPaused = false;

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        DontDestroyOnLoad(mainCamera.gameObject);

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

    void ConfigData()
    {
        mainCamera.GetComponent<CameraManager>().SetLevelLimits(camLimits);

        if (playerObject == null)
        {
            playerObject = GameObject.Find("Player");
        }


        if (pauseMenuManager == null)
        {
            pauseMenuManager = GameObject.Find("PauseMenuManager");
        }
        pauseMenuManager.GetComponent<PauseMenuManager>().gameObject.SetActive(false);

        if (playerSpawnPoint == null) playerSpawnPoint = GameObject.Find("PlayerSpawnPoint").transform;
    }

    public void PauseGame()
    {
        pauseMenuManager.GetComponent<PauseMenuManager>().PauseGame();
    }

    public void ResumeGame()
    {
        pauseMenuManager.GetComponent<PauseMenuManager>().ResumeGame();
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public Vector3 GetPlayerSpawnPoint()
    {
        return playerSpawnPoint.position;
    }

    public GameObject GetPlayerObject()
    {
        return this.playerObject;
    }
}