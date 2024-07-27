using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    private bool _isPlayerNear = false;
    private PlayerInventory _playerInventory;
    private Color _myColor;

    void OnEnable()
    {
        InventoryEvents.OnItemSucessfulPickup += SelfDestruct;
    }

    void OnDisable()
    {
        InventoryEvents.OnItemSucessfulPickup -= SelfDestruct;
    }

    void Start()
    {
        _myColor = GetComponent<SpriteRenderer>().color;
        if (_myColor == null)
            Debug.LogError("MyColor is NULL on " + transform.name);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _isPlayerNear && GameManager.Instance.CurrentActionState == ActionState.None)
        {
            if(_playerInventory != null)
            {
                InventoryEvents.AttempToPickupItem(_myColor);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.CurrentActionState == ActionState.None)
        {
            _isPlayerNear = true;            
            UIManager.Instance.SetProximityMessage(true);
            _playerInventory = other.GetComponent<PlayerInventory>();
            if (_playerInventory == null)
                Debug.LogError("Player Inventory is NULL on " + transform.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = false;
            UIManager.Instance.SetProximityMessage(false);
            _playerInventory = null;
        }
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}