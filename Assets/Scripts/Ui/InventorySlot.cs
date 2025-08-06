using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler
{
    ItemData itemToDisplay;

    public Image itemDisplayImage;
    public Image itemBackImage;
    // public GameObject itemInfoBox;

    public void Display(ItemData itemToDisplay)
    {
        // check if there is an item to display
        if (itemToDisplay != null)
        {
            itemDisplayImage.sprite = itemToDisplay.thumbnail;
            itemBackImage.sprite = itemToDisplay.back;

            this.itemToDisplay = itemToDisplay;
            itemDisplayImage.gameObject.SetActive(true);
            itemBackImage.gameObject.SetActive(true);
            return;
        }
        itemDisplayImage.gameObject.SetActive(false);
        itemBackImage.gameObject.SetActive(false);

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // itemInfoBox.SetActive(!itemInfoBox.activeSelf);
        UIManager.Instance.DisplayItemInfo(itemToDisplay);
    }
}
