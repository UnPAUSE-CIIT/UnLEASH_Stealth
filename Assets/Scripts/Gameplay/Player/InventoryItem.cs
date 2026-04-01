// ============================================================================================
// File: InventoryItem.cs
// Description: Use this to create different types of items the player can pick up and use.
// ============================================================================================

using UnityEngine;


public class InventoryItem : MonoBehaviour
{
    [Header( "Item Settings" )]
    [field: SerializeField] public string ItemName { get; protected set; } = "Item";
    [field: SerializeField] public int Quantity { get; set; } = 1;

    public virtual void OnUse()
    {
        Debug.Log( $"Used: {ItemName}" );
    }

    public void SetQuantity( int quantity )
    {
        Quantity = quantity;
    }

    public void DecreaseQuantity()
    {
        Quantity--;
    }
}
