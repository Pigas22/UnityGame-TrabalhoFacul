using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private SceneMenuPrincipalManager sceneManager;
    [SerializeField] private GameObject panelSubMenus;
    [SerializeField] private GameObject seletorNiveisPanel;

    [SerializeField] private GameObject alterarSkinPanel;
    [SerializeField] private GameObject configPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private string nomeDoLevelDoJogo;
    [SerializeField] private bool isDebugging = false;

    void Awake()
    {
        FecharTodosMenus();
    }

    // Jogar
    public void OpenLevelSelectorMenu() { }
    public void Jogar() { SceneManager.LoadScene(nomeDoLevelDoJogo); }
    public void CloseLevelSelectorMenu() { }


    // Skin Changer Menu
    public void OpenSkinChangerMenu()
    {
        DebugIsOpenMenu(alterarSkinPanel, false);
        ActivatePanelSubMenus();
        alterarSkinPanel.SetActive(true);
    }
    public void PreviousSkin() { }
    public void NextSkin() { }
    public void CloseSkinChangerMenu()
    {
        DebugIsOpenMenu(alterarSkinPanel, false);
        alterarSkinPanel.SetActive(false);
        DeActivatePanelSubMenus();
    }

    // Settings Menu
    public void OpenSettingsMenu()
    {
        DebugIsOpenMenu(configPanel, true);
        ActivatePanelSubMenus();
        configPanel.SetActive(true);
    }
    public void SaveSettings()
    {
        Debug.Log("Salvando Alterações");
        configPanel.SetActive(false);
        DeActivatePanelSubMenus();
    }
    public void CloseSettingsMenu()
    {
        DebugIsOpenMenu(configPanel, false);
        configPanel.SetActive(false);
        DeActivatePanelSubMenus();
    }

    // Credits Button
    public void OpenCreditsMenu()
    {
        FecharTodosMenus();
        DebugIsOpenMenu(creditsPanel, true);
        ActivatePanelSubMenus();
        creditsPanel.SetActive(true);
    }
    public void CloseCreditsMenu()
    {
        DebugIsOpenMenu(creditsPanel, false);
        creditsPanel.SetActive(false);
        DeActivatePanelSubMenus();
    }

    // Info Menu (Abrir GitHub do projeto)
    public void OpenLinkInfo() {
        DebugIsOpenMenu(new GameObject(name = "LinkGitHub"), true);
        Application.OpenURL("https://github.com/Pigas22/UnityGame-TrabalhoFacul"); 
    }

    // Sair
    public void Quit()
    {
        DebugIsOpenMenu(new GameObject(name = "Application"), false);
        Application.Quit();
    }


    private void FecharTodosMenus()
    {
        DebugIsOpenMenu(new GameObject(name = "Todos"), false);
        panelSubMenus.SetActive(false);
        alterarSkinPanel.SetActive(false);
        configPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // Ativa o panel e o background utilizado em todas as sub telas
    private void ActivatePanelSubMenus()
    {
        panelSubMenus.SetActive(true);
    }
    private void DeActivatePanelSubMenus()
    {
        panelSubMenus.SetActive(false);
    }

    // Mostra no console qual objeto está abrindo ou fechando
    private void DebugIsOpenMenu(GameObject obj, bool isOpenning)
    {
        if (isOpenning) DebugMenusMsg("Abrindo", obj);
        else DebugMenusMsg("Fechando", obj);
    }
    private void DebugMenusMsg(string msg, GameObject obj)
    {
        if (isDebugging) Debug.Log(msg + " " + obj);
    }
}
