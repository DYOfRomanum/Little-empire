using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set;}

    [Header("Inventory System")]
    public GameObject inventoryPanel;

    public InventorySlot[] itemSlots;
    // item info box
    public Text itemNameText;
    public Text itemDescriptionText;

    private void Awake()
    {
        // if there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            // set the static instance to this instance
            Instance = this;
        }
    }

    public void Start()
    {
        RenderInventory();
    }
    // render the inventory screen to reflect the player's inventory
    public void RenderInventory()
    {
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;
        // render the item section
        RenderInventoryPanel(inventoryItemSlots, itemSlots);
    }
    void RenderInventoryPanel(ItemData[] slots, InventorySlot[] uiSlots)
    {
        for (int i=0; i<uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
        }
    }

    public void ToggleInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        RenderInventory();

    }

    //display info
    public void DisplayItemInfo(ItemData data)
    {
        itemNameText.text = data.itemName;
        itemDescriptionText.text = data.description;
    }
}
