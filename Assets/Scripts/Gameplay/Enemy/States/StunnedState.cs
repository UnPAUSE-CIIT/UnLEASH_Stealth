// ============================================================================================
// File: StunnedState.cs
// Description: Use this to make the enemy stunned and unable to move or detect the player.
// ============================================================================================

using UnityEngine;

[CreateAssetMenu( menuName = "Enemy States/Stunned" )]
public class StunnedState : EnemyState
{
    [Header( "Stun Settings" )]
    [SerializeField] float _stunDuration = 5f;

    float _stunTimer;

    public override void OnEnter( EnemyStateMachine machine )
    {
        _stunTimer = 0f;
        machine.Enemy.NavAgent.isStopped = true;
    }

    public override void OnUpdate( EnemyStateMachine machine )
    {
        _stunTimer += Time.deltaTime;

        if ( _stunTimer >= _stunDuration )
        {
            machine.ChangeState( machine.GetState<IdleState>() );
        }
    }

    public override void OnExit( EnemyStateMachine machine )
    {
        machine.Enemy.NavAgent.isStopped = false;
    }
}
