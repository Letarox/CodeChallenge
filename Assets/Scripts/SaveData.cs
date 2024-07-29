using System;

[Serializable]
public class SaveData
{
    public SerializableItem[] InventoryItems;
    public SerializableItem[] EquippedItems;
    public SerializableItem ConsumableItem;

    //initializes SaveData from inventory and equipped items
    public SaveData(Item[] inventory, Item[] equippedItems, Item consumableItem)
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

        if(consumableItem != null)
            ConsumableItem = new SerializableItem(consumableItem);
    }
}