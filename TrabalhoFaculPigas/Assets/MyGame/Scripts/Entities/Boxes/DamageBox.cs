using UnityEngine;
using UnityEngine.UIElements;

public class DamageBox : BoxBase
{
    void Start()
    {
        boxName = "DamageBox";
        boxHealth = 2;
        opcoesReward = new int[] { -1, -2 };
        CalculateBoxReward();
    }
}