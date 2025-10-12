using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class SceneManagerModel : MonoBehaviour
{
    [SerializeField] protected GameObject pauseMenuManager;

    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected Vector2 camLimits = new Vector2(25f, 10f);
    [SerializeField] protected bool isPaused = false;
    [SerializeField] protected GameObject playerSpawnPoint;
    [SerializeField] public int currentSkinIndex;


    void Awake()
    {
        currentSkinIndex = GameManagement.CurrentSkinIndex;
        ConfigData(); 
    }

    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null) GameManagement.CurrentPlayer = playerObject;
        GameManagement.ConfigPlayerSkin();
        
        pauseMenuManager.GetComponent<PauseMenuManager>().enabled = true;
        
        mainCamera.GetComponent<CameraManager>().SetActualTarget(GameManagement.CurrentPlayer);
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
        if (mainCamera == null) mainCamera = GameManagement.MainCamera;
        mainCamera.GetComponent<CameraManager>().SetLevelLimits(camLimits);

        if (playerSpawnPoint == null) playerSpawnPoint = GameObject.Find("PlayerSpawnPoint");

        if (pauseMenuManager == null) pauseMenuManager = GameObject.Find("PauseMenuManager");
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
        mainCamera = GameManagement.MainCamera;
    }
}