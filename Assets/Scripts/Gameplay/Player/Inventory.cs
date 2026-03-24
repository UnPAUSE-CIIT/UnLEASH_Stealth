// ============================================================================================
// File: Inventory.cs
// Description: Use this to give the player an inventory system. Handles picking up and using items.
// ============================================================================================

using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [Header( "Inventory Settings" )]
    [SerializeField] int _maxSlots = 9;

    public List<InventoryItem> Items { get; private set; } = new();
    public int MaxSlots => _maxSlots;

    public bool AddItem( InventoryItem item )
    {
        if ( Items.Count >= _maxSlots )
        {
            Debug.Log( "Inventory is full!" );
            return false;
        }

        Items.Add( item );
        Debug.Log( $"Added {item.ItemName} to inventory." );
        return true;
    }

    public void RemoveItem( InventoryItem item )
    {
        if ( Items.Contains( item ) )
        {
            Items.Remove( item );
            Debug.Log( $"Removed {item.ItemName} from inventory." );
        }
    }

    public void UseItem( int slotIndex )
    {
        if ( slotIndex < 0 || slotIndex >= Items.Count )
        {
            Debug.Log( "Invalid slot index." );
            return;
        }

        InventoryItem item = Items[slotIndex];
        item.OnUse();
        item.DecreaseQuantity();

        if ( item.Quantity <= 0 )
        {
            Items.RemoveAt( slotIndex );
        }
    }
}
