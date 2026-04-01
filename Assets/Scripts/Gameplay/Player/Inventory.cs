// ============================================================================================
// File: Inventory.cs
// Description: Use this to give the player an inventory. Scroll to select item, press Q to use.
// ============================================================================================

using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] int _maxSlots = 9;
    [SerializeField] LayerMask _groundLayer;

    public List<InventoryItem> Items { get; private set; } = new();
    public int SelectedIndex { get; private set; }

    public bool AddItem( InventoryItem item )
    {
        if ( Items.Count >= _maxSlots )
        {
            Debug.Log( "Inventory full!" );
            return false;
        }

        Items.Add( item );
        return true;
    }

    public void SelectNext()
    {
        if ( Items.Count == 0 ) return;
        SelectedIndex = ( SelectedIndex + 1 ) % Items.Count;
    }

    public void SelectPrevious()
    {
        if ( Items.Count == 0 ) return;
        SelectedIndex = ( SelectedIndex - 1 + Items.Count ) % Items.Count;
    }

    public InventoryItem GetSelectedItem()
    {
        if ( Items.Count == 0 || SelectedIndex >= Items.Count ) return null;
        return Items[SelectedIndex];
    }

    public void UseSelected()
    {
        InventoryItem item = GetSelectedItem();
        if ( item == null ) return;

        ThrowableItem throwable = item as ThrowableItem;

        if ( throwable != null )
        {
            Vector3 targetPos = GetMouseTargetPosition( throwable.ThrowRange );
            targetPos.y = transform.position.y;

            item.transform.SetParent( null );
            item.transform.position = transform.position;
            item.gameObject.SetActive( true );

            throwable.Throw( targetPos );
        }
        else
        {
            item.transform.SetParent( null );
            item.transform.position = transform.position + transform.forward * 2f;
            item.transform.gameObject.SetActive( true );
            item.OnUse();
        }

        item.Quantity--;

        if ( item.Quantity <= 0 )
        {
            Items.RemoveAt( SelectedIndex );
            if ( SelectedIndex >= Items.Count ) SelectedIndex = Mathf.Max( 0, Items.Count - 1 );
        }
    }

    Vector3 GetMouseTargetPosition( float maxRange )
    {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if ( Physics.Raycast( ray, out hit, maxRange, _groundLayer ) )
        {
            return hit.point;
        }

        return ray.GetPoint( maxRange );
    }
}
