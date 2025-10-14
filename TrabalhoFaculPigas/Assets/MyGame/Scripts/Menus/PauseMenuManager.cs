using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private SceneManagerModel sceneManager;
    [SerializeField] private GameObject canvasPauseMenu;
    public GameObject pontosTotaisObject;
    private TextMeshProUGUI pontosTotaisText;
    [SerializeField] private TextMeshProUGUI kiwiCollectableInfoText;
    [SerializeField] private TextMeshProUGUI orangeCollectableInfoText;
    [SerializeField] private GameObject panelConfigBackground;
    [SerializeField] private GameObject configPanel;

    void Start()
    {
        sceneManager = sceneManager == null ? GameObject.Find("SceneManager").GetComponent<SceneManagerModel>() : sceneManager;
        canvasPauseMenu = canvasPauseMenu == null ? GameObject.Find("CanvasPauseMenu") : canvasPauseMenu;

        pontosTotaisText = pontosTotaisObject.GetComponent<TextMeshProUGUI>();

        panelConfigBackground = panelConfigBackground == null ? GameObject.Find("PanelConfigBackground") : panelConfigBackground;
        configPanel = configPanel == null ? GameObject.Find("ConfigPanel") : configPanel;

        canvasPauseMenu.SetActive(false);
        panelConfigBackground.SetActive(false);
        configPanel.SetActive(false);
    }
    public void PauseGame()
    {
        Time.timeScale = 0f; // Pausa o tempo do jogo
        canvasPauseMenu.SetActive(true); // Ativa o menu de pausa
        OnEnable();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Retoma o tempo do jogo
        canvasPauseMenu.SetActive(false); // Desativa o menu de pausa

        panelConfigBackground.SetActive(false);
        configPanel.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        GameManagement.DebugIsOpenMenu(configPanel, true);
        panelConfigBackground.SetActive(true);
        configPanel.SetActive(true);
    }
    public void SaveSettings()
    {
        GameManagement.DebugLog("Salvando Alterações");

        CloseSettingsMenu();
    }
    public void CloseSettingsMenu()
    {
        GameManagement.DebugIsOpenMenu(configPanel, false);
        panelConfigBackground.SetActive(false);
        configPanel.SetActive(false);
    }


    public void RestartLevel()
    { 
        GameManagement.DebugLog("Restarting level...");
        Time.timeScale = 1f;
        sceneManager.RestartCurrentScene();
    }


    public void LoadMainMenu()
    {
        sceneManager.DestroyMusicPlayer();
        GameManagement.CurrentSkinIndex = sceneManager.currentSkinIndex;
        GameManagement.DebugLog("Loading main menu...");
        Time.timeScale = 1f; // Retoma o tempo do jogo
        SceneManager.LoadScene("MenuPrincipal");
    }

    void OnEnable()
    {
        if (GameManagement.CurrentPlayer == null)
        {
            Debug.LogWarning("CurrentPlayer ainda não foi inicializado.");
            return;
        }

        PlayerManager playerManager = GameManagement.CurrentPlayer.GetComponent<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogWarning("PlayerManager não encontrado no CurrentPlayer!");
            return;
        }

        if (pontosTotaisObject == null)
        {
            Debug.LogWarning("pontosTotaisObject não está atribuído!");
            return;
        }

        if (pontosTotaisText == null)
        {
            Debug.LogWarning("TextMeshProUGUI não encontrado no pontosTotaisObject!");
            return;
        }

        GameManagement.AtualizaScoreInfoPlayer(playerManager, kiwiCollectableInfoText, orangeCollectableInfoText, pontosTotaisText);
    }
}
