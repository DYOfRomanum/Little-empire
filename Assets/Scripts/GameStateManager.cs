using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance {get; private set;}

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
    // Start is called before the first frame update
    void Start()
    {
        //Add this to TimeManager's listener list
        TimeManager.Instance.RegisterTracker(this);
    }
    public void saveController()
    {
        SaveManager.Save(ExportSaveState());
        Debug.Log("Game Saved");
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        throw new System.NotImplementedException();
    }

    public GameSaveState ExportSaveState()
    {
        ItemSlotData[] itemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);
        ItemSlotData[] fragmentSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Fragment);

        return new GameSaveState(itemSlots, fragmentSlots);
    }

    public void LoadSave()
    {
        //Retrieve the loaded save
        GameSaveState save = SaveManager.Load();

        //Inventory
        ItemSlotData[] itemSlots = ItemSlotData.DeserializeArray(save.itemSlots);
        ItemSlotData[] fragmentSlots = ItemSlotData.DeserializeArray(save.fragmentSlots);
        InventoryManager.Instance.LoadInventory(itemSlots,fragmentSlots);
    }

    // IEnumerator TransitionTime()
    // {
        
    // }



}
