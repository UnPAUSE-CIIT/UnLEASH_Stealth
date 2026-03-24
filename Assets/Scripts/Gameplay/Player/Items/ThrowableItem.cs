// ============================================================================================
// File: ThrowableItem.cs
// Description: Use this as a base for throwable items. Handles arc trajectory.
// ============================================================================================

using UnityEngine;

public class ThrowableItem : InventoryItem
{
    [Header( "Throwable Settings" )]
    [SerializeField] protected float _throwRange = 10f;
    [SerializeField] protected float _throwArc = 5f;
    [SerializeField] protected float _throwSpeed = 15f;

    public float ThrowRange => _throwRange;
    public float ThrowArc => _throwArc;
    public float ThrowSpeed => _throwSpeed;

    protected Vector3 _targetPosition;
    protected bool _isFlying;

    public virtual void Throw( Vector3 targetPos )
    {
        _targetPosition = targetPos;
        _isFlying = true;
    }

    protected void Update()
    {
        if ( !_isFlying ) return;

        Vector3 direction = _targetPosition - transform.position;
        float distance = direction.magnitude;

        if ( distance < 0.5f )
        {
            _isFlying = false;
            OnImpact();
            return;
        }

        direction.Normalize();

        float heightDiff = _targetPosition.y - transform.position.y;
        float arcModifier = Mathf.Sin( distance * Mathf.PI / _throwRange ) * _throwArc;

        Vector3 moveDir = direction * _throwSpeed * Time.deltaTime;
        moveDir.y = ( -heightDiff / distance + arcModifier ) * _throwSpeed * Time.deltaTime;

        transform.position += moveDir;

        transform.LookAt( _targetPosition );
    }

    protected virtual void OnImpact()
    {
        Debug.Log( $"Throwable landed at {_targetPosition}" );
    }
}
