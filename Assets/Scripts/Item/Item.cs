using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item/Create new Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private EquipmentType _equipmentType;
    public string Name => _name;
    public Sprite Icon => _icon;
    public EquipmentType EquipmentType => _equipmentType;
}