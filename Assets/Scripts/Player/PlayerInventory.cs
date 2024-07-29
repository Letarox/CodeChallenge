using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private void OnEnable()
    {
        InventoryEvents.OnItemEquippedFromInventory += EquipItemFromInventory;
        InventoryEvents.OnItemSwapFromInventory += SwapItemFromInventory;
        InventoryEvents.OnItemEquippedFromEquipment += EquipItemFromEquipment;
        InventoryEvents.OnItemSwapFromEquipment += SwapItemFromEquipment;        
        InventoryEvents.OnItemDiscard += RemoveItemFromInventory;
        InventoryEvents.OnItemPickupAttempt += AddItemToInventory;
        InventoryEvents.OnConsumableEquip += EquipConsumableItem;
        InventoryEvents.OnConsumableSwap += SwapConsumableItem;
    }

    private void OnDisable()
    {
        InventoryEvents.OnItemEquippedFromInventory -= EquipItemFromInventory;
        InventoryEvents.OnItemSwapFromInventory -= SwapItemFromInventory;
        InventoryEvents.OnItemEquippedFromEquipment -= EquipItemFromEquipment;
        InventoryEvents.OnItemSwapFromEquipment -= SwapItemFromEquipment;
        InventoryEvents.OnItemDiscard -= RemoveItemFromInventory;
        InventoryEvents.OnItemPickupAttempt -= AddItemToInventory;
        InventoryEvents.OnConsumableEquip -= EquipConsumableItem;
        InventoryEvents.OnConsumableSwap -= SwapConsumableItem;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && GameManager.Instance.CurrentActionState == ActionState.None)
        {
            UIManager.Instance.OpenInventory();
        }

        if (Input.GetKeyDown(KeyCode.Q) && InventoryManager.Instance.ConsumableItem != null && GameManager.Instance.CurrentActionState == ActionState.None)
        {
            InventoryManager.Instance.UseConsumableItem();
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

    private void EquipItemFromEquipment(Item item, int inventoryIndex, int equipmentIndex)
    {
        InventoryManager.Instance.EquipItemFromEquipment(item, inventoryIndex, equipmentIndex);
    }

    private void SwapItemFromEquipment(Item currentItem, Item newItem, int draggableIndex, int index)
    {
        InventoryManager.Instance.SwapItemFromEquipment(currentItem, newItem, draggableIndex, index);
    }

    private void EquipConsumableItem(Item item, int inventoryIndex)
    {
        InventoryManager.Instance.EquipConsumableItem(item, inventoryIndex);
    }

    private void SwapConsumableItem(Item item, Item newItem, int inventoryIndex)
    {
        InventoryManager.Instance.SwapConsumableItem(item, newItem, inventoryIndex);
    }

    private void RemoveItemFromInventory(int index)
    {
        InventoryManager.Instance.RemoveItemFromInventory(index);
    }

    private void AddItemToInventory(Item newItem, GameObject obj)
    {
        InventoryManager.Instance.AddItemToInventory(newItem, obj);
    }
}