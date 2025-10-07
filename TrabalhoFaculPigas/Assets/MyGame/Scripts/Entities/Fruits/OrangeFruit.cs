using System.Xml.Serialization;
using UnityEngine;

public class OrangeFruit : CollectibleBase
{

    void Awake()
    {
        collectibleName = "Orange";
        scoreValue = 7;
        animator = GetComponent<Animator>();
    }
}