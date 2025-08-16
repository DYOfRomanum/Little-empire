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
    public Image fragmentImage;
    public Text quantityText;

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

        // By default, the quantity text should not show
        quantityText.text = "";
        // check if there is an item to display
        if (itemToDisplay != null)
        {
            itemDisplayImage.sprite = itemToDisplay.thumbnail;
            itemBackImage.sprite = itemToDisplay.back;

            //display the stack quantity
            if (quantity>1)
            {
                quantityText.text = quantity.ToString();
            }
            if (inventoryType == InventoryType.Fragment)
            {
                fragmentImage.gameObject.SetActive(true);
            }
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
        GameStateManager.Instance.saveController();
    }

}
