// ============================================================================================
// File: PickupItem.cs
// Description: Use this on items in the world that the player can pick up.
// ============================================================================================

using KBCore.Refs;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField, Self] InventoryItem _item;
    [SerializeField] int _quantity = 1;

    public int Quantity => _quantity;

    void OnValidate()
    {
        this.ValidateRefs();
    }

    public InventoryItem GetInventoryItem()
    {
        return _item;
    }
}
