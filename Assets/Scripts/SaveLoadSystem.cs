using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadSystem
{
    private static string filePath = Application.persistentDataPath + "/inventory.save";

    //save the inventory and equipped items to a file
    public static void SaveInventory(Item[] inventory, Item[] equippedItems, Item consumableItem)
    {
        //creates a new BinaryFormater and FileStream to save the data
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            //creates a new SaveData object from the current inventory and equipped items, then serializes the SaveData object to the file
            SaveData saveData = new SaveData(inventory, equippedItems, consumableItem);
            formatter.Serialize(stream, saveData);
        }

        Debug.Log("Inventory saved.");
    }

    //loads the inventory and equipped items from the file
    public static SaveData LoadInventory()
    {
        //check if there is a saved file to load from
        if (File.Exists(filePath))
        {
            //creates a new BinaryFormatter and opens the FileStream to read the data
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                //deserializes the saved data to a SaveData object
                SaveData saveData = formatter.Deserialize(stream) as SaveData;
                return saveData;
            }
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }
}
