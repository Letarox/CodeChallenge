using System;

[Serializable]
public class SaveData
{
    public SerializableItem[] InventoryItems;
    public SerializableItem[] EquippedItems;

    //initializes SaveData from inventory and equipped items
    public SaveData(Item[] inventory, Item[] equippedItems)
    {
        InventoryItems = new SerializableItem[inventory.Length];
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                InventoryItems[i] = new SerializableItem(inventory[i]);
            }
        }

        EquippedItems = new SerializableItem[equippedItems.Length];
        for (int i = 0; i < equippedItems.Length; i++)
        {
            if (equippedItems[i] != null)
            {
                EquippedItems[i] = new SerializableItem(equippedItems[i]);
            }
        }
    }
}