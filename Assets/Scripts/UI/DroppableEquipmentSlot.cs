using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroppableEquipmentSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private int _equipmentIndex;

    private Item _item;
    public Image Image => _image;

    public void OnDrop(PointerEventData eventData)
    {
        //grab a handle of the image that was dragged and update the item to match the current selected item
        Image draggedImage = eventData.pointerDrag.GetComponent<Image>();
        DraggableInventorySlot draggableInventorySlot = eventData.pointerDrag.GetComponent<DraggableInventorySlot>();
        DraggableEquipmentSlot draggableEquipmentSlot = eventData.pointerDrag.GetComponent<DraggableEquipmentSlot>();

        if (draggedImage != null && draggableInventorySlot != null && draggableEquipmentSlot == null)
        {
            _item = InventoryManager.Instance.Inventory[UIManager.Instance.CurrentSelectedItem];
            InventoryEvents.ItemEquippedFromInventory(_item, UIManager.Instance.CurrentSelectedItem, _equipmentIndex);
        }
        else
        {
            _image.raycastTarget = true;
            // Get the index of this drop slot within its parent
            int dropSlotIndex = this.transform.GetSiblingIndex();

            _item = InventoryManager.Instance.EquippedItems[UIManager.Instance.CurrentSelectedItem];

            // Trigger an event to update the UIManager or inventory
            InventoryEvents.ItemEquippedFromEquipment(_item, UIManager.Instance.CurrentSelectedItem, _equipmentIndex);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        InventoryEvents.BeginHoverEquipment(_equipmentIndex);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        InventoryEvents.EndHoverEquipment(_equipmentIndex);
    }
}
