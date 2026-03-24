// ============================================================================================
// File: PlayerController.cs
// Description: You can use this script to give the player movement, pickup items, and hide in objects.
// ============================================================================================

using UnityEngine;

[RequireComponent( typeof( CharacterController ) )]
public class PlayerController : MonoBehaviour
{
    [Header( "Movement Settings" )]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _runSpeed = 8f;
    [SerializeField] float _crouchSpeed = 2.5f;

    [Header( "Pickup Settings" )]
    [SerializeField] float _pickupRange = 2f;
    [SerializeField] LayerMask _pickupLayer;

    [Header( "Hide Settings" )]
    [SerializeField] float _hideRadius = 1.5f;
    [SerializeField] LayerMask _hideableLayer;

    [SerializeField] CharacterController _characterController;
    [field: SerializeField] public bool IsHidden { get; private set; }

    PickupItem _currentPickup;
    Hideable _currentHideable;
    bool _isCrouching;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if ( IsHidden )
        {
            HandleUnhide();
            return;
        }

        HandleMovement();
        HandlePickup();
        HandleHide();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis( "Horizontal" );
        float vertical = Input.GetAxis( "Vertical" );

        Vector3 direction = new Vector3( horizontal, 0f, vertical ).normalized;

        _isCrouching = Input.GetKey( KeyCode.LeftControl );

        float speed = _isCrouching ? _crouchSpeed : ( Input.GetKey( KeyCode.LeftShift ) ? _runSpeed : _moveSpeed );

        Vector3 moveDirection = transform.TransformDirection( direction ) * speed;

        moveDirection.y = -9.81f;

        _characterController.Move( moveDirection * Time.deltaTime );
    }

    void HandlePickup()
    {
        if ( Input.GetKeyDown( KeyCode.E ) )
        {
            if ( _currentPickup != null )
            {
                _currentPickup.Pickup();
                _currentPickup = null;
            }
            else
            {
                TryFindPickup();
            }
        }
    }

    void TryFindPickup()
    {
        Collider[] colliders = Physics.OverlapSphere( transform.position, _pickupRange, _pickupLayer );

        if ( colliders.Length > 0 )
        {
            PickupItem pickup = colliders[0].GetComponent<PickupItem>();

            if ( pickup != null )
            {
                _currentPickup = pickup;
            }
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

        PickupItem pickup = other.GetComponent<PickupItem>();
        if ( pickup != null )
        {
            _currentPickup = pickup;
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
}
