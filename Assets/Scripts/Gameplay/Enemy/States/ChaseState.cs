// ============================================================================================
// File: ChaseState.cs
// Description: Use this to make the enemy chase the player. The enemy will attack when in range.
// ============================================================================================

using UnityEngine;

[CreateAssetMenu( menuName = "Enemy States/Chase" )]
public class ChaseState : EnemyState
{
    [Header( "Chase Settings" )]
    [SerializeField] EnemyState _onPlayerLost;
    [SerializeField] float _chaseStopDistance = 2f;
    [SerializeField] float _chaseLostDelay = 3f;

    float _chaseLostTimer;

    public override void OnEnter( EnemyStateMachine machine )
    {
        _chaseLostTimer = 0f;
    }

    public override void OnUpdate( EnemyStateMachine machine )
    {
        if ( !machine.Enemy.IsPlayerDetected )
        {
            _chaseLostTimer += Time.deltaTime;

            if ( _chaseLostTimer >= _chaseLostDelay )
            {
                machine.ChangeState( _onPlayerLost );
            }
            return;
        }

        _chaseLostTimer = 0f;

        Transform player = machine.Enemy.PlayerTarget;

        if ( player == null ) return;

        machine.Enemy.NavAgent.SetDestination( player.position );

        float distanceToPlayer = Vector3.Distance( machine.transform.position, player.position );

        if ( distanceToPlayer <= _chaseStopDistance )
        {
            machine.Enemy.NavAgent.isStopped = true;

            if ( distanceToPlayer <= machine.Enemy.AttackRange )
            {
                machine.Enemy.Attack();
            }
        }
        else
        {
            machine.Enemy.NavAgent.isStopped = false;
        }
    }

    public override void OnExit( EnemyStateMachine machine )
    {
        machine.Enemy.NavAgent.isStopped = false;
    }
}
