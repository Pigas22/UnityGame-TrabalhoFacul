using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private SceneTestManager sceneManager;
    [SerializeField] private GameObject canvasPauseMenu;

    private PlayerManager playerManager;
    public GameObject pontosTotaisObject;
    private TextMeshProUGUI pontosTotaisText;
    public GameObject collectablesInfoObject;
    private TextMeshProUGUI collectablesInfoText;

    void Awake()
    {
        if (sceneManager == null)
        {
            sceneManager = GameObject.Find("SceneManager").GetComponent<SceneTestManager>();
        }
        else sceneManager = GetComponent<SceneTestManager>();

        if (canvasPauseMenu == null)
        {
            canvasPauseMenu = GameObject.Find("CanvasPauseMenu");
        }
        else canvasPauseMenu = GetComponent<GameObject>();

        playerManager = GameManagement.PlayerObject.GetComponent<PlayerManager>();
        pontosTotaisText = pontosTotaisObject.GetComponent<TextMeshProUGUI>();
        collectablesInfoText = collectablesInfoObject.GetComponent<TextMeshProUGUI>();

        canvasPauseMenu.SetActive(false);
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
    }

    public void OpenSettings()
    {
        Debug.Log("Settings menu opened.");
        // Implement settings menu logic here
    }


    public void RestartLevel()
    {
        Debug.Log("Restarting level...");
        Time.timeScale = 1f;
        sceneManager.RestartCurrentScene();
    }


    public void LoadMainMenu()
    {
        Debug.Log("Loading main menu...");
        // Time.timeScale = 1f; // Assegura que o tempo do jogo está normal antes de carregar o menu principal
        // var sceneManager = FindObjectOfType<SceneTestManager>();
        // if (sceneManager != null)
        // {
        //     sceneManager.LoadMainMenuScene(); // Carrega a cena do menu principal
        // }
        // else
        // {
        //     Debug.LogError("SceneTestManager not found in the scene.");
        // }
    }

    void OnEnable()
    {
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

        if (playerManager.GetTotalScore() >= 0)
        {
            var lista = playerManager.GetCollectedItensInfos();

            if (lista.Count > 0)
            {
                Debug.Log("Primeiro item : " + playerManager.GetCollectedItensInfos()[0].ToString());
                foreach (CollectedItensInfo item in lista)
                {
                    collectablesInfoText.text += item.ToString() + "\n";
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
