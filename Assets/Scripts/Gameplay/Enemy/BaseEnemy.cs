// ============================================================================================
// File: BaseEnemy.cs
// Description: Use this as the base class for all enemies. Attach to a GameObject with EnemyStateMachine.
// ============================================================================================

using UnityEngine;
using UnityEngine.AI;

[RequireComponent( typeof( NavMeshAgent ) )]
public class BaseEnemy : MonoBehaviour
{
    [Header( "Detection Settings" )]
    [SerializeField] float _viewDistance = 10f;
    [SerializeField] float _viewAngle = 90f;
    [SerializeField] float _detectionHeight = 2f;

    [Header( "Combat Settings" )]
    [SerializeField] int _damage = 10;
    [SerializeField] float _attackRange = 1.5f;
    [SerializeField] float _attackCooldown = 1f;

    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] Transform _playerTarget;
    [field: SerializeField] public bool IsPlayerDetected { get; private set; }

    float _lastAttackTime;

    public NavMeshAgent NavAgent => _navAgent;
    public Transform PlayerTarget => _playerTarget;
    public int Damage => _damage;
    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;

    protected virtual void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag( "Player" );

        if ( player != null )
        {
            _playerTarget = player.transform;
        }
    }

    protected virtual void Update()
    {
        if ( _playerTarget == null ) return;

        CheckPlayerDetection();
    }

    protected virtual void CheckPlayerDetection()
    {
        Vector3 eyePosition = transform.position + Vector3.up * _detectionHeight;
        Vector3 directionToPlayer = _playerTarget.position - eyePosition;
        float distanceToPlayer = directionToPlayer.magnitude;

        if ( distanceToPlayer > _viewDistance )
        {
            IsPlayerDetected = false;
            return;
        }

        directionToPlayer.Normalize();
        float angle = Vector3.Angle( transform.forward, directionToPlayer );

        if ( angle > _viewAngle / 2f )
        {
            IsPlayerDetected = false;
            return;
        }

        if ( !Physics.Linecast( eyePosition, _playerTarget.position ) )
        {
            IsPlayerDetected = true;
        }
    }

    public virtual void Attack()
    {
        if ( Time.time - _lastAttackTime < AttackCooldown ) return;

        float distanceToPlayer = Vector3.Distance( transform.position, _playerTarget.position );

        if ( distanceToPlayer <= AttackRange )
        {
            Debug.Log( $"{name} attacks player for {Damage} damage!" );
            _lastAttackTime = Time.time;
        }
    }

    public void GoToPosition( Vector3 position )
    {
        NavAgent.SetDestination( position );
    }

    public void TakeDamage( int damage )
    {
        Debug.Log( $"{name} took {damage} damage!" );
    }
}
