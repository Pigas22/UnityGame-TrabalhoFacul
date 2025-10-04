using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class SceneManagerModel : MonoBehaviour
{
    [SerializeField] protected GameObject pauseMenuManager;

    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected Vector2 camLimits = new Vector2(25f, 10f);
    [SerializeField] protected bool isPaused = false;
    [SerializeField] protected GameObject playerSpawnPoint;

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = GameManagement.MainCamera;
        }

        DontDestroyOnLoad(mainCamera.gameObject);

        if (playerSpawnPoint == null)
        {
            playerSpawnPoint = new GameObject(name = "PlayerSpawnPoint");
        }

        ConfigData();
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

    protected void ConfigData()
    {
        mainCamera.GetComponent<CameraManager>().SetLevelLimits(camLimits);

        if (pauseMenuManager == null) 
            {
                pauseMenuManager = GameObject.Find("PauseMenuManager");
            }
        pauseMenuManager.GetComponent<PauseMenuManager>().ResumeGame();
    }

    public Vector3 GetPlayerSpawnPoint()
    {
        return playerSpawnPoint.GetComponent<Transform>().position;
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
}