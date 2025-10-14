using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    [SerializeField] protected int damageValue = 1;
    [SerializeField] protected string trapName = "trap";
    [SerializeField] protected bool colidindoComPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManagement.CurrentPlayer.activeSelf)
        {
            colidindoComPlayer = true;
            
            PlayerManager pm = collision.GetComponentInParent<PlayerManager>();

            if (!pm.PlayerIsTakingDamage())
            {
                Debug.Log("Eu dei dano " + gameObject.name);
                pm.TakeDamage(damageValue);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        colidindoComPlayer = false;
    }
}