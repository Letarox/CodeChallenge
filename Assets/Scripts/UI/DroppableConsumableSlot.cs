using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroppableConsumableSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image _image;

    public void OnDrop(PointerEventData eventData)
    {
        //gets a handle of the image that was dragged over, and item
        Image draggedImage = eventData.pointerDrag.GetComponent<Image>();
        DraggableInventorySlot draggableInventorySlot = eventData.pointerDrag.GetComponent<DraggableInventorySlot>();
        DraggableConsumableSlot draggableConsumableSlot = eventData.pointerDrag.GetComponent<DraggableConsumableSlot>();

        //check if we have an image and the inventory slot
        if (draggedImage != null && draggableInventorySlot != null && draggableConsumableSlot == null)
        {
            _image.raycastTarget = true;
            // Get the index of this drop slot within its parent
            int dropSlotIndex = this.transform.GetSiblingIndex();

            // Trigger an event to update the UIManager or inventory
            InventoryEvents.EquipConsumableItem(InventoryManager.Instance.Inventory[draggableInventorySlot.InventoryIndex], draggableInventorySlot.InventoryIndex);
        }
        else
        {
            _image.raycastTarget = true;
            // Get the index of this drop slot within its parent
            int dropSlotIndex = this.transform.GetSiblingIndex();

            // Trigger an event to update the UIManager or inventory
            InventoryEvents.SwapConsumableItem(InventoryManager.Instance.ConsumableItem, InventoryManager.Instance.Inventory[draggableInventorySlot.InventoryIndex], draggableInventorySlot.InventoryIndex);
        }
    }
}
