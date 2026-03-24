// ============================================================================================
// File: IdleState.cs
// Description: Use this to make the enemy do nothing. Transitions to Chase when player is detected.
// ============================================================================================

using UnityEngine;

[CreateAssetMenu( menuName = "Enemy States/Idle" )]
public class IdleState : EnemyState
{
    public override void OnUpdate( EnemyStateMachine machine )
    {
        if ( machine.Enemy.IsPlayerDetected )
        {
            machine.ChangeState( machine.GetState<ChaseState>() );
        }
    }
}
