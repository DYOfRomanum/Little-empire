using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotSaveData
{
    public string itemID;
    public int quantity;
    //convert ItemSlotData into a serializable format
    public ItemSlotSaveData(ItemSlotData data)
    {
        if (data.IsEmpty())
        {
            itemID = null;
            quantity = 0;
            return;
        }
        //Copy over the values
        itemID = data.itemData.name;
        quantity = data.quantity;
    }
}
