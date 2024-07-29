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
        //gets a handle of the image that was dragged over, and item
        Image draggedImage = eventData.pointerDrag.GetComponent<Image>();
        DraggableInventorySlot draggableInventorySlot = eventData.pointerDrag.GetComponent<DraggableInventorySlot>();
        DraggableEquipmentSlot draggableEquipmentSlot = eventData.pointerDrag.GetComponent<DraggableEquipmentSlot>();
        DraggableConsumableSlot draggableConsumableSlot = eventData.pointerDrag.GetComponent<DraggableConsumableSlot>();

        //check if we have an image and the inventory slot
        if (draggedImage != null && draggableInventorySlot != null && draggableEquipmentSlot == null)
        {
            _image.raycastTarget = true;
            // Get the index of this drop slot within its parent
            int dropSlotIndex = this.transform.GetSiblingIndex();

            // Trigger an event to update the UIManager or inventory
            InventoryEvents.ItemSwappedFromInventory(InventoryManager.Instance.Inventory[draggableInventorySlot.InventoryIndex], InventoryManager.Instance.Inventory[dropSlotIndex], draggableInventorySlot.InventoryIndex, dropSlotIndex);
        }
        else if(draggableEquipmentSlot != null && draggableConsumableSlot == null)
        {
            _image.raycastTarget = true;
            // Get the index of this drop slot within its parent
            int dropSlotIndex = this.transform.GetSiblingIndex();

            // Trigger an event to update the UIManager or inventory
            InventoryEvents.ItemSwappedFromEquipment(InventoryManager.Instance.EquippedItems[draggableEquipmentSlot.EquipmentIndex], InventoryManager.Instance.Inventory[dropSlotIndex], draggableEquipmentSlot.EquipmentIndex, dropSlotIndex);
        }
        else
        {
            _image.raycastTarget = true;
            // Get the index of this drop slot within its parent
            int dropSlotIndex = this.transform.GetSiblingIndex();

            // Trigger an event to update the UIManager or inventory
            InventoryEvents.SwapConsumableItem(InventoryManager.Instance.ConsumableItem, InventoryManager.Instance.Inventory[dropSlotIndex], dropSlotIndex);

        }
    }
}
