using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Fragment")]
public class FragmentData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite thumbnail;
    public Sprite back;

}