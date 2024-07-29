using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item/Create new Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string _name;
    [TextArea][SerializeField] string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private ItemType _itemType;
    public string Name => _name;
    public string Description => _description;
    public Sprite Icon => _icon;
    public ItemType ItemType => _itemType;

    public void SetData(SerializableItem serializableItem)
    {
        _name = serializableItem.itemName;
        _description = serializableItem.description;
        if (!string.IsNullOrEmpty(serializableItem.iconPath))
        {
            _icon = Resources.Load<Sprite>(serializableItem.iconPath);
        }
        _itemType = serializableItem.equipmentType;
    }
}