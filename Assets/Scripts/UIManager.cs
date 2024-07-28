using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
    [SerializeField] private GameObject[] _itemSlots;
    [SerializeField] private Image[] _itemSlotImages;
    [SerializeField] private Image _playerCharacter;
    [SerializeField] private TextMeshProUGUI _proximityText;
    [SerializeField] private GameObject[] _itemDetailsPanelInventory, _itemDetailsPanelEquipment;
    [SerializeField] private TextMeshProUGUI[] _itemNamesTextInventory, _itemEquipmentTypeTextInventory, _itemDescriptionTextInventory;
    [SerializeField] private TextMeshProUGUI[] _itemNamesTextEquipment, _itemEquipmentTypeTextEquipment, _itemDescriptionTextEquipment;
    [SerializeField] private Sprite _blankSprite;
    [SerializeField] private Image[] _equipmentImage;

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
    }

    void OnDisable()
    {
        InventoryEvents.OnBeginDrag -= SelectItem;
        InventoryEvents.OnBeginHoverInventory -= OpenDescriptionPanelInventory;
        InventoryEvents.OnEndHoverInventory -= CloseDescriptionPanelInventory;
        InventoryEvents.OnBeginHoverEquipment -= OpenDescriptionPanelEquipment;
        InventoryEvents.OnEndHoverEquipment -= CloseDescriptionPanelEquipment;
    }

    void Start()
    {
        _playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        if (_playerInventory == null)
            Debug.LogError("Player Inventory is NULL on UI Manager.");
    }

    public void OpenInventory()
    {
        GameManager.Instance.SetActionState(ActionState.Inventory);
        _inventoryPanel.SetActive(true);
        _playerCharacter.color = _playerInventory.transform.GetComponent<SpriteRenderer>().color;

        for(int i = 0; i < _playerInventory.Inventory.Length; i++)
        {
            if(_playerInventory.Inventory[i] != null)
                _itemSlotImages[i].sprite = _playerInventory.Inventory[i].Icon;
            else
            {
                _itemSlotImages[i].sprite = _blankSprite;
                CheckToDisableIcon(_itemSlotImages[i]);
            }
                
        }
    }

    public void CloseInventory()
    {
        GameManager.Instance.SetActionState(ActionState.None);
        _inventoryPanel.SetActive(false);        
    }

    public void EquipItem(Item newItem, int inventoryIndex, int equipmentIndex)
    {
        Sprite currentEquipped = _equipmentImage[equipmentIndex].sprite;
        if(currentEquipped == null)
        {
            _itemSlotImages[inventoryIndex].sprite = _blankSprite;
        }
        else
        {
            _itemSlotImages[inventoryIndex].sprite = currentEquipped;
        }

        _equipmentImage[equipmentIndex].sprite = newItem.Icon;
        CheckToDisableIcon(_itemSlotImages[inventoryIndex]);
        _currentSelectedItem = -1;
    }

    public void EquipTwoHandedItem(Item newItem, int inventoryIndex, int equipmentIndex)
    {
        for(int i = 0; i < _equipmentImage.Length; i++)
        {
            _equipmentImage[i].sprite = _blankSprite;
        }

        _equipmentImage[equipmentIndex].sprite = newItem.Icon;
        CheckToDisableIcon(_itemSlotImages[inventoryIndex]);
        _currentSelectedItem = -1;
    }

    public void ChangeItemPosition(Item currentItem, Item newItem, int draggableIndex, int oldIndex)
    {
        if (newItem == null)
        {
            _itemSlotImages[draggableIndex].sprite = _blankSprite;
        }
        else
        {
            _itemSlotImages[draggableIndex].sprite = newItem.Icon;
        }

        if (currentItem == null)
        {
            _itemSlotImages[oldIndex].sprite = _blankSprite;
        }
        else
        {
            _itemSlotImages[oldIndex].sprite = currentItem.Icon;
        }

        CheckToDisableIcon(_itemSlotImages[draggableIndex]);
        CheckToDisableIcon(_itemSlotImages[oldIndex]);
        _currentSelectedItem = -1;
    }

    void CheckToDisableIcon(Image image)
    {
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
        if (PlayerInventory.Inventory[index] == null)
            return;
        _itemDetailsPanelInventory[index].SetActive(true);
        _itemNamesTextInventory[index].text = PlayerInventory.Inventory[index].Name;
        _itemEquipmentTypeTextInventory[index].text = PlayerInventory.Inventory[index].EquipmentType.ToString();
        _itemDescriptionTextInventory[index].text = PlayerInventory.Inventory[index].Description;
    }

    public void OpenDescriptionPanelEquipment(int index)
    {
        if (PlayerInventory.EquippedItems[index] == null)
            return;

        _itemDetailsPanelEquipment[index].SetActive(true);
        _itemNamesTextEquipment[index].text = PlayerInventory.EquippedItems[index].Name;
        _itemEquipmentTypeTextEquipment[index].text = PlayerInventory.EquippedItems[index].EquipmentType.ToString();
        _itemDescriptionTextEquipment[index].text = PlayerInventory.EquippedItems[index].Description;
    }

    public void CloseDescriptionPanelInventory(int index)
    {
        _itemDetailsPanelInventory[index].SetActive(false);
    }

    public void CloseDescriptionPanelEquipment(int index)
    {
        _itemDetailsPanelEquipment[index].SetActive(false);
    }

    public void DiscardItem(int index)
    {
        _itemSlotImages[index].sprite = _blankSprite;
    }
}
