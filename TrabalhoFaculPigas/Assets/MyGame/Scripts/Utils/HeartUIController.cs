using UnityEngine;
using UnityEngine.UI;

public class HeartUIController : MonoBehaviour
{
    [Header("Sprite Settings (0=Vazio, 1=Metade, 2=Cheio)")]
    public Sprite[] heartSprites; 
    
    [Header("Heart Images (Ordem: 1, 2, 3)")]
    public Image[] heartImages;

    private const int HP_PER_HEART = 2; // Cada coração representa 2 de HP
    
    // Referência ao seu PlayerManager (que herda de CharacterBase)
    private PlayerManager playerHealthComponent; 

    void Start()
    {
        // Tenta encontrar o player na cena para obter o componente CharacterBase
        playerHealthComponent = GameManagement.CurrentPlayer.GetComponent<PlayerManager>(); 

        if (playerHealthComponent != null)
        {
            // INSCREVE-SE no evento de mudança de vida!
            playerHealthComponent.OnHealthChanged += UpdateHealthDisplay;
        }
        else
        {
            Debug.LogError("PlayerManager (CharacterBase) não encontrado na cena!");
            return;
        }

        // Garante que o HUD seja iniciado com a vida atual
        UpdateHealthDisplay(playerHealthComponent.GetCurrentHealth(), playerHealthComponent.IsAlive());
    }

    void OnDisable()
    {
        // CRUCIAL: Desinscreve-se para evitar erros quando o objeto for destruído
        if (playerHealthComponent != null)
        {
            playerHealthComponent.OnHealthChanged -= UpdateHealthDisplay;
        }
    }

    // Este método é chamado automaticamente pelo evento OnHealthChanged
    public void UpdateHealthDisplay(int currentHealth, bool isAlive) 
    {
        // Itera sobre cada coração no HUD
        for (int i = 0; i < heartImages.Length; i++)
        {
            // 1. Calcula o valor de HP que este coração representa (0, 1 ou 2)
            int heartValue = currentHealth - (i * HP_PER_HEART);

            // 2. Limita o valor entre 0 (Vazio) e 2 (Cheio) para o índice do Sprite
            int spriteIndex = Mathf.Clamp(heartValue, 0, HP_PER_HEART);

            // 3. Aplica o sprite
            if (i < heartImages.Length && spriteIndex < heartSprites.Length)
            {
                 heartImages[i].sprite = heartSprites[spriteIndex];
            }
        }
    }
}