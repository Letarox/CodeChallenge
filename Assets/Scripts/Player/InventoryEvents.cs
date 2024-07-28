using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryEvents : MonoBehaviour
{
    public static event Action<Item, int, int> OnItemEquipped;
    public static event Action<Item, Item, int, int> OnItemSwap;
    public static event Action<int> OnBeginDrag;
    public static event Action<int> OnItemDiscard;
    public static event Action<Item> OnItemPickupAttempt;
    public static event Action OnItemSucessfulPickup;
    public static event Action<int> OnBeginHoverInventory;
    public static event Action<int> OnEndHoverInventory;
    public static event Action<int> OnBeginHoverEquipment;
    public static event Action<int> OnEndHoverEquipment;

    public static void ItemEquipped(Item item, int inventoryIndex, int equipmentIndex)
    {
        OnItemEquipped?.Invoke(item, inventoryIndex, equipmentIndex);
    }

    public static void ItemSwapped(Item currentItem, Item newItem, int draggableIndex, int index)
    {
        OnItemSwap?.Invoke(currentItem, newItem, draggableIndex, index);
    }

    public static void BeginDraggin(int index)
    {
        OnBeginDrag?.Invoke(index);
    }

    public static void DiscardItem(int index)
    {
        OnItemDiscard?.Invoke(index);
    }

    public static void AttempToPickupItem(Item itemColor)
    {
        OnItemPickupAttempt?.Invoke(itemColor);
    }

    public static void SuccessfulItemPickup()
    {
        OnItemSucessfulPickup?.Invoke();
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
}