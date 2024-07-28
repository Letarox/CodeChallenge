using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroppableEquipment : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private int _equipmentIndex;

    private Item _item;
    public Image Image => _image;

    public void OnDrop(PointerEventData eventData)
    {
        Image draggedImage = eventData.pointerDrag.GetComponent<Image>();
        if (draggedImage != null)
        {
            _item = UIManager.Instance.PlayerInventory.Inventory[UIManager.Instance.CurrentSelectedItem];
            InventoryEvents.ItemEquipped(_item, UIManager.Instance.CurrentSelectedItem, _equipmentIndex);
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

    public void UpdateImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
