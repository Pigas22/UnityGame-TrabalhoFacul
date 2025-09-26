using System.Xml.Serialization;
using UnityEngine;

public class KiwiFruit : CollectibleBase
{
    void Start()
    {
        collectibleName = "Kiwi";
        scoreValue = 5;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollect(collision.gameObject);
            Destroy(gameObject);
        }
    }
}