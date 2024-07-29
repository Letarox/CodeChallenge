using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryEvents : MonoBehaviour
{
    public static event Action<Item, int, int> OnItemEquippedFromInventory;
    public static event Action<Item, Item, int, int> OnItemSwapFromInventory;
    public static event Action<Item, int, int> OnItemEquippedFromEquipment;
    public static event Action<Item, Item, int, int> OnItemSwapFromEquipment;
    public static event Action<Item, int> OnConsumableEquip;
    public static event Action<Item, Item, int> OnConsumableSwap;
    public static event Action<int> OnBeginDrag;
    public static event Action<int> OnItemDiscard;
    public static event Action<Item, GameObject> OnItemPickupAttempt;
    public static event Action<GameObject> OnItemSucessfulPickup;
    public static event Action<int> OnBeginHoverInventory;
    public static event Action<int> OnEndHoverInventory;
    public static event Action<int> OnBeginHoverEquipment;
    public static event Action<int> OnEndHoverEquipment;
    public static event Action OnBeginHoverConsumable;
    public static event Action OnEndHoverConsumable;

    public static void ItemEquippedFromInventory(Item item, int inventoryIndex, int equipmentIndex)
    {
        OnItemEquippedFromInventory?.Invoke(item, inventoryIndex, equipmentIndex);
    }

    public static void ItemSwappedFromInventory(Item currentItem, Item newItem, int draggableIndex, int index)
    {
        OnItemSwapFromInventory?.Invoke(currentItem, newItem, draggableIndex, index);
    }

    public static void ItemEquippedFromEquipment(Item item, int inventoryIndex, int equipmentIndex)
    {
        OnItemEquippedFromEquipment?.Invoke(item, inventoryIndex, equipmentIndex);
    }

    public static void ItemSwappedFromEquipment(Item currentItem, Item newItem, int draggableIndex, int index)
    {
        OnItemSwapFromEquipment?.Invoke(currentItem, newItem, draggableIndex, index);
    }

    public static void BeginDraggin(int index)
    {
        OnBeginDrag?.Invoke(index);
    }

    public static void DiscardItem(int index)
    {
        OnItemDiscard?.Invoke(index);
    }

    public static void AttempToPickupItem(Item itemColor, GameObject obj)
    {
        OnItemPickupAttempt?.Invoke(itemColor, obj);
    }

    public static void SuccessfulItemPickup(GameObject obj)
    {
        OnItemSucessfulPickup?.Invoke(obj);
    }

    public static void BeginHoverInventory(int index)
    {
        OnBeginHoverInventory?.Invoke(index);
    }

    public static void EndHoverInventory(int index)
    {
        OnEndHoverInventory?.Invoke(index);
    }

    public static void BeginHoverEquipment(int index)
    {
        OnBeginHoverEquipment?.Invoke(index);
    }

    public static void EndHoverEquipment(int index)
    {
        OnEndHoverEquipment?.Invoke(index);
    }

    public static void EquipConsumableItem(Item item, int index)
    {
        OnConsumableEquip?.Invoke(item, index);
    }

    public static void SwapConsumableItem(Item item, Item newItem, int index)
    {
        OnConsumableSwap?.Invoke(item, newItem, index);
    }

    public static void BeginHoverConsumable()
    {
        OnBeginHoverConsumable?.Invoke();
    }

    public static void EndHoverConsumable()
    {
        OnEndHoverConsumable?.Invoke();
    }
}