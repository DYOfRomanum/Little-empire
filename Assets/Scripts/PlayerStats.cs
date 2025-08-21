using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType
{
    Coin,
    Crystal,
    MagicSpar,
    DarkCrystal,
    Mojo
    // 可以继续添加其他货币类型
}

public class PlayerStats
{
    private static Dictionary<CurrencyType, int> currencies = new Dictionary<CurrencyType, int>()
    {
        { CurrencyType.Coin, 1000 },
        { CurrencyType.Crystal, 0 },
        { CurrencyType.MagicSpar, 0 },
        { CurrencyType.DarkCrystal, 0 },
        { CurrencyType.Mojo, 0 }
    };
    // 获取特定货币的数量
    public static int GetCurrency(CurrencyType type)
    {
        return currencies[type];
    }

    public static bool Spend(CurrencyType type, int cost)
    {
        //check if the player has enough to spend
        if (cost>currencies[type])
        {
            Debug.Log("Player does not have enough resource");
            return false;
        }
        currencies[type] -= cost;
        UIManager.Instance.RenderPlayerStats();
        return true;
    }

    public static void Earn(CurrencyType type, int income)
    {
        currencies[type] += income;
        UIManager.Instance.RenderPlayerStats();
    }
    // 检查是否有足够货币
    public static bool HasEnough(CurrencyType type, int amount)
    {
        return currencies[type] >= amount;
    }
}
