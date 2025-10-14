using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline.Actions;
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
    [SerializeField] protected GameObject telaFinalPontuacao;
    [SerializeField] protected GameObject scoreResumePanelInfo;
    [SerializeField] protected GameObject gameOverPanel;
    [SerializeField] protected static bool gameOvered = false;

    private static GameObject MusicPlayer;
    [SerializeField] AudioSource musicaFundo;
    [SerializeField] AudioSource musicaVitoria;


    void Awake()
    {
        currentSkinIndex = GameManagement.CurrentSkinIndex;
        ConfigData(); 
    }

    void Start()
    {
        if (MusicPlayer == null)
        {
            MusicPlayer = new GameObject("Music Player");
            MusicPlayer.transform.position = playerSpawnPoint.transform.position;
            DontDestroyOnLoad(MusicPlayer);

            musicaVitoria = MusicPlayer.AddComponent<AudioSource>();
            musicaVitoria.clip = Resources.Load<AudioClip>("Sounds/Victory Sound");
            musicaVitoria.volume = GameManagement.MusicVolume;
            musicaVitoria.loop = false;

            musicaFundo = MusicPlayer.AddComponent<AudioSource>();
            musicaFundo.clip = Resources.Load<AudioClip>("Sounds/Megaman X Theme");
            musicaFundo.volume = GameManagement.MusicVolume;
            musicaFundo.loop = true;
            musicaFundo.Play();
        }

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null) GameManagement.CurrentPlayer = playerObject;
        GameManagement.ConfigPlayerSkin();

        GameObject player = GameManagement.CurrentPlayer;
        mainCamera.transform.position = player.transform.position;
        mainCamera.GetComponent<CameraManager>().SetActualTarget(player);
        mainCamera.transform.localScale = new(1, 1, 1);

        telaFinalPontuacao.SetActive(false);
        pauseMenuManager.GetComponent<PauseMenuManager>().enabled = true;

        gameOverPanel.SetActive(false);
        gameOvered = false;
    }

    void Update()
    {
        if (!GameManagement.CurrentPlayer.GetComponent<PlayerManager>().IsAlive())
        {
            gameOvered = true;
            gameOverPanel.SetActive(true);
        } 

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

        if (Input.GetKeyDown(KeyCode.R) && gameOvered)
        {
            RestartCurrentScene();
        }
    }

    protected void ConfigData()
    {
        if (mainCamera == null) mainCamera = GameManagement.MainCamera;
        mainCamera.transform.localScale = new(10, 10, 10);
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
        musicaFundo.Pause();
        pauseMenuManager.GetComponent<PauseMenuManager>().PauseGame();
    }

    public void ResumeGame()
    {
        musicaFundo.UnPause();
        pauseMenuManager.GetComponent<PauseMenuManager>().ResumeGame();
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        mainCamera = GameManagement.MainCamera;
    }

    public void DestroyMusicPlayer()
    {
        Destroy(MusicPlayer);
    }

    public void FinishGame()
    {
        musicaFundo.UnPause();
        musicaVitoria.Play();

        GameManagement.CurrentPlayer.GetComponent<PlayerManager>().StopPlayer();
        
        StartCoroutine(StopGameTimeAtFinishRoutine());
    }

    private IEnumerator StopGameTimeAtFinishRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        telaFinalPontuacao.SetActive(true);

        PlayerManager playerManager = GameManagement.CurrentPlayer.GetComponent<PlayerManager>();

        TextMeshProUGUI kiwiInfoText = scoreResumePanelInfo.transform.GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI orangeInfoText = scoreResumePanelInfo.transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI pontosTotaisText = scoreResumePanelInfo.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

        GameManagement.AtualizaScoreInfoPlayer(playerManager, kiwiInfoText, orangeInfoText, pontosTotaisText);
        Destroy(MusicPlayer);
    }
}