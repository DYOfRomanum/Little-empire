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
    public ItemData[] items = new ItemData[30];
    // Equipment slots
    public ItemData[] equipments = new ItemData[16];
    public ItemData[] equipped = new ItemData[8];
    // Spell slots
    public ItemData[] spells = new ItemData[16];
    public ItemData[] equippedSpell = new ItemData[4];
    // Fragment slots
    public ItemData[] fragments = new ItemData[30];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
