// ============================================================================================
// File: ThrowableItem.cs
// Description: Use this as a base for throwable items. Handles arc trajectory.
// ============================================================================================

using UnityEngine;

public class ThrowableItem : InventoryItem
{
    [Header( "Throwable Settings" )]
    [SerializeField] protected float _throwRange = 10f;
    [SerializeField] protected float _throwHeight = 3f;

    public float ThrowRange => _throwRange;

    Vector3 _startPos;
    Vector3 _endPos;
    float _progress;
    bool _isFlying;

    public void Throw( Vector3 targetPos )
    {
        _startPos = transform.position;
        _endPos = targetPos;
        _progress = 0f;
        _isFlying = true;
    }

    void Update()
    {
        if ( !_isFlying ) return;

        _progress += Time.deltaTime * 2f;

        if ( _progress >= 1f )
        {
            transform.position = _endPos;
            _isFlying = false;
            OnImpact();
            return;
        }

        Vector3 flatPos = Vector3.Lerp( _startPos, _endPos, _progress );
        float height = Mathf.Sin( _progress * Mathf.PI ) * _throwHeight;
        transform.position = flatPos + Vector3.up * height;
    }

    protected virtual void OnImpact()
    {
        Debug.Log( $"Throwable landed at {_endPos}" );
    }
}
