using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool colidindoComInimigo = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Player hit a enemy!");
            colidindoComInimigo = true;
            // Aqui você pode adicionar lógica para reduzir a vida do jogador, tocar um som, etc.
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
    }
}
