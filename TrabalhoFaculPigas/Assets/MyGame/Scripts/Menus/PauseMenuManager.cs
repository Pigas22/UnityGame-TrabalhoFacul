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
    public GameObject collectablesInfoObject;
    private TextMeshProUGUI collectablesInfoText;
    [SerializeField] private TextMeshProUGUI kiwiCollectableInfoText;
    [SerializeField] private TextMeshProUGUI orangeCollectableInfoText;
    [SerializeField] private GameObject panelConfigBackground;
    [SerializeField] private GameObject configPanel;

    void Start()
    {
        sceneManager = sceneManager == null ? GameObject.Find("SceneManager").GetComponent<SceneManagerModel>() : sceneManager;
        canvasPauseMenu = canvasPauseMenu == null ? GameObject.Find("CanvasPauseMenu") : canvasPauseMenu;

        pontosTotaisText = pontosTotaisObject.GetComponent<TextMeshProUGUI>();
        collectablesInfoText = collectablesInfoObject.GetComponent<TextMeshProUGUI>();

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
        Debug.Log("Salvando Alterações");

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
        Debug.Log("Restarting level...");
        Time.timeScale = 1f;
        sceneManager.RestartCurrentScene();
    }


    public void LoadMainMenu()
    {
        GameManagement.CurrentSkinIndex = sceneManager.currentSkinIndex;
        Debug.Log("Loading main menu...");
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
        
        collectablesInfoText.text = "";
        if (playerManager.GetTotalScore() >= 0)
        {
            var lista = playerManager.GetCollectedItensInfos();

            if (lista.Count > 0)
            {
                Debug.Log("Primeiro item : " + playerManager.GetCollectedItensInfos()[0].ToString());
                foreach (CollectedItensInfo item in lista)
                {
                    if (item.NameItem == "Kiwi")
                    {
                        kiwiCollectableInfoText.text = item.Qtd + " X " + item.Value + " = " + (item.Qtd * item.Value);
                    }
                    else if (item.NameItem == "Orange")
                    {
                        orangeCollectableInfoText.text = item.Qtd + " X " + item.Value + " = " + (item.Qtd * item.Value);
                    }
                }
            }
            else
            {
                Debug.Log("Lista : " + playerManager.GetCollectedItensInfos().ToString());
            }

            pontosTotaisText.text = "Total de Pontos: " + playerManager.GetTotalScore().ToString();
        }
        else
        {
            pontosTotaisText.text = "Total de Pontos: --";
        }
    }
}
