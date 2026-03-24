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

    public override void OnUpdate( EnemyStateMachine machine )
    {
        if ( !machine.Enemy.IsPlayerDetected )
        {
            machine.ChangeState( _onPlayerLost );
            return;
        }

        Transform player = machine.Enemy.PlayerTarget;

        if ( player == null ) return;

        machine.Enemy.NavAgent.SetDestination( player.position );

        float distanceToPlayer = Vector3.Distance( machine.transform.position, player.position );

        if ( distanceToPlayer <= machine.Enemy.AttackRange )
        {
            machine.Enemy.NavAgent.isStopped = true;
            machine.Enemy.Attack();
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
