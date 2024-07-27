using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroppableEquipment : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image _image;

    public void OnDrop(PointerEventData eventData)
    {
        Image draggedImage = eventData.pointerDrag.GetComponent<Image>();
        if (draggedImage != null)
        {
            Color newColor = draggedImage.color;
            Color currentColor = _image.color;
            _image.color = newColor;
            InventoryEvents.ColorEquipped(currentColor, UIManager.Instance.currentSelectedItem);
        }
    }
}
