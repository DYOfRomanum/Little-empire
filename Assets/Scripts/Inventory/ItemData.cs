using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite thumbnail;
    public Sprite back;

    public int cost;

}
