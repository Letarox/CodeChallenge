using System;
using UnityEngine;

public class InventoryEvents : MonoBehaviour
{
    public static event Action<Color> OnColorUsed;

    public static void ColorUsed(Color color)
    {
        OnColorUsed?.Invoke(color);
    }
}