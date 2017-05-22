using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "Scriptable Object/ItemList")]
public class ItemList : ScriptableObject
{
    public List<ItemData> listItemData;
}