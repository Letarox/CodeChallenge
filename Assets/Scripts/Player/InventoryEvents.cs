using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryEvents : MonoBehaviour
{
    public static event Action<Color, int> OnColorEquipped;
    public static event Action<Color, Color, int, int> OnColorSwap;
    public static event Action<int> OnBeginDrag;
    public static event Action<int> OnEndDrag;
    public static event Action<int> OnItemDiscard;
    public static event Action<Color> OnItemPickupAttempt;
    public static event Action OnItemSucessfulPickup;

    public static void ColorEquipped(Color color, int index)
    {
        OnColorEquipped?.Invoke(color, index);
    }

    public static void ColorSwapped(Color currentColor, Color newColor, int draggableIndex, int index)
    {
        OnColorSwap?.Invoke(currentColor, newColor, draggableIndex, index);
    }

    public static void BeginDraggin(int index)
    {
        OnBeginDrag?.Invoke(index);
    }

    public static void EndDraggin(int index)
    {
        OnEndDrag?.Invoke(index);
    }

    public static void DiscardItem(int index)
    {
        OnItemDiscard?.Invoke(index);
    }

    public static void AttempToPickupItem(Color itemColor)
    {
        OnItemPickupAttempt?.Invoke(itemColor);
    }

    public static void SuccessfulItemPickup()
    {
        OnItemSucessfulPickup?.Invoke();
    }
}