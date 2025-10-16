using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool colidindoComInimigo = false;
    public bool colidindoComChao = false;
    protected static AudioSource audioHitted;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Chao"))
        {
            colidindoComChao = true;
            GameManagement.DebugLog("Colidindo com o chão");

        }
    }

    void Start()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
        {
            audioHitted = gameObject.AddComponent<AudioSource>();
            audioHitted.clip = Resources.Load<AudioClip>("Sounds/Tuc");
            audioHitted.volume = 1f;
            audioHitted.loop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            audioHitted.Stop();
            audioHitted.time = 2.2f;
            audioHitted.Play();

            // Aplica o "empurrão"
            GameManagement.CurrentPlayer.GetComponent<PlayerManager>().AplicaEmpurrao(0.65f, 0.45f);

            GameManagement.DebugLog("Player hit a enemy!");
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
