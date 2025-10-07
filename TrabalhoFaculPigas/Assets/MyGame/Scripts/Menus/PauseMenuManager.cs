using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private SceneManagerModel sceneManager;
    [SerializeField] private GameObject canvasPauseMenu;
    [SerializeField] private PlayerManager playerManager;
    public GameObject pontosTotaisObject;
    private TextMeshProUGUI pontosTotaisText;
    public GameObject collectablesInfoObject;
    private TextMeshProUGUI collectablesInfoText;
    [SerializeField] private TextMeshProUGUI kiwiCollectableInfoText;
    [SerializeField] private TextMeshProUGUI orangeCollectableInfoText;

    void Awake()
    {
        // if (sceneManager == null)
        // {
        //     sceneManager = GameObject.Find("SceneManager").GetComponent<SceneTestManager>();
        // }
        // else sceneManager = GetComponent<SceneTestManager>();

        // if (canvasPauseMenu == null)
        // {
        //     canvasPauseMenu = GameObject.Find("CanvasPauseMenu");
        // }
        // else canvasPauseMenu = GetComponent<GameObject>();

        sceneManager = sceneManager == null ? GameObject.Find("SceneManager").GetComponent<SceneManagerModel>() : sceneManager;
        canvasPauseMenu = canvasPauseMenu == null ? GameObject.Find("CanvasPauseMenu") : canvasPauseMenu;

        pontosTotaisText = pontosTotaisObject.GetComponent<TextMeshProUGUI>();
        collectablesInfoText = collectablesInfoObject.GetComponent<TextMeshProUGUI>();


        playerManager = playerManager == null ? GameManagement.PlayerObject.GetComponent<PlayerManager>() : playerManager;

//        canvasPauseMenu.SetActive(false);
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
        SceneManager.LoadScene("MenuPrincipal");
    }

    void OnEnable()
    {
        collectablesInfoText.text = "";

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

        if (playerManager == null) return;
        
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

                    // collectablesInfoText.text += item.ToString() + "\n";
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
