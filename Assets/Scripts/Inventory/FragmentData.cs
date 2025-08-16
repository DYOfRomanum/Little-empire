using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Fragment")]
public class FragmentData : ItemData
{
    public string fragmentName;
    public string descriptionComposite;
    public Sprite fragmentThumbnail;
    public Sprite fragmentBack;
    public int numToCompose;

}