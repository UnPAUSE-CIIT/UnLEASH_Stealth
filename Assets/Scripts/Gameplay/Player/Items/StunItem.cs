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

    public override void OnUse()
    {
        GameObject player = GameObject.FindGameObjectWithTag( "Player" );

        if ( player == null ) return;

        Vector3 throwPosition = player.transform.position + player.transform.forward * _throwRange;

        GameObject stun = new GameObject( "StunGrenade" );
        stun.transform.position = player.transform.position;

        StunItem stunComponent = stun.AddComponent<StunItem>();
        stunComponent._stunDuration = _stunDuration;
        stunComponent._effectRadius = _effectRadius;
        stunComponent.ItemName = ItemName;

        stunComponent.Throw( throwPosition );

        Debug.Log( $"Stun grenade thrown!" );
    }

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
