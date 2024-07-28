using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroppableTrash : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Image draggedImage = eventData.pointerDrag.GetComponent<Image>();
        DraggableInventorySlot draggableItem = eventData.pointerDrag.GetComponent<DraggableInventorySlot>();

        if (draggedImage != null && draggableItem != null)
        {
            // Trigger an event to update the UIManager or inventory
            InventoryEvents.DiscardItem(draggableItem.InventoryIndex);
        }
    }
}
