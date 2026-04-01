// ============================================================================================
// File: PlayerController.cs
// Description: You can use this script to give the player movement, pickup items, and hide in objects.
// ============================================================================================

using UnityEngine;
using KBCore.Refs;

[RequireComponent( typeof( CharacterController ) )]
public class PlayerController : MonoBehaviour
{
    [Header( "Movement Settings" )]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _runSpeed = 8f;

    [Header( "Pickup Settings" )]
    [SerializeField] float _pickupRange = 2f;
    [SerializeField] LayerMask _pickupLayer;

    [Header( "Hide Settings" )]
    [SerializeField] float _hideRadius = 1.5f;
    [SerializeField] LayerMask _hideableLayer;

    [Header( "Inventory" )]
    [SerializeField, Self] Inventory _inventory;

    [SerializeField, Self] CharacterController _characterController;
    [field: SerializeField] public bool IsHidden { get; private set; }
    [SerializeField, Child] Animator _model;

    InventoryItem _currentPickup;
    Hideable _currentHideable;

    void OnValidate()
    {
        this.ValidateRefs();
    }

    void Update()
    {
        if ( IsHidden )
        {
            HandleUnhide();
            return;
        }

        HandleMovement();
		HandleAnimation();
        HandlePickup();
        HandleHide();
        HandleUseItem();
    }

    void HandleUseItem()
    {
        if ( _inventory == null ) return;

        float scroll = Input.mouseScrollDelta.y;

        if ( scroll > 0 )
        {
            _inventory.SelectNext();
        }
        else if ( scroll < 0 )
        {
            _inventory.SelectPrevious();
        }

        if ( Input.GetKeyDown( KeyCode.Q ) )
        {
            _inventory.UseSelected();
        }
    }

	float GetSpeed()
	{
		return Input.GetKey( KeyCode.LeftShift ) ? _runSpeed : _moveSpeed;
	}

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw( "Horizontal" );
        float vertical = Input.GetAxisRaw( "Vertical" );

        Vector3 direction = new Vector3( horizontal, 0f, vertical ).normalized;

        Vector3 moveDirection = transform.TransformDirection( direction ) * GetSpeed();

        _characterController.SimpleMove( moveDirection );

		if ( direction.sqrMagnitude > 0 )
			_model.transform.rotation = Quaternion.LookRotation( direction );
    }

	void HandleAnimation()
	{
		_model.SetFloat( "Speed", _characterController.velocity.magnitude / _runSpeed );
	}

    void HandlePickup()
    {
        if ( Input.GetKeyDown( KeyCode.E ) && _currentPickup != null )
        {
            InventoryItem item = _currentPickup.GetComponent<InventoryItem>();

            if ( item != null )
            {
                item.transform.SetParent( transform );
                item.gameObject.SetActive( false );

                _inventory.AddItem( item );
                Debug.Log( $"Picked up: {item.ItemName}" );
            }

            _currentPickup = null;
        }
    }

    void HandleHide()
    {
        if ( Input.GetKeyDown( KeyCode.H ) )
        {
            if ( _currentHideable != null )
            {
                EnterHide();
            }
            else
            {
                TryFindHideable();
            }
        }
    }

    void TryFindHideable()
    {
        Collider[] colliders = Physics.OverlapSphere( transform.position, _hideRadius, _hideableLayer );

        if ( colliders.Length > 0 )
        {
            Hideable hideable = colliders[0].GetComponent<Hideable>();

            if ( hideable != null && !hideable.IsOccupied )
            {
                _currentHideable = hideable;
            }
        }
    }

    void EnterHide()
    {
        IsHidden = true;
        _currentHideable.Occupy( this );
        _characterController.enabled = false;
    }

    void HandleUnhide()
    {
        if ( Input.GetKeyDown( KeyCode.H ) )
        {
            ExitHide();
        }
    }

    public void ExitHide()
    {
        if ( _currentHideable != null )
        {
            transform.position = _currentHideable.GetExitPosition().position;
            _currentHideable.Vacate();
            _currentHideable = null;
        }

        IsHidden = false;
        _characterController.enabled = true;
    }

    void OnTriggerStay( Collider other )
    {
        if ( IsHidden ) return;

        if ( other.TryGetComponent<PickupItem>( out var pickup ) )
        {
            _currentPickup = pickup.GetInventoryItem();
        }

        Hideable hideable = other.GetComponent<Hideable>();
        if ( hideable != null && !hideable.IsOccupied )
        {
            _currentHideable = hideable;
        }
    }

    void OnTriggerExit( Collider other )
    {
        if ( other.GetComponent<PickupItem>() == _currentPickup )
        {
            _currentPickup = null;
        }

        if ( other.GetComponent<Hideable>() == _currentHideable )
        {
            _currentHideable = null;
        }
    }

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawCube( transform.position, new Vector3( 1, 2, 1 ) );
	}
}
