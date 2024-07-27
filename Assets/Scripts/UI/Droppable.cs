using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Droppable : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
        if (_image == null)
            Debug.LogError("Image is NULL on " + transform.name);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Image draggedImage = eventData.pointerDrag.GetComponent<Image>();
        if (draggedImage != null)
        {
            Color newColor = draggedImage.color;
            _image.color = newColor;
            InventoryEvents.ColorUsed(newColor);
        }
    }
}
