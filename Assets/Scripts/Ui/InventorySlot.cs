using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    ItemData itemToDisplay;

    int quantity;

    public Image itemDisplayImage;
    public Image itemBackImage;

    public enum InventoryType
    {
        Item, Fragment
    }
    public InventoryType inventoryType;
    int slotIndex;
    // public GameObject itemInfoBox;

    public void Display(ItemSlotData itemSlot)
    {
        //set the variables accordingly
        itemToDisplay = itemSlot.itemData;
        quantity = itemSlot.quantity;
        // check if there is an item to display
        if (itemToDisplay != null)
        {
            itemDisplayImage.sprite = itemToDisplay.thumbnail;
            itemBackImage.sprite = itemToDisplay.back;

            // this.itemToDisplay = itemToDisplay;
            itemDisplayImage.gameObject.SetActive(true);
            itemBackImage.gameObject.SetActive(true);
            return;
        }
        itemDisplayImage.gameObject.SetActive(false);
        itemBackImage.gameObject.SetActive(false);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(itemToDisplay);
    }

}
