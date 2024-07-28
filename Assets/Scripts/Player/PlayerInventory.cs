using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private void OnEnable()
    {
        InventoryEvents.OnItemEquipped += EquipItemFromInventory;
        InventoryEvents.OnItemSwap += SwapItemFromInventory;
        InventoryEvents.OnItemDiscard += RemoveItemFromInventory;
        InventoryEvents.OnItemPickupAttempt += AddItemToInventory;
    }

    private void OnDisable()
    {
        InventoryEvents.OnItemEquipped -= EquipItemFromInventory;
        InventoryEvents.OnItemSwap -= SwapItemFromInventory;
        InventoryEvents.OnItemDiscard -= RemoveItemFromInventory;
        InventoryEvents.OnItemPickupAttempt -= AddItemToInventory;
    }

    private void Update()
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

    private void EquipItemFromInventory(Item item, int inventoryIndex, int equipmentIndex)
    {
        InventoryManager.Instance.EquipItemFromInventory(item, inventoryIndex, equipmentIndex);
    }

    private void SwapItemFromInventory(Item currentItem, Item newItem, int draggableIndex, int index)
    {
        InventoryManager.Instance.SwapItemFromInventory(currentItem, newItem, draggableIndex, index);
    }

    private void RemoveItemFromInventory(int index)
    {
        InventoryManager.Instance.RemoveItemFromInventory(index);
    }

    private void AddItemToInventory(Item newItem)
    {
        InventoryManager.Instance.AddItemToInventory(newItem);
    }
}