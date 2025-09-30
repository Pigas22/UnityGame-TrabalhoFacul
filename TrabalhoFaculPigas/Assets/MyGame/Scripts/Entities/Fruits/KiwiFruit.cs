using System.Xml.Serialization;
using UnityEngine;

public class KiwiFruit : CollectibleBase
{
    private int collectingHash = Animator.StringToHash("collecting");

    void Start()
    {
        collectibleName = "Kiwi";
        scoreValue = 5;
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollect(collision.gameObject);
            animator.SetBool(collectingHash, true);
        }
    }

    void OnCollectAnimationEnd() {
        animator.SetBool(collectingHash, false);
        Destroy(gameObject);   
    }
}