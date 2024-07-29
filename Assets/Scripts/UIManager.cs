using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(typeof(UIManager).ToString() + " is NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _inventoryDisplayText;
    [SerializeField] private GameObject _consumableDisplayText;
    [SerializeField] private GameObject _usingConsumableText;
    [SerializeField] private GameObject[] _itemSlots;
    [SerializeField] private Image[] _itemSlotImages;
    [SerializeField] private Image _playerCharacter;
    [SerializeField] private TextMeshProUGUI _proximityText;
    [SerializeField] private GameObject[] _itemDetailsPanelInventory, _itemDetailsPanelEquipment;
    [SerializeField] private TextMeshProUGUI[] _itemNamesTextInventory, _itemEquipmentTypeTextInventory, _itemDescriptionTextInventory;
    [SerializeField] private TextMeshProUGUI[] _itemNamesTextEquipment, _itemEquipmentTypeTextEquipment, _itemDescriptionTextEquipment;
    [SerializeField] private TextMeshProUGUI _itemNameTextConsumable, _itemEquipmentTypeTextConsumable, _itemDescriptionTextConsumable;
    [SerializeField] private Sprite _blankSprite;
    [SerializeField] private Image[] _equipmentSlotImages;
    [SerializeField] private GameObject _consumablePanel;
    [SerializeField] private Image _consumableImage;
    [SerializeField] private GameObject _itemDetailsPanelConsumable;
    [SerializeField] private Image _consumableDisplay;

    private PlayerInventory _playerInventory;   
    public PlayerInventory PlayerInventory => _playerInventory;
    public Sprite BlankSprite => _blankSprite;
    private int _currentSelectedItem = 0;
    public int CurrentSelectedItem => _currentSelectedItem;

    void OnEnable()
    {
        InventoryEvents.OnBeginDrag += SelectItem;
        InventoryEvents.OnBeginHoverInventory += OpenDescriptionPanelInventory;
        InventoryEvents.OnEndHoverInventory += CloseDescriptionPanelInventory;
        InventoryEvents.OnBeginHoverEquipment += OpenDescriptionPanelEquipment;
        InventoryEvents.OnEndHoverEquipment += CloseDescriptionPanelEquipment;
        InventoryEvents.OnBeginHoverConsumable += OpenDescriptionPanelConsumable;
        InventoryEvents.OnEndHoverConsumable += CloseDescriptionPanelConsumable;
    }

    void OnDisable()
    {
        InventoryEvents.OnBeginDrag -= SelectItem;
        InventoryEvents.OnBeginHoverInventory -= OpenDescriptionPanelInventory;
        InventoryEvents.OnEndHoverInventory -= CloseDescriptionPanelInventory;
        InventoryEvents.OnBeginHoverEquipment -= OpenDescriptionPanelEquipment;
        InventoryEvents.OnEndHoverEquipment -= CloseDescriptionPanelEquipment;
        InventoryEvents.OnBeginHoverConsumable -= OpenDescriptionPanelConsumable;
        InventoryEvents.OnEndHoverConsumable -= CloseDescriptionPanelConsumable;
    }

    void Start()
    {
        _playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        if (_playerInventory == null)
            Debug.LogError("Player Inventory is NULL on UI Manager.");
    }

    public void OpenInventory()
    {
        //change the ActionState in the GameManager to prevent player movement
        //Open the Panel for the player to access the inventory
        GameManager.Instance.SetActionState(ActionState.Inventory);
        _inventoryPanel.SetActive(true);
        _inventoryDisplayText.SetActive(false);
        _consumableDisplayText.SetActive(false);
        _usingConsumableText.SetActive(false);

        //loops through the player inventory and fill the information from the inventory slots with the items
        for (int i = 0; i < InventoryManager.Instance.Inventory.Length; i++)
        {
            if(InventoryManager.Instance.Inventory[i] != null)
            {
                _itemSlotImages[i].sprite = InventoryManager.Instance.Inventory[i].Icon;
                CheckToDisableIcon(_itemSlotImages[i]);
            }
            else
            {
                _itemSlotImages[i].sprite = _blankSprite;
                CheckToDisableIcon(_itemSlotImages[i]);
            }
                
        }
    }

    public void CloseInventory()
    {
        //returns the ActionState to None so the player can move again. Also disables the panel
        GameManager.Instance.SetActionState(ActionState.None);
        _inventoryPanel.SetActive(false);
        _inventoryDisplayText.SetActive(true);
        if (InventoryManager.Instance.ConsumableItem != null)
            _consumableDisplayText.SetActive(true);
    }

    public void EquipItemFromInventory(Item newItem, int inventoryIndex, int equipmentIndex)
    {
        //grab a handle of the item that is currently equipped
        Sprite currentEquipped = _equipmentSlotImages[equipmentIndex].sprite;

        //if there is no item equipped, the inventory index receives a blank sprite, otherwise it gets the proper sprite
        if(currentEquipped == null)
        {
            _itemSlotImages[inventoryIndex].sprite = _blankSprite;
        }
        else
        {
            _itemSlotImages[inventoryIndex].sprite = currentEquipped;
        }

        //updates the sprite of the equipped item, matching the provided item. Check to disable the item and resets the selection
        _equipmentSlotImages[equipmentIndex].sprite = newItem.Icon;
        CheckToDisableIcon(_itemSlotImages[inventoryIndex]);
        _currentSelectedItem = -1;
    }

    public void EquipItemFromEquipment(Item newItem, int inventoryIndex, int equipmentIndex)
    {
        // Grab a handle of the item that is currently equipped
        Sprite currentEquipped = _equipmentSlotImages[equipmentIndex].sprite;

        // If there is no item equipped, the inventory index receives a blank sprite, otherwise it gets the proper sprite
        if (currentEquipped == null)
        {
            _equipmentSlotImages[inventoryIndex].sprite = _blankSprite;
        }
        else
        {
            _equipmentSlotImages[inventoryIndex].sprite = currentEquipped;
        }

        // Updates the sprite of the equipped item, matching the provided item. Check to disable the item and reset the selection
        _equipmentSlotImages[equipmentIndex].sprite = newItem.Icon;
        CheckToDisableIcon(_itemSlotImages[inventoryIndex]);
        _currentSelectedItem = -1;
    }

    public void EquipConsumableItem(Item newItem, int inventoryIndex)
    {
        // Grab a handle of the item that is currently equipped
        Sprite currentEquipped = _consumableImage.sprite;

        // If there is no item equipped, the inventory index receives a blank sprite, otherwise it gets the proper sprite
        if (currentEquipped == null)
        {
            _itemSlotImages[inventoryIndex].sprite = _blankSprite;
        }
        else
        {
            _itemSlotImages[inventoryIndex].sprite = currentEquipped;
        }

        // Updates the sprite of the equipped item, matching the provided item. Check to disable the item and reset the selection
        _consumableImage.sprite = newItem.Icon;
        _consumableDisplay.sprite = newItem.Icon;
        _consumableDisplay.gameObject.SetActive(true);
        CheckToDisableIcon(_itemSlotImages[inventoryIndex]);
        _currentSelectedItem = -1;
    }

    public void ChangeConsumableItemPosition(Item currentItem, Item newItem, int draggableIndex)
    {
        //check if the item we landed is an empty inventory slot. If it is, the dragged inventory slot becomes an empty slot
        if (newItem == null)
        {
            _consumableImage.sprite = _blankSprite;
            _consumableDisplay.sprite = _blankSprite;
            _consumableDisplay.gameObject.SetActive(false);
        }
        else
        {
            _consumableImage.sprite = newItem.Icon;
            _consumableDisplay.sprite = newItem.Icon;
            _consumableDisplay.gameObject.SetActive(true);
        }

        //check if we have a current item, in case we do, we set the sprite to items
        if (currentItem == null)
        {
            _itemSlotImages[draggableIndex].sprite = _blankSprite;
        }
        else
        {
            _itemSlotImages[draggableIndex].sprite = currentItem.Icon;
        }

        //check to disable the icon for both items and resets the selection
        CheckToDisableIcon(_consumableImage);
        CheckToDisableIcon(_itemSlotImages[draggableIndex]);
        _currentSelectedItem = -1;
    }

    public void ChangeItemPositionFromInventory(Item currentItem, Item newItem, int draggableIndex, int oldIndex)
    {
        //check if the item we landed is an empty inventory slot. If it is, the dragged inventory slot becomes an empty slot
        if (newItem == null)
        {
            _itemSlotImages[draggableIndex].sprite = _blankSprite;
        }
        else
        {
            _itemSlotImages[draggableIndex].sprite = newItem.Icon;
        }

        //check if we have a current item, in case we do, we set the sprite to items
        if (currentItem == null)
        {
            _itemSlotImages[oldIndex].sprite = _blankSprite;
        }
        else
        {
            _itemSlotImages[oldIndex].sprite = currentItem.Icon;
        }

        //check to disable the icon for both items and resets the selection
        CheckToDisableIcon(_itemSlotImages[draggableIndex]);
        CheckToDisableIcon(_itemSlotImages[oldIndex]);
        _currentSelectedItem = -1;
    }

    public void ChangeItemPositionFromEquipment(Item currentItem, Item newItem, int draggableIndex, int oldIndex)
    {
        //check if the item we landed is an empty inventory slot. If it is, the dragged inventory slot becomes an empty slot
        if (newItem == null)
        {
            _equipmentSlotImages[draggableIndex].sprite = _blankSprite;
        }
        else
        {
            _equipmentSlotImages[draggableIndex].sprite = newItem.Icon;
        }

        //check if we have a current item, in case we do, we set the sprite to items
        if (currentItem == null)
        {
            _itemSlotImages[oldIndex].sprite = _blankSprite;
        }
        else
        {
            _itemSlotImages[oldIndex].sprite = currentItem.Icon;
        }

        //check to disable the icon for both items and resets the selection
        CheckToDisableIcon(_itemSlotImages[draggableIndex]);
        CheckToDisableIcon(_itemSlotImages[oldIndex]);
        _currentSelectedItem = -1;
    }

    void CheckToDisableIcon(Image image)
    {
        //check if its an empty image/inventory slot, if it is disables raycasting for that slot
        if(image.sprite == _blankSprite || image.sprite == null)
        {
            image.raycastTarget = false;
        }
        else
        {
            image.raycastTarget = true;
        }
    }

    void SelectItem(int index)
    {
        _currentSelectedItem = index;
    }

    public void SetProximityMessage(bool state)
    {
        _proximityText.gameObject.SetActive(state);
    }

    public void OpenDescriptionPanelInventory(int index)
    {
        //if we are hovering an empty slot we simply return
        if (InventoryManager.Instance.Inventory[index] == null)
            return;

        //open all panels/texts regarding the detailed information
        _itemDetailsPanelInventory[index].SetActive(true);
        _itemNamesTextInventory[index].text = InventoryManager.Instance.Inventory[index].Name;
        string equipmentText = SetItemTypeText(InventoryManager.Instance.Inventory[index].ItemType);
        _itemEquipmentTypeTextInventory[index].text = InventoryManager.Instance.Inventory[index].ItemType.ToString() + equipmentText;
        _itemDescriptionTextInventory[index].text = InventoryManager.Instance.Inventory[index].Description;
    }

    public void OpenDescriptionPanelEquipment(int index)
    {
        //if we are hovering an empty slot we simply return
        if (InventoryManager.Instance.EquippedItems[index] == null)
            return;

        //open all panels/texts regarding the detailed information
        _itemDetailsPanelEquipment[index].SetActive(true);
        _itemNamesTextEquipment[index].text = InventoryManager.Instance.EquippedItems[index].Name;
        string equipmentText = SetItemTypeText(InventoryManager.Instance.EquippedItems[index].ItemType);
        _itemEquipmentTypeTextEquipment[index].text = InventoryManager.Instance.EquippedItems[index].ItemType.ToString() + equipmentText;
        _itemDescriptionTextEquipment[index].text = InventoryManager.Instance.EquippedItems[index].Description;
    }

    public void OpenDescriptionPanelConsumable()
    {
        //if we are hovering an empty slot we simply return
        if (InventoryManager.Instance.ConsumableItem == null)
            return;

        //open all panels/texts regarding the detailed information
        _itemDetailsPanelConsumable.SetActive(true);
        _itemNameTextConsumable.text = InventoryManager.Instance.ConsumableItem.Name;
        string equipmentText = SetItemTypeText(InventoryManager.Instance.ConsumableItem.ItemType);
        _itemEquipmentTypeTextConsumable.text = InventoryManager.Instance.ConsumableItem.ItemType.ToString() + equipmentText;
        _itemDescriptionTextConsumable.text = InventoryManager.Instance.ConsumableItem.Description;
    }

    string SetItemTypeText(ItemType type)
    {
        switch (type)
        {
            case ItemType.Sword:
                return " (Main-hand)";
            case ItemType.Axe:
                return " (Any hand)";
            case ItemType.Shield:
                return " (Off-hand)";
            case ItemType.Consumable:
                return string.Empty;
        }
        return string.Empty;
    }

    public void CloseDescriptionPanelInventory(int index)
    {
        _itemDetailsPanelInventory[index].SetActive(false);
    }

    public void CloseDescriptionPanelEquipment(int index)
    {
        _itemDetailsPanelEquipment[index].SetActive(false);
    }

    public void CloseDescriptionPanelConsumable()
    {
        _itemDetailsPanelConsumable.SetActive(false);
    }

    public void DiscardItem(int index)
    {
        _itemSlotImages[index].sprite = _blankSprite;
    }

    public void UpdateInventoryUI(Item[] inventory)
    {
        //loops through all inventory slots and assign their icons
        for (int i = 0; i < _itemSlotImages.Length; i++)
        {
            if (i < inventory.Length && inventory[i] != null)
            {
                _itemSlotImages[i].sprite = inventory[i].Icon;
            }
            else
            {
                _itemSlotImages[i].sprite = _blankSprite;
            }
        }
    }

    public void UpdateEquippedItemsUI(Item[] equippedItems)
    {
        //loops through all equipment slots and assign their icons
        for (int i = 0; i < _equipmentSlotImages.Length; i++)
        {
            if (i < equippedItems.Length && equippedItems[i] != null)
            {
                _equipmentSlotImages[i].sprite = equippedItems[i].Icon;
            }
            else
            {
                _equipmentSlotImages[i].sprite = _blankSprite;
            }
        }
    }

    public void UpdateConsumableItemUI(Item consumableItem)
    {
        //assigns the consumable icon
        if (consumableItem != null)
        {
            _consumableImage.sprite = consumableItem.Icon;
            _consumableDisplay.sprite = consumableItem.Icon;
            _consumableDisplay.gameObject.SetActive(true);
            _consumableDisplayText.SetActive(true);
        }
        else
        {
            _consumableImage.sprite = _blankSprite;
            _consumableDisplay.gameObject.SetActive(false);
            _consumableDisplayText.SetActive(false);
        }
    }

    public IEnumerator UsingConsumableRoutine()
    {
        //enables the text telling the player the item was used, and removes the item from where it was
        _usingConsumableText.SetActive(true);
        _consumableDisplayText.SetActive(false);
        _consumableImage.sprite = _blankSprite;
        _consumableDisplay.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        _usingConsumableText.SetActive(false);
    }
}
