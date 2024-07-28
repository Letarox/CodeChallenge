using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    private bool _isPlayerNear = false;
    private PlayerInventory _playerInventory;
    private Item _item;

    void OnEnable()
    {
        InventoryEvents.OnItemSucessfulPickup += SelfDestruct;
    }

    void OnDisable()
    {
        InventoryEvents.OnItemSucessfulPickup -= SelfDestruct;
    }

    public void SetupItem(Item item)
    {
        _item = item;
    }

    void Start()
    {
        if (_item == null)
            Debug.LogError("Item is NULL on " + transform.name);
    }

    void Update()
    {
        //check if the player pressed E, he is close to the item and the game AcionState is free to play
        if(Input.GetKeyDown(KeyCode.E) && _isPlayerNear && GameManager.Instance.CurrentActionState == ActionState.None)
        {
            if(_playerInventory != null)
            {
                InventoryEvents.AttempToPickupItem(_item);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //check comes in range and set the bool. If we are not in Inventory Mode, display the proper message to pickup the item
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = true;
            _playerInventory = other.GetComponent<PlayerInventory>();

            if (_playerInventory == null)
                Debug.LogError("Player Inventory is NULL on " + transform.name);

            if (GameManager.Instance.CurrentActionState == ActionState.None)
                 UIManager.Instance.SetProximityMessage(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //when the player leaves it resets everything
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
