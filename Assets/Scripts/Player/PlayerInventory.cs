using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Color[] colors;

    private void OnEnable()
    {
        InventoryEvents.OnColorUsed += RemoveColorFromInventory;
    }

    private void OnDisable()
    {
        InventoryEvents.OnColorUsed -= RemoveColorFromInventory;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.OpenInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.CloseInventory();
        }
    }

    private void RemoveColorFromInventory(Color color)
    {
        // Find the color in the inventory and remove it
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i] == color)
            {
                colors[i] = Color.white;
                UIManager.Instance.UpdateInventory(i);
                break;
            }
        }
    }
}
