using System.Xml.Serialization;
using UnityEngine;

public class KiwiFruit : CollectibleBase
{

    void Awake()
    {
        collectibleName = "Kiwi";
        scoreValue = 5;
        animator = GetComponent<Animator>();
    }
}