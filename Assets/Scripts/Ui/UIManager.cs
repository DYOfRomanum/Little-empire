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
    public GameObject itemInfoBox;
    public Text itemNameText;
    public Text itemDescriptionText;
    // 用于存储自动关闭的协程
    private Coroutine autoCloseCoroutine;

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
        if (data!= null)
        {
            itemInfoBox.SetActive(!itemInfoBox.activeSelf);
            itemNameText.text = data.itemName;
            itemDescriptionText.text = data.description; 
        }    
        else
        {return;}

        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
        }
        
        // 启动新的自动关闭协程
        autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
    }

    private IEnumerator AutoCloseAfterDelay()
    {
        // 等待2秒
        yield return new WaitForSeconds(2f);
        
        // 关闭信息窗口
        itemInfoBox.SetActive(false);
    }
}
