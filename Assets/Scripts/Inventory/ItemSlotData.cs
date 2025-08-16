using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotData
{
    public ItemData itemData;
    public int quantity;

    public ItemSlotData(ItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        ValidateQuantity();
    }

    //automatically construct the class with the item data of quantity 1
    public ItemSlotData(ItemData itemData)
    {
        this.itemData = itemData;
        quantity = 1;
        ValidateQuantity();
    }
    //Stacking system
    public void AddQuantity()
    {
        AddQuantity(1);
    }
    public void AddQuantity(int amountToAdd)
    {
        quantity += amountToAdd;
    }
    public void Remove()
    {
        quantity--;
        ValidateQuantity();
    }

    private void ValidateQuantity()
    {
        if (quantity <= 0 || itemData == null)
        {
            Empty();
        }
    }
    public void Empty()
    {
        itemData = null;
        quantity = 0;
    }

    public bool IsEmpty()
    {
        return itemData == null;
    }
    //Convert ItemSlotData into ItemSlotSaveData
    public static ItemSlotSaveData SerializeData(ItemSlotData itemSlot)
    {
        return new ItemSlotSaveData(itemSlot);
    }
    //Convert an entire ItemSlotData array int an ItemSlotSaveData
    public static ItemSlotSaveData[] SerializeArray(ItemSlotData[] array)
    {
        return Array.ConvertAll(array, new Converter<ItemSlotData, ItemSlotSaveData>(SerializeData));
    }
    // Convert back into ItemSlotData
    public static ItemSlotData DeserializeData(ItemSlotSaveData itemSaveSlot)
    {
        ItemData item = InventoryManager.Instance.itemIndex.GetItemFromString(itemSaveSlot.itemID);
        return new ItemSlotData(item, itemSaveSlot.quantity);
    }
    public static ItemSlotData[] DeserializeArray(ItemSlotSaveData[] array)
    {
        return Array.ConvertAll(array, new Converter<ItemSlotSaveData, ItemSlotData>(DeserializeData));
    }
}
