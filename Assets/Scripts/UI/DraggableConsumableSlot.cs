using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableConsumableSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;
    private Vector3 _originalPosition;

    void Start()
    {
        _image = GetComponent<Image>();
        if (_image == null)
            Debug.LogError("Image is NULL on " + transform.name);
        _originalPosition = _image.rectTransform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //we make our alpha lower to visually see what item we are dragging
        _image.raycastTarget = false;
        var temp = _image.color;
        temp.a = 0.5f;
        _image.color = temp;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //if we dragged outside of a proper spot, we set the position back to the original position
        _image.raycastTarget = true;
        _image.rectTransform.localPosition = _originalPosition;
        var temp = _image.color;
        temp.a = 1f;
        _image.color = temp;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //when hovering over we call the event for it
        InventoryEvents.BeginHoverConsumable();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //when no longer hovering over we call the event for it
        InventoryEvents.EndHoverConsumable();
    }
}