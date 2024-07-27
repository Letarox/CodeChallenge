using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Color[] _colors;

    public Color[] Colors => _colors;

    void OnEnable()
    {
        InventoryEvents.OnColorEquipped += EquipColorFromInventory;
        InventoryEvents.OnColorSwap += SwapColorFromInventory;
        InventoryEvents.OnItemDiscard += RemoveItemFromInventory;
        InventoryEvents.OnItemPickupAttempt += AddItemToInventory;
    }

    void OnDisable()
    {
        InventoryEvents.OnColorEquipped -= EquipColorFromInventory;
        InventoryEvents.OnColorSwap -= SwapColorFromInventory;
        InventoryEvents.OnItemDiscard -= RemoveItemFromInventory;
        InventoryEvents.OnItemPickupAttempt -= AddItemToInventory;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && GameManager.Instance.CurrentActionState == ActionState.None)
        {            
            UIManager.Instance.OpenInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.CurrentActionState == ActionState.Inventory)
        {
            UIManager.Instance.CloseInventory();
        }
    }

    void EquipColorFromInventory(Color color, int index)
    {
        _colors[index] = color;
        UIManager.Instance.ChangeColor(color, index);
    }

    void SwapColorFromInventory(Color currentColor, Color newColor, int draggableIndex, int index)
    {
        _colors[index] = newColor;
        UIManager.Instance.ChangeColor(currentColor, newColor, draggableIndex, index);
    }

    void RemoveItemFromInventory(int index)
    {
        UIManager.Instance.ChangeColor(Color.white, index);
        InventoryManager.Instance.SpawnItemNextToPlayer(_colors[index], transform.position);
        _colors[index] = Color.white;
    }

    void AddItemToInventory(Color newColor)
    {
        int index = CanPickupItem();
        if (index != -1)
        {
            _colors[index] = newColor;
            InventoryEvents.SuccessfulItemPickup();
        }
    }

    int CanPickupItem()
    {
        for (int i = 0; i < _colors.Length; i++)
        {
            if (_colors[i] == Color.white)
            {
                return i;
            }
        }
        return -1;
    }
}
