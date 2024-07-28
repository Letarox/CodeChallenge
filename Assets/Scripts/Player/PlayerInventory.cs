using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Item[] _inventory;
    [SerializeField] private Item[] _equippedItems;

    public Item[] Inventory => _inventory;
    public Item[] EquippedItems => _equippedItems;

    void OnEnable()
    {
        InventoryEvents.OnItemEquipped += EquipItemFromInventory;
        InventoryEvents.OnItemSwap += SwapItemFromInventory;
        InventoryEvents.OnItemDiscard += RemoveItemFromInventory;
        InventoryEvents.OnItemPickupAttempt += AddItemToInventory;
    }

    void OnDisable()
    {
        InventoryEvents.OnItemEquipped -= EquipItemFromInventory;
        InventoryEvents.OnItemSwap -= SwapItemFromInventory;
        InventoryEvents.OnItemDiscard -= RemoveItemFromInventory;
        InventoryEvents.OnItemPickupAttempt -= AddItemToInventory;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && GameManager.Instance.CurrentActionState == ActionState.None)
        {            
            UIManager.Instance.OpenInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.CurrentActionState == ActionState.Inventory)
        {
            UIManager.Instance.CloseInventory();
        }
    }

    void EquipItemFromInventory(Item item, int inventoryIndex, int equipmentIndex)
    {
        switch (item.EquipmentType)
        {
            case EquipmentType.Sword:
                //can only be equipped in the main hand
                if (equipmentIndex != 0)
                    equipmentIndex = 0;
                break;

            case EquipmentType.Shield:
                //can only be equipped in the offhand
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
        _inventory[draggableIndex] = newItem;
        _inventory[index] = currentItem;
        UIManager.Instance.ChangeItemPosition(currentItem, newItem, draggableIndex, index);
    }

    void RemoveItemFromInventory(int index)
    {
        UIManager.Instance.DiscardItem(index);
        InventoryManager.Instance.SpawnItemNextToPlayer(_inventory[index], transform.position);
        _inventory[index] = null;
    }

    void AddItemToInventory(Item newItem)
    {
        int index = CanPickupItem();
        if (index != -1)
        {
            _inventory[index] = newItem;
            InventoryEvents.SuccessfulItemPickup();
        }
    }

    int CanPickupItem()
    {
        for (int i = 0; i < _inventory.Length; i++)
        {
            if (_inventory[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}
