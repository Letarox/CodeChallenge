using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    private PlayerInventory _playerInventory;

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
        _inventoryPanel.SetActive(true);
        _playerCharacter.color = _playerInventory.transform.GetComponent<SpriteRenderer>().color;

        for(int i = 0; i < _playerInventory.colors.Length; i++)
        {
            if (_playerInventory.colors[i] == Color.white)
            {
                _itemSlotImages[i].gameObject.SetActive(false);
            }
            else
            {
                _itemSlotImages[i].color = _playerInventory.colors[i];
                _itemSlotImages[i].gameObject.SetActive(true);
            }
        }
    }

    public void CloseInventory()
    {
        _inventoryPanel.SetActive(false);
    }

    public void UpdateInventory(int index)
    {
        _itemSlotImages[index].color = Color.white;
        _itemSlotImages[index].gameObject.SetActive(false);
    }
}
