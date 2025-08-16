using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveState : MonoBehaviour
{
    // Inventory
    public ItemSlotSaveData[] itemSlots;
    public ItemSlotSaveData[] fragmentSlots;

    // public GameTimestamp timestamp;


    public GameSaveState(ItemSlotData[] itemSlots, ItemSlotData[] fragmentSlots)
    {
        this.itemSlots = ItemSlotData.SerializeArray(itemSlots);
        this.fragmentSlots = ItemSlotData.SerializeArray(fragmentSlots);
    }
}
