using UnityEngine;

public class CharacterBase : MonoBehaviour, ICharacterBase
{
    [SerializeField] protected int lives = 1;
    [SerializeField] protected bool isAlive = true;

    public virtual void TakeDamage(int amount)
    {
        lives -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Lives left: {lives}");

        Die();
    }

    public virtual void Die()
    {
        if (lives <= 0)
        {
            isAlive = false;
            gameObject.SetActive(false);
            Debug.Log($"{gameObject.name} died.");
        }
    }
}