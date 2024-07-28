using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Singleton
    private static InventoryManager _instance;

    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(typeof(InventoryManager).ToString() + " is NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Keep the singleton alive across scenes
            LoadInventory();
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
    #endregion

    [SerializeField] private Item[] _inventory = new Item[10]; // Adjust size as needed
    [SerializeField] private Item[] _equippedItems = new Item[2]; // Assuming two slots for equipped items
    [SerializeField] private GameObject _itemOverworldPrefab;
    private float spawnRadius = 1.5f;
    private int cachedIndex = -1;

    private Transform _playerTransform;

    public Item[] Inventory => _inventory;
    public Item[] EquippedItems => _equippedItems;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnApplicationQuit()
    {
        SaveInventory();
    }

    public void SaveInventory()
    {
        SaveLoadSystem.SaveInventory(_inventory, _equippedItems);
    }

    public void LoadInventory()
    {
        SaveData saveData = SaveLoadSystem.LoadInventory();

        if (saveData == null)
        {
            //no save data found, uses default inventory
            return;
        }

        //if save data was found, load the inventory
        for (int i = 0; i < _inventory.Length; i++)
        {
            if (saveData.InventoryItems != null && saveData.InventoryItems.Length > i && saveData.InventoryItems[i] != null)
            {
                _inventory[i] = saveData.InventoryItems[i].ToItem();
            }
            else
            {
                _inventory[i] = null;
            }
        }

        //load the equipped items
        for (int i = 0; i < _equippedItems.Length; i++)
        {
            if (saveData.EquippedItems != null && saveData.EquippedItems.Length > i && saveData.EquippedItems[i] != null)
            {
                _equippedItems[i] = saveData.EquippedItems[i].ToItem();
            }
            else
            {
                _equippedItems[i] = null;
            }
        }

        //lastly we update the UI
        UIManager.Instance.UpdateInventoryUI(_inventory);
        UIManager.Instance.UpdateEquippedItemsUI(_equippedItems);
    }

    public void AddItemToInventory(Item newItem)
    {
        //we check if our index is -1, which is its default value. Then we attribute the return of the CanPickup item to see if a slot is available
        if (cachedIndex == -1)
        {
            cachedIndex = CanPickupItem();
        }

        //if the index is not -1, it uses the index for that item
        if (cachedIndex != -1)
        {
            _inventory[cachedIndex] = newItem;
            InventoryEvents.SuccessfulItemPickup();
            cachedIndex = -1;
        }
    }

    private int CanPickupItem()
    {
        //loops through the array to find an open slot and returns that index. If none is found, returns -1
        for (int i = 0; i < _inventory.Length; i++)
        {
            if (_inventory[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public void EquipItemFromInventory(Item item, int inventoryIndex, int equipmentIndex)
    {
        //ensure that swords are always in the main-hand and shields are in the off-hand
        switch (item.EquipmentType)
        {
            case EquipmentType.Sword:
                if (equipmentIndex != 0)
                    equipmentIndex = 0;
                break;

            case EquipmentType.Shield:
                if (equipmentIndex != 1)
                    equipmentIndex = 1;
                break;
        }

        Item currentEquipped = _equippedItems[equipmentIndex];
        _equippedItems[equipmentIndex] = item;
        UIManager.Instance.EquipItem(item, inventoryIndex, equipmentIndex);
        _inventory[inventoryIndex] = currentEquipped;
    }

    public void SwapItemFromInventory(Item currentItem, Item newItem, int draggableIndex, int index)
    {
        //change slots for both received items and their respective indexes in the array
        _inventory[draggableIndex] = newItem;
        _inventory[index] = currentItem;
        UIManager.Instance.ChangeItemPosition(currentItem, newItem, draggableIndex, index);
    }

    public void RemoveItemFromInventory(int index)
    {
        //tells the UIManager to create a blank sprite where the item was, spawns the item next to the player and set it to null
        UIManager.Instance.DiscardItem(index);
        SpawnItemNextToPlayer(_inventory[index], _playerTransform.position);
        _inventory[index] = null;
    }

    public void SpawnItemNextToPlayer(Item item, Vector3 dropPosition)
    {
        //randomizes an offset within a circle of radius around the drop position then calculates a spawn position with a random offset
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = dropPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

        //instantiate the item and setup its data
        var tempItem = Instantiate(_itemOverworldPrefab, spawnPosition, Quaternion.identity);
        tempItem.GetComponent<DroppedItem>().SetupItem(item);
    }
}