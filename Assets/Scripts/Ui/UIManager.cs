using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set;}

    [Header("Status Bar")]
    public Text timeText;

    [Header("Inventory System")]
    public GameObject inventoryPanel;
    
    public InventorySlot[] itemSlots;
    public InventorySlot[] fragmentSlots;
    // item info box
    public GameObject itemInfoBox;
    public Text itemNameText;
    public Text itemDescriptionText;
    // use and sell button
    public GameObject useButton;
    // 用于存储自动关闭的协程
    private Coroutine autoCloseCoroutine;
    [Header("Player Stats")]
    public Text coinText;
    public Text crystalText;
    public Text magicSparText;
    public Text darkCrystalText;
    public Text mojoText;

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
        RenderPlayerStats();
        // Add UImanager to the list of objects Timemanager
        TimeManager.Instance.RegisterTracker(this);
    }
    // render the inventory screen to reflect the player's inventory
    public void RenderInventory()
    {
        ItemSlotData[] inventoryItemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);
        ItemSlotData[] inventoryFragmentSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Fragment);
        // ItemData[] inventoryItemSlots = InventoryManager.Instance.items;
        // render the item section
        RenderInventoryPanel(inventoryItemSlots, itemSlots);
        RenderInventoryPanel(inventoryFragmentSlots, fragmentSlots);
    }
    void RenderInventoryPanel(ItemSlotData[] slots, InventorySlot[] uiSlots)
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
    // use button
    public void DisplayUseButton(ItemData data)
    {
        if (data!= null)
        {
            useButton.SetActive(!useButton.activeSelf);
        }    
        else
        {return;}
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

    public void ClockUpdate(GameTimestamp timestamp)
    {
        int year = timestamp.year;
        int week = timestamp.week;
        int day = timestamp.day;
        int hour = timestamp.hour;
        int minute = timestamp.minute;
        int second = timestamp.second;

        timeText.text = year.ToString("0000")+" "+week+" "+day+" "+hour.ToString("00")+":"+minute.ToString("00")+":"+second.ToString("00");
        //Handle the date
        // int day = timestamp.day;
    }

    //Render the UI of the player stats
    public void RenderPlayerStats()
    {
        coinText.text = PlayerStats.GetCurrency(CurrencyType.Coin).ToString();
        crystalText.text = PlayerStats.GetCurrency(CurrencyType.Crystal).ToString();
        magicSparText.text = PlayerStats.GetCurrency(CurrencyType.MagicSpar).ToString();
        darkCrystalText.text = PlayerStats.GetCurrency(CurrencyType.DarkCrystal).ToString();
        mojoText.text = PlayerStats.GetCurrency(CurrencyType.Mojo).ToString();
    }
}
