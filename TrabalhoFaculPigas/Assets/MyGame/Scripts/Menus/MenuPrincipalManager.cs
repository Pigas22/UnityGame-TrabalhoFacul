using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private GameObject panelSubMenus;
    [SerializeField] private GameObject seletorNiveisPanel;
    [SerializeField] private GameObject[] niveisDoJogo;
    [SerializeField] private int indexFaseAtual = 0;
    [SerializeField] private GameObject alterarSkinPanel;
    [SerializeField] private GameObject configPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private bool isDebugging = false;

    void Awake()
    {
        FecharTodosMenus();
    }

    // Jogar
    public void OpenLevelSelectorMenu()
    {
        DebugIsOpenMenu(seletorNiveisPanel, false);
        ActivatePanelSubMenus();
        seletorNiveisPanel.SetActive(true);
        UpdateLevelPlayButton();
    }

    public void NextLevel() // IA Version
    {
        // Primeiro, atualiza o index da fase atual
        int previousIndex = GetPreviousIndex(indexFaseAtual);
        int nextIndex = GetNextIndex(indexFaseAtual);

        // -----------------------------------------------------------
        // Inversão da lógica de posição dos GameObjects
        RectTransform rtPrev = niveisDoJogo[previousIndex].GetComponent<RectTransform>(); 
        RectTransform rtNext = niveisDoJogo[nextIndex].GetComponent<RectTransform>();
        RectTransform rtAtual = niveisDoJogo[indexFaseAtual].GetComponent<RectTransform>();

        Vector2 posPrev = rtPrev.anchoredPosition;
        Vector2 posNext = rtNext.anchoredPosition;
        Vector2 posAtual = rtAtual.anchoredPosition;

        // Altera a posição das fases para o movimento de 'voltar'
        rtPrev.anchoredPosition = posNext;
        rtNext.anchoredPosition = posAtual;
        rtAtual.anchoredPosition = posPrev;

        // -----------------------------------------------------------
        Button buttonChildPrev = niveisDoJogo[previousIndex].GetComponentInChildren<Button>();
        Button buttonChildNext = niveisDoJogo[nextIndex].GetComponentInChildren<Button>();
        Button buttonChildAtual = niveisDoJogo[indexFaseAtual].GetComponentInChildren<Button>();

        // -----------------------------------------------------------
        // Inversão da lógica de escala dos botões
        RectTransform rtChildPrev = buttonChildPrev.GetComponentInChildren<RectTransform>();
        RectTransform rtChildNext = buttonChildNext.GetComponentInChildren<RectTransform>();
        RectTransform rtChildAtual = buttonChildAtual.GetComponentInChildren<RectTransform>();

        Vector2 deltaChildPrev = rtChildPrev.sizeDelta;
        Vector2 deltaChildNext = rtChildNext.sizeDelta;
        Vector2 deltaChildAtual = rtChildAtual.sizeDelta;

        // Altera a escala dos botões para o movimento de 'voltar'
        rtChildPrev.sizeDelta = deltaChildNext;
        rtChildNext.sizeDelta = deltaChildAtual;
        rtChildAtual.sizeDelta = deltaChildPrev;

        // -----------------------------------------------------------
        // Inversão da lógica de opacidade das imagens
        RawImage imgChildPrev = buttonChildPrev.GetComponentInChildren<RawImage>();
        RawImage imgChildNext = buttonChildNext.GetComponentInChildren<RawImage>();
        RawImage imgChildAtual = buttonChildAtual.GetComponentInChildren<RawImage>();

        float opacidadeAtual = 1f;
        float opacidadeOutros = 0.5f;

        Color corImgChildPrev = imgChildPrev.color;
        Color corImgChildNext = imgChildNext.color;
        Color corImgChildAtual = imgChildAtual.color;

        // Altera a opacidade das imagens para o movimento de 'voltar'
        imgChildPrev.color = new Color(corImgChildPrev.r, corImgChildPrev.g, corImgChildPrev.b, opacidadeOutros);
        imgChildNext.color = new Color(corImgChildNext.r, corImgChildNext.g, corImgChildNext.b, opacidadeAtual);
        imgChildAtual.color = new Color(corImgChildAtual.r, corImgChildAtual.g, corImgChildAtual.b, opacidadeOutros);

        // -----------------------------------------------------------
        // Inversão da lógica de tamanho da fonte
        TextMeshProUGUI txtChildButtonPrev = buttonChildPrev.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI txtChildButtonNext = buttonChildNext.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI txtChildButtonAtual = buttonChildAtual.GetComponentInChildren<TextMeshProUGUI>();

        float tamanhoFonteAtual = 47.9f;
        float tamanhoFonteOutros = 32.6f;

        // Altera o tamanho da fonte para o movimento de 'voltar'
        txtChildButtonPrev.fontSize = tamanhoFonteOutros;
        txtChildButtonNext.fontSize = tamanhoFonteAtual;
        txtChildButtonAtual.fontSize = tamanhoFonteOutros;

        // -----------------------------------------------------------
        // Inversão da lógica de posição do texto
        RectTransform rtTextChildButtonPrev = txtChildButtonPrev.gameObject.GetComponent<RectTransform>();
        RectTransform rtTextChildButtonNext = txtChildButtonNext.gameObject.GetComponent<RectTransform>();
        RectTransform rtTextChildButtonAtual = txtChildButtonAtual.gameObject.GetComponent<RectTransform>();

        Vector2 posTextChildButtonPrev = rtTextChildButtonPrev.localPosition;
        Vector2 posTextChildButtonNext = rtTextChildButtonNext.localPosition;
        Vector2 posTextChildButtonAtual = rtTextChildButtonAtual.localPosition;

        // Altera a posição do texto para o movimento de 'voltar'
        rtTextChildButtonPrev.localPosition = posTextChildButtonNext;
        rtTextChildButtonNext.localPosition = posTextChildButtonAtual;
        rtTextChildButtonAtual.localPosition = posTextChildButtonPrev;

        // -----------------------------------------------------------
        // Atualiza o índice da fase
        indexFaseAtual = nextIndex;
        UpdateLevelPlayButton();
    }
    public void PreviousLevel() // Dev Version
    {
        int previousIndex = GetPreviousIndex(indexFaseAtual);
        int nextIndex = GetNextIndex(indexFaseAtual);

        // -----------------------------------------------------------
        // Pega a posição do GameObject das fases
        RectTransform rtPrev = niveisDoJogo[previousIndex].GetComponent<RectTransform>();  // posicao fase anterior
        RectTransform rtNext = niveisDoJogo[nextIndex].GetComponent<RectTransform>();      // posicao prox fase
        RectTransform rtAtual = niveisDoJogo[indexFaseAtual].GetComponent<RectTransform>(); // posicao fase atual

        Vector2 posPrev = rtPrev.anchoredPosition;
        Vector2 posNext = rtNext.anchoredPosition;
        Vector2 posAtual = rtAtual.anchoredPosition;

        // alteração da posição das fases
        rtPrev.anchoredPosition = posAtual;   // fase anterior -> fase atual
        rtNext.anchoredPosition = posPrev;    // prox fase     -> fase anterior
        rtAtual.anchoredPosition = posNext;    // fase atual    -> fase prox 
        // -----------------------------------------------------------
        Button buttonChildPrev = niveisDoJogo[previousIndex].GetComponentInChildren<Button>();
        Button buttonChildNext = niveisDoJogo[nextIndex].GetComponentInChildren<Button>();
        Button buttonChildAtual = niveisDoJogo[indexFaseAtual].GetComponentInChildren<Button>();

        // -----------------------------------------------------------
        // Alteração da escala dos botões
        RectTransform rtChildPrev = buttonChildPrev.GetComponentInChildren<RectTransform>();  // scala do botao filho da fase anterior
        RectTransform rtChildNext = buttonChildNext.GetComponentInChildren<RectTransform>();  // scala do botao filho da prox fase
        RectTransform rtChildAtual = buttonChildAtual.GetComponentInChildren<RectTransform>(); // scala do botao filho da fase atual

        Vector2 deltaChildPrev = new(rtChildPrev.rect.width, rtChildPrev.rect.height);
        Vector2 deltaChildNext = new(rtChildNext.rect.width, rtChildNext.rect.height);
        Vector2 deltaChildAtual = new(rtChildAtual.rect.width, rtChildAtual.rect.height);

        // alteração da scala do botões das fases
        rtChildPrev.sizeDelta = deltaChildAtual;   // fase anterior -> fase atual
        rtChildNext.sizeDelta = deltaChildPrev;    // prox fase     -> fase anterior
        rtChildAtual.sizeDelta = deltaChildNext;    // fase atual    -> fase prox 
        // -----------------------------------------------------------
        // Corrige a opacidade da imagem dos botões.
        RawImage imgChildPrev = buttonChildPrev.GetComponentInChildren<RawImage>();  // imagem do botao filho da fase anterior
        RawImage imgChildNext = buttonChildNext.GetComponentInChildren<RawImage>();  // imagem do botao filho da fase atual
        RawImage imgChildAtual = buttonChildAtual.GetComponentInChildren<RawImage>(); // imagem do botao filho da fase atual

        float opacidadeAtual = 1f;
        float opacidadeOutros = 0.5f;

        Color corImgChildPrev = new(imgChildPrev.color.r, imgChildPrev.color.g, imgChildPrev.color.b, opacidadeOutros);
        Color corImgChildNext = new(imgChildNext.color.r, imgChildNext.color.g, imgChildNext.color.b, opacidadeOutros);
        Color corImgChildAtual = new(imgChildAtual.color.r, imgChildAtual.color.g, imgChildAtual.color.b, opacidadeAtual);

        // alteração da opacidade das imagens dos botões
        imgChildPrev.color = corImgChildAtual; // fase anterior -> fase atual
        imgChildNext.color = corImgChildPrev;  // prox fase     -> fase anterior
        imgChildAtual.color = corImgChildNext;  // fase atual    -> fase prox 
        // -----------------------------------------------------------
        // alteração do tamanho da fonte
        TextMeshProUGUI txtChildButtonPrev = buttonChildPrev.GetComponentInChildren<TextMeshProUGUI>();  // texto do botao filho da fase anterior
        TextMeshProUGUI txtChildButtonNext = buttonChildNext.GetComponentInChildren<TextMeshProUGUI>();  // texto do botao filho da prox fase
        TextMeshProUGUI txtChildButtonAtual = buttonChildAtual.GetComponentInChildren<TextMeshProUGUI>(); // texto do botao filho da fase atual

        float tamanhoFonteAtual = 47.9f;
        float tamanhoFonteOutros = 32.6f;

        // alteração do tamanho da fonte dos textos
        txtChildButtonPrev.fontSize = tamanhoFonteAtual;     // fase anterior -> fase atual
        txtChildButtonNext.fontSize = tamanhoFonteOutros;    // prox fase     -> fase anterior
        txtChildButtonAtual.fontSize = tamanhoFonteOutros;    // fase atual    -> fase prox 
        // -----------------------------------------------------------
        // alteração da posição do texto
        RectTransform rtTextChildButtonPrev = txtChildButtonPrev.gameObject.GetComponent<RectTransform>();  // posicao botao filho da fase anterior
        RectTransform rtTextChildButtonNext = txtChildButtonNext.gameObject.GetComponent<RectTransform>();  // posicao botao filho da prox fase
        RectTransform rtTextChildButtonAtual = txtChildButtonAtual.gameObject.GetComponent<RectTransform>(); // posicao botao filho da fase atual

        Vector2 posTextChildButtonPrev = rtTextChildButtonPrev.localPosition;
        Vector2 posTextChildButtonNext = rtTextChildButtonNext.localPosition;
        Vector2 posTextChildButtonAtual = rtTextChildButtonAtual.localPosition;

        // alteração da posição do texto dos botões
        rtTextChildButtonPrev.localPosition = posTextChildButtonAtual;  // fase anterior -> fase atual
        rtTextChildButtonNext.localPosition = posTextChildButtonPrev;   // prox fase     -> fase anterior
        rtTextChildButtonAtual.localPosition = posTextChildButtonNext;   // fase atual    -> fase prox 
        // -----------------------------------------------------------

        // atualiza a variavel
        indexFaseAtual = previousIndex;
        UpdateLevelPlayButton();
    }
    private void UpdateLevelPlayButton()
    {
        int previousIndex = GetPreviousIndex(indexFaseAtual);
        int nextIndex = GetNextIndex(indexFaseAtual);

        // Deixa somente o Bottao da Fase Selecionada Ativo
        niveisDoJogo[previousIndex].GetComponentInChildren<Button>().enabled = false;
        niveisDoJogo[indexFaseAtual].GetComponentInChildren<Button>().enabled = true;
        niveisDoJogo[nextIndex].GetComponentInChildren<Button>().enabled = false;
    }
    public void Jogar() { SceneManager.LoadScene(niveisDoJogo[indexFaseAtual].name); }
    public void CloseLevelSelectorMenu()
    { 
        DebugIsOpenMenu(seletorNiveisPanel, false);
        seletorNiveisPanel.SetActive(false);
        DeActivatePanelSubMenus();
    }
    private int GetNextIndex(int indexRef) {
        return (indexRef + 1) % niveisDoJogo.Length;
    }
    private int GetPreviousIndex(int indexRef) {
        return  (indexRef - 1 + niveisDoJogo.Length) % niveisDoJogo.Length;
    }

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
        DebugIsOpenMenu("Todos", false);
        panelSubMenus.SetActive(false);
        seletorNiveisPanel.SetActive(false);
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
    private void DebugIsOpenMenu(string name, bool isOpenning)
    {
        if (isOpenning) Debug.Log("Abrindo: " + name);
        else Debug.Log("Fechando: " + name);
    }
    private void DebugIsOpenMenu(GameObject obj, bool isOpenning)
    {
        if (isOpenning) Debug.Log("Abrindo: " + obj.name);
        else Debug.Log("Fechando: " + obj.name);
    }
}
