using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroppableInventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image _image;

    public void OnDrop(PointerEventData eventData)
    {
        Image draggedImage = eventData.pointerDrag.GetComponent<Image>();
        DraggableInventorySlot draggableItem = eventData.pointerDrag.GetComponent<DraggableInventorySlot>();

        if (draggedImage != null && draggableItem != null)
        {
            _image.raycastTarget = true;
            Color newColor = draggedImage.color;
            Color currentColor = _image.color;

            _image.color = newColor;

            // Get the index of this drop slot within its parent
            int dropSlotIndex = this.transform.GetSiblingIndex();

            // Trigger an event to update the UIManager or inventory
            InventoryEvents.ColorSwapped(currentColor, newColor, draggableItem.InventoryIndex, dropSlotIndex);
        }
    }
}
