using UnityEngine;
using UnityEngine.UIElements;

public class BoxBase : MonoBehaviour
{
    [SerializeField] protected int boxHealth = 2;
    [SerializeField] protected bool isAlive;
    [SerializeField] protected string boxName = "box";
    [SerializeField] protected bool colidindoComPlayer;
    [SerializeField] protected int healthyReward = 0;
    [SerializeField] protected int[] opcoesReward = { 0 };
    private Animator animator;
    private static readonly int breakTrigger = Animator.StringToHash("BreakTrigger");
    private static readonly int hitTrigger = Animator.StringToHash("HitTrigger");

    void Awake()
    {
        animator = GetComponent<Animator>();
        isAlive = true;
    }

    public void TakeDamage(int amount)
    {
        boxHealth -= amount;
        boxHealth = Mathf.Max(boxHealth, 0);
        animator.SetTrigger(hitTrigger);
    }

    public void OnTakeDamageAnimationEnd()
    {
        Break();        
    }

    private void Break()
    {
        if (boxHealth <= 0) animator.SetTrigger(breakTrigger);
    }

    public void OnBreakAnimationEnd()
    {
        isAlive = false;
        gameObject.SetActive(false);

        var player = GameManagement.CurrentPlayer.GetComponent<PlayerManager>();

        if (healthyReward > 0) player.Heal(healthyReward);
        else if (healthyReward < 0) { player.TakeDamage(-healthyReward); Debug.Log("Eu dei dano "+ gameObject); };

        GameManagement.DebugLog($"{boxName} destruiu e aplicou recompensa: {healthyReward}");
    }

    protected void CalculateBoxReward()
    {
        healthyReward = opcoesReward[Random.Range(0, opcoesReward.Length)];
    }
    
    public int GetBoxReward() { return healthyReward; }
    public bool GetBoxIsAlive() { return isAlive; }
}