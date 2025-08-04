using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equipment")]
public class EquipmentData : ItemData 
{
    public enum ToolType
    {
        Wrist, Body, Shoulder, Head, Weapon, Ring, Relics
    }
    public ToolType toolType;

}
