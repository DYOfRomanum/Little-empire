using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance {get; private set; }

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

    //The full list of items
    public ItemIndex itemIndex;
    // Item slots
    [SerializeField] private ItemSlotData[] itemSlots = new ItemSlotData[30];
    // Equipment slots
    [SerializeField] private ItemSlotData[] equipments = new ItemSlotData[16];
    [SerializeField] private ItemSlotData[] equipped = new ItemSlotData[8];
    // Spell slots
    [SerializeField] private ItemSlotData[] spellSlot = new ItemSlotData[16];
    [SerializeField] private ItemSlotData[] equippedSpell = new ItemSlotData[4];
    // Fragment slots
    [SerializeField] private ItemSlotData[] fragmentSlots = new ItemSlotData[30];

    // Load the inventory items from a save
    public void LoadInventory(ItemSlotData[] itemSlots, ItemSlotData[] fragmentSlots)
    {
        this.itemSlots = itemSlots;
        this.fragmentSlots = fragmentSlots;

        UIManager.Instance.RenderInventory();
    }
    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return itemSlots;
        }
        else if (inventoryType == InventorySlot.InventoryType.Fragment)
        {
            return fragmentSlots;
        }
        else{return equipments;}
    }
    // when giving the itemData value in the inspector, automatically set the quantity to 1
    private void OnValidate()
    {
        ValidateInventorySlots(itemSlots);
        ValidateInventorySlots(fragmentSlots);
    }
    void ValidateInventorySlot(ItemSlotData slot)
    {
        if (slot.itemData != null && slot.quantity == 0)
        {
            slot.quantity = 1;
        }
    }
    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach (ItemSlotData slot in array)
        {
            ValidateInventorySlot(slot);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
