// ============================================================================================
// File: StunItem.cs
// Description: Use this to create a stun item. Temporarily stuns an enemy.
// ============================================================================================

using UnityEngine;

public class StunItem : ThrowableItem
{
    [Header( "Stun Settings" )]
    [SerializeField] float _stunDuration = 5f;
    [SerializeField] float _effectRadius = 3f;

    public float StunDuration => _stunDuration;
    public float EffectRadius => _effectRadius;

    protected override void OnImpact()
    {
        BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();

        foreach ( BaseEnemy enemy in enemies )
        {
            float distance = Vector3.Distance( enemy.transform.position, transform.position );

            if ( distance <= _effectRadius )
            {
                EnemyStateMachine stateMachine = enemy.GetComponent<EnemyStateMachine>();

                if ( stateMachine != null )
                {
                    StunnedState stunnedState = stateMachine.GetState<StunnedState>();

                    if ( stunnedState != null )
                    {
                        stateMachine.ChangeState( stunnedState );
                        Debug.Log( $"{enemy.name} is stunned!" );
                    }
                }
            }
        }

        Debug.Log( $"Stun activated! Enemies within {_effectRadius}m stunned for {_stunDuration} seconds." );

        Destroy( gameObject );
    }
}
