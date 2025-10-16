using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] protected bool gameOvered = false;
    [SerializeField] protected bool gameFinished = false;

    private static GameObject MusicPlayer;
    [SerializeField] private AudioSource audioSource;

    private AudioClip musicaFundo;
    private AudioClip musicaGameOver;


    void Awake()
    {
        currentSkinIndex = GameManagement.CurrentSkinIndex;
        ConfigData();         
    }

    void Start()
    {
        audioSource.clip = musicaFundo;
        audioSource.loop = true;
        audioSource.Play();

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null) GameManagement.CurrentPlayer = playerObject;
        GameManagement.ConfigPlayerSkin();

        GameObject player = GameManagement.CurrentPlayer;
        mainCamera.transform.position = player.transform.position;
        mainCamera.GetComponent<CameraManager>().SetActualTarget(player);
        mainCamera.transform.localScale = new(1, 1, 1);

        telaFinalPontuacao.SetActive(false);
        gameFinished = false;
        pauseMenuManager.GetComponent<PauseMenuManager>().enabled = true;

        gameOverPanel.SetActive(false);
        gameOvered = false;
    }

    void Update()
    {
        if (!GameManagement.CurrentPlayer.GetComponent<PlayerManager>().IsAlive())
        {
            audioSource.Stop();

            audioSource.clip = musicaGameOver;
            audioSource.loop = false;
            audioSource.time = 0.5f;

            audioSource.Play();
            
            gameOvered = true;
            gameOverPanel.SetActive(true);
        } 

        if (Input.GetKeyDown(KeyCode.Escape) && !gameFinished)
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

        if (MusicPlayer == null)
        {
            MusicPlayer = new GameObject("Music Player");
            MusicPlayer.transform.position = playerSpawnPoint.transform.position;
            DontDestroyOnLoad(MusicPlayer);
        }
        audioSource = MusicPlayer.GetOrAddComponent<AudioSource>();
        audioSource.volume = GameManagement.MusicVolume;
        audioSource.playOnAwake = false;

        musicaFundo = Resources.Load<AudioClip>("Sounds/Megaman X Theme");
        musicaGameOver = Resources.Load<AudioClip>("Sounds/GameOver");
    }

    public Vector3 GetPlayerSpawnPoint()
    {
        return playerSpawnPoint.GetComponent<Transform>().position;
    }

    public void PauseGame()
    {
        audioSource.Pause();
        pauseMenuManager.GetComponent<PauseMenuManager>().PauseGame();
    }

    public void ResumeGame()
    {
        audioSource.UnPause();
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
        gameFinished = true;
        GameManagement.CurrentPlayer.GetComponent<PlayerManager>().StopPlayer();
        
        StartCoroutine(StopGameTimeAtFinishRoutine());
    }

    private IEnumerator StopGameTimeAtFinishRoutine()
    {
        yield return new WaitForSeconds(0.6f);

        telaFinalPontuacao.SetActive(true);

        PlayerManager playerManager = GameManagement.CurrentPlayer.GetComponent<PlayerManager>();

        TextMeshProUGUI kiwiInfoText = scoreResumePanelInfo.transform.GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI orangeInfoText = scoreResumePanelInfo.transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI pontosTotaisText = scoreResumePanelInfo.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

        GameManagement.AtualizaScoreInfoPlayer(playerManager, kiwiInfoText, orangeInfoText, pontosTotaisText);
        Destroy(MusicPlayer);
    }
}