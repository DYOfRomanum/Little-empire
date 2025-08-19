using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public static int Coin {get; private set;}

    public static void Spend(int cost)
    {
        //check if the player has enough to spend
        if (cost>Coin)
        {
            Debug.Log("Player does not have enough resource");
            return;
        }
        Coin -= cost;
        UIManager.Instance.RenderPlayerStats();
    }

    public static void Earn(int income)
    {
        Coin += income;
        UIManager.Instance.RenderPlayerStats();
    }
}
