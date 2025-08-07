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

    // Item slots
    public ItemSlotData[] itemSlots = new ItemSlotData[30];
    // Equipment slots
    public ItemSlotData[] equipments = new ItemSlotData[16];
    public ItemSlotData[] equipped = new ItemSlotData[8];
    // Spell slots
    public ItemSlotData[] spellSlot = new ItemSlotData[16];
    public ItemSlotData[] equippedSpell = new ItemSlotData[4];
    // Fragment slots
    public ItemSlotData[] fragmentSlots = new ItemSlotData[30];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
