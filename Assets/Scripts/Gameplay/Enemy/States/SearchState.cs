// ============================================================================================
// File: SearchState.cs
// Description: Use this to make the enemy search the last known player position, then return to a default state.
// ============================================================================================

using UnityEngine;

[CreateAssetMenu( menuName = "Enemy States/Search" )]
public class SearchState : EnemyState
{
    [Header( "Search Settings" )]
    [SerializeField] float _searchTime = 5f;
    [SerializeField] EnemyState _onSearchComplete;

    float _searchTimer;
    Vector3 _lastKnownPosition;

    public override void OnEnter( EnemyStateMachine machine )
    {
        _searchTimer = 0f;
        _lastKnownPosition = machine.Enemy.PlayerTarget.position;
        machine.Enemy.NavAgent.SetDestination( _lastKnownPosition );
    }

    public override void OnUpdate( EnemyStateMachine machine )
    {
        if ( machine.Enemy.IsPlayerDetected )
        {
            machine.ChangeState( machine.GetState<ChaseState>() );
            return;
        }

        _searchTimer += Time.deltaTime;

        if ( _searchTimer >= _searchTime )
        {
            machine.ChangeState( _onSearchComplete );
            return;
        }

        float distanceToLastKnown = Vector3.Distance( machine.transform.position, _lastKnownPosition );

        if ( distanceToLastKnown < 1f )
        {
            _searchTimer = _searchTime;
        }
    }
}
