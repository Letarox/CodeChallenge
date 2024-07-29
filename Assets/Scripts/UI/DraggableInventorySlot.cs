using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableInventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
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
        //call the event when we begging dragging and change the alpha of the dragged image to 0.5f so we can notice it. Also disables its raycast during the drag
        InventoryEvents.BeginDraggin(_inventoryIndex);
        _image.raycastTarget = false;
        var temp = _image.color;
        temp.a = 0.5f;
        _image.color = temp;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //make sure we have our image follow our mouse as we drag it        
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //if we dragged outside of a proper spot, we set the position back to the original position. We also revert the alpha change of the image
        _image.raycastTarget = true;
        _image.rectTransform.localPosition = _originalPosition;
        var temp = _image.color;
        temp.a = 1f;
        _image.color = temp;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //when hovering over we call the event for it
        InventoryEvents.BeginHoverInventory(_inventoryIndex);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //when no longer hovering over we call the event for it
        InventoryEvents.EndHoverInventory(_inventoryIndex);
    }
}
