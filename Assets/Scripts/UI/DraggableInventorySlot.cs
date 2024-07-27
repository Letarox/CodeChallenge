using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableInventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private int _inventoryIndex;
    private Vector3 _originalPosition;

    public int InventoryIndex => _inventoryIndex;

    void Start()
    {
        _image = GetComponent<Image>();
        if (_image == null)
            Debug.LogError("Image is NULL on " + transform.name);
        _originalPosition = _image.rectTransform.localPosition;
    }    

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryEvents.BeginDraggin(_inventoryIndex);
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InventoryEvents.EndDraggin(_inventoryIndex);
        _image.raycastTarget = true;
        _image.rectTransform.localPosition = _originalPosition;
    }
}
