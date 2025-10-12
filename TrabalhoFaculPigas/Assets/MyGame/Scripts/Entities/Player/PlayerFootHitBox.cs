using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public bool colidindoComInimigo = false;
    [SerializeField] public bool colidindoComChao = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chao"))
        {
            colidindoComChao = true;
            Debug.Log("Colidindo com o chão");

        }

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Player hit a enemy!");
            colidindoComInimigo = true;

            collision.GetComponent<EnemyBase>().TakeDamage(1);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        colidindoComInimigo = false;
        colidindoComChao = false;
    }
}
