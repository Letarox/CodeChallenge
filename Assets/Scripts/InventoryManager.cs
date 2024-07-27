using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region Singleton
    private static InventoryManager _instance;

    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(typeof(InventoryManager).ToString() + " is NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    #endregion

    [SerializeField] private GameObject _itemOverworldPrefab;

    private float spawnRadius = 1.5f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnItemNextToPlayer(Color itemColor, Vector3 dropPosition)
    {
        // Calculate a random position within a circle of radius spawnRadius around the drop position
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = dropPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

        var item = Instantiate(_itemOverworldPrefab, spawnPosition, Quaternion.identity);
        item.GetComponent<SpriteRenderer>().color = itemColor;
    }
}
