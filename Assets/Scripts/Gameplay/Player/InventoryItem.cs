// ============================================================================================
// File: InventoryItem.cs
// Description: Use this to create different types of items the player can pick up and use.
// ============================================================================================

using UnityEngine;

public enum ItemType
{
    Weapon,
    Gadget,
    KeyItem,
    Objective
}

public class InventoryItem : MonoBehaviour
{
    [Header( "Item Settings" )]
    [field: SerializeField] public string ItemName { get; protected set; } = "Item";
    [field: SerializeField] public string ItemDescription { get; private set; } = "A useful item.";
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public ItemType ItemType { get; private set; }
    [field: SerializeField] public int Quantity { get; private set; } = 1;

    public virtual void OnUse()
    {
        Debug.Log( $"Used: {ItemName}" );
    }

    public void DecreaseQuantity()
    {
        Quantity--;
    }
}
