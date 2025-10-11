using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public bool colidindoComPlayer = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManagement.CurrentPlayer.activeSelf)
        {
            colidindoComPlayer = true;
            PlayerManager pm = collision.GetComponent<PlayerManager>();
            
            if (!pm.PlayerIsTakingDamage())
            {
                pm.TakeDamage(GetComponentInParent<EnemyBase>().GetDamageValue());
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        colidindoComPlayer = false;
    }
}