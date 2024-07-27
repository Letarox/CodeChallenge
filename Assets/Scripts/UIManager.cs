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

    private PlayerInventory _playerInventory;   
    public PlayerInventory PlayerInventory => _playerInventory;
    public int currentSelectedItem = 0;

    void OnEnable()
    {
        InventoryEvents.OnBeginDrag += SelectItem;
    }

    void OnDisable()
    {
        InventoryEvents.OnBeginDrag -= SelectItem;
    }

    void Start()
    {
        _playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        if (_playerInventory == null)
            Debug.LogError("Player Inventory is NULL on UI Manager.");
    }

    void Update()
    {
        
    }

    public void OpenInventory()
    {
        GameManager.Instance.SetActionState(ActionState.Inventory);
        _inventoryPanel.SetActive(true);
        _playerCharacter.color = _playerInventory.transform.GetComponent<SpriteRenderer>().color;

        for(int i = 0; i < _playerInventory.Colors.Length; i++)
        {
            if (_playerInventory.Colors[i] == Color.white)
            {
                _itemSlotImages[i].enabled = false;
            }
            else
            {
                _itemSlotImages[i].color = _playerInventory.Colors[i];
                _itemSlotImages[i].enabled = true;
            }
        }
    }

    public void CloseInventory()
    {
        GameManager.Instance.SetActionState(ActionState.None);
        _inventoryPanel.SetActive(false);        
    }

    public void ChangeColor(Color newColor, int index)
    {
        _itemSlotImages[index].color = newColor;
        if (newColor == Color.white)
            _itemSlotImages[index].enabled = false;
        else
            _itemSlotImages[index].enabled = true;

        currentSelectedItem = -1;
    }

    public void ChangeColor(Color currentColor, Color newColor, int draggableIndex, int oldIndex)
    {
        _itemSlotImages[oldIndex].color = newColor;
        if (newColor == Color.white)
            _itemSlotImages[oldIndex].enabled = false;
        else
            _itemSlotImages[oldIndex].enabled = true;

        _itemSlotImages[draggableIndex].color = currentColor;
        if (currentColor == Color.white)
            _itemSlotImages[draggableIndex].enabled = false;
        else
            _itemSlotImages[draggableIndex].enabled = true;
        currentSelectedItem = -1;
    }

    void SelectItem(int index)
    {
        currentSelectedItem = index;
    }

    public void SetProximityMessage(bool state)
    {
        _proximityText.gameObject.SetActive(state);
    }
}
