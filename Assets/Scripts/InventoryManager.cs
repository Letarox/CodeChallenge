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

    [SerializeField] private Item[] _inventory = new Item[10];
    [SerializeField] private Item[] _equippedItems = new Item[2];
    [SerializeField] private Item _consumableItem;
    [SerializeField] private GameObject _itemOverworldPrefab;
    private float spawnRadius = 1.5f;
    private int cachedIndex = -1;

    private Transform _playerTransform;

    public Item[] Inventory => _inventory;
    public Item[] EquippedItems => _equippedItems;
    public Item ConsumableItem => _consumableItem;

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
        SaveLoadSystem.SaveInventory(_inventory, _equippedItems, _consumableItem);
    }

    public void LoadInventory()
    {
        SaveData saveData = SaveLoadSystem.LoadInventory();

        //if no data is found, uses default
        if (saveData == null)
        {
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

        //load the consumable item
        if (saveData.ConsumableItem != null)
        {
            _consumableItem = saveData.ConsumableItem.ToItem();
        }
        else
        {
            _consumableItem = null;
        }

        //lastly we update the UI
        UIManager.Instance.UpdateInventoryUI(_inventory);
        UIManager.Instance.UpdateEquippedItemsUI(_equippedItems);
        UIManager.Instance.UpdateConsumableItemUI(_consumableItem);
    }

    public void AddItemToInventory(Item newItem, GameObject obj)
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
            InventoryEvents.SuccessfulItemPickup(obj);
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
        switch (item.ItemType)
        {
            case ItemType.Sword:
                if (equipmentIndex != 0)
                    equipmentIndex = 0;
                break;

            case ItemType.Shield:
                if (equipmentIndex != 1)
                    equipmentIndex = 1;
                break;

            case ItemType.Consumable:
                return;
        }

        //grab a handle of the current equipped item, and swap spots with the inventory slot. UIManager is also updated
        Item currentEquipped = _equippedItems[equipmentIndex];
        _equippedItems[equipmentIndex] = item;
        UIManager.Instance.EquipItemFromInventory(item, inventoryIndex, equipmentIndex);
        _inventory[inventoryIndex] = currentEquipped;
    }

    public void SwapItemFromInventory(Item currentItem, Item newItem, int draggableIndex, int index)
    {
        //change slots for both received items and their respective indexes in the array
        _inventory[draggableIndex] = newItem;
        _inventory[index] = currentItem;
        UIManager.Instance.ChangeItemPositionFromInventory(currentItem, newItem, draggableIndex, index);
    }

    public void EquipItemFromEquipment(Item item, int draggedIndex, int equipmentIndex)
    {
        //check if the new item is trying to change places with an item that can't be placed in its current slot
        if ((_equippedItems[equipmentIndex].ItemType == ItemType.Sword && draggedIndex != 0) || (_equippedItems[equipmentIndex].ItemType == ItemType.Shield && draggedIndex != 1))
            return;

            //ensure that swords are always in the main-hand and shields are in the off-hand
            switch (item.ItemType)
        {
            case ItemType.Sword:
                if (equipmentIndex != 0)
                    equipmentIndex = 0;
                break;

            case ItemType.Shield:
                if (equipmentIndex != 1)
                    equipmentIndex = 1;
                break;
        }

        //grab a handle of the current equipped item, and swap spots with the inventory slot. UIManager is also updated
        Item currentEquipped = _equippedItems[equipmentIndex];
        _equippedItems[equipmentIndex] = item;
        UIManager.Instance.EquipItemFromEquipment(item, draggedIndex, equipmentIndex);
        _equippedItems[draggedIndex] = currentEquipped;
    }

    public void SwapItemFromEquipment(Item currentItem, Item newItem, int draggableIndex, int index)
    {
        // Change slots for both received items and their respective indexes in the array
        _equippedItems[draggableIndex] = newItem;
        _inventory[index] = currentItem;
        UIManager.Instance.ChangeItemPositionFromEquipment(currentItem, newItem, draggableIndex, index);
    }

    public void EquipConsumableItem(Item item, int draggedIndex)
    {
        //check if the new item is trying to change places with an item that can't be placed in its current slot
        if (item.ItemType != ItemType.Consumable)
            return;

        //grab a handle of the current equipped item, and swap spots with the inventory slot. UIManager is also updated
        Item currentEquipped = _consumableItem;
        _consumableItem = item;
        UIManager.Instance.EquipConsumableItem(item, draggedIndex);
        _inventory[draggedIndex] = currentEquipped;
    }

    public void SwapConsumableItem(Item currentItem, Item newItem, int draggableIndex)
    {
        //change slots for both received items and their respective indexes in the array
        _consumableItem = newItem;
        _inventory[draggableIndex] = currentItem;
        UIManager.Instance.ChangeConsumableItemPosition(currentItem, newItem, draggableIndex);
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

    public void UseConsumableItem()
    {
        _consumableItem = null;
        UIManager.Instance.StartCoroutine(UIManager.Instance.UsingConsumableRoutine());
    }
}