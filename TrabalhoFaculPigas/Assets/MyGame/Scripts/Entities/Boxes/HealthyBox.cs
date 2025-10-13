using UnityEngine;
using UnityEngine.UIElements;

public class HealthyBox : BoxBase
{
    void Start()
    {
        boxName = "HealthBox";
        boxHealth = 3;
        opcoesReward = new int[]{ 1, 2, 3 };
        CalculateBoxReward();
    }
}