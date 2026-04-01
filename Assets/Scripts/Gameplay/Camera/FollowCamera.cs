// ============================================================================================
// File: FollowCamera.cs
// Description: Use this to create a top-down follow camera.
// ============================================================================================

using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header( "Target" )]
    [SerializeField] Transform _target;

    [Header( "Settings" )]
    [SerializeField] float _distance = 10f;
    [SerializeField] float _height = 8f;
    [SerializeField] float _rotationX = 75f;
    [SerializeField] float _smoothSpeed = 5f;

    void LateUpdate()
    {
        if ( _target == null ) return;

        Vector3 targetPosition = _target.position;
        targetPosition.y += _height;
        targetPosition -= Vector3.forward * _distance;

        transform.position = Vector3.Lerp( transform.position, targetPosition, _smoothSpeed * Time.deltaTime );
    }

    void Start()
    {
        transform.rotation = Quaternion.Euler( _rotationX, 0f, 0f );
    }
}
