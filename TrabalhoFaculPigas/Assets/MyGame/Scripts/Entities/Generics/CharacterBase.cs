using UnityEngine;
using System;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 2; // NOVO: Defina um maxHealth
    [SerializeField] protected int currentHealth = 2; // RENOMEADO: Use 'health' para HP
    [SerializeField] protected bool isAlive = true;

    void Awake()
    {
        InitConfigHPChar(2);
    }

    public virtual void TakeDamage(int amount)
    {
        if (!isAlive) return;
        
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        GameManagement.DebugLog($"{gameObject.name} took {amount} damage. Health left: {currentHealth}");
    }

    public virtual void Die()
    {
        if (currentHealth <= 0)
        {
            isAlive = false;
            gameObject.SetActive(false);
            //Destroy(gameObject);
            GameManagement.DebugLog($"{gameObject.name} died.");
        }
    }
    
    // NOVO: Método para curar
    public virtual void Heal(int amount)
    {
        if (!isAlive) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Garante que não exceda o HP máximo
        
        GameManagement.DebugLog($"{gameObject.name} healed {amount}. Health: {currentHealth}");
    }
    
    protected void InitConfigHPChar(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }
    public int GetCurrentHealth() { return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }
    public bool IsAlive() { return isAlive; }

    public void CharacterStats()
    {
        GameManagement.DebugLog(gameObject.name + " Max HP: " + maxHealth);
        GameManagement.DebugLog(gameObject.name + " Current HP: " + currentHealth);
        GameManagement.DebugLog(gameObject.name + " Position: " + transform.position);
    }
}