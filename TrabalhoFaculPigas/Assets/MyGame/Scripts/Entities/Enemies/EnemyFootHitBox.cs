using UnityEngine;

public class EnemyFootHitBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public bool colidindoComChao = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chao"))
        {
            colidindoComChao = true;
        } 
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Chao"))
        {
            colidindoComChao = false;
        } 
    }
}