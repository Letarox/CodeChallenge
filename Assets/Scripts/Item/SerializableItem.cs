using System;
using UnityEngine;

[Serializable]
public class SerializableItem
{
    public string itemName;
    public string description;
    public string iconPath; // Save path to the icon resource
    public ItemType equipmentType;

    public SerializableItem(Item item)
    {
        itemName = item.Name;
        description = item.Description;
        iconPath = item.Icon != null ? $"Icons/{item.Icon.name}" : null; // Assuming the icons are in Resources/Icons/
        equipmentType = item.ItemType;
    }

    //converts SerializableItem back to an Item
    public Item ToItem()
    {
        Item newItem = ScriptableObject.CreateInstance<Item>();
        newItem.SetData(this);
        return newItem;
    }
}