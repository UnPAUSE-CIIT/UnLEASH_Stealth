// ============================================================================================
// File: PatrolState.cs
// Description: Use this to make the enemy patrol between waypoints. The enemy will automatically find child objects named "Waypoint".
// ============================================================================================

using UnityEngine;

[CreateAssetMenu( menuName = "Enemy States/Patrol" )]
public class PatrolState : EnemyState
{
    [Header( "Patrol Settings" )]
    [SerializeField] float _waitTime = 2f;

    Transform[] _waypoints;
    int _currentWaypointIndex;
    float _waitTimer;

    public override void OnEnter( EnemyStateMachine machine )
    {
        _currentWaypointIndex = 0;
        _waitTimer = 0f;

        _waypoints = FindWaypoints( machine.transform );
    }

    Transform[] FindWaypoints( Transform parent )
    {
        Transform[] children = new Transform[parent.childCount];
        int waypointCount = 0;

        for ( int i = 0; i < parent.childCount; i++ )
        {
            Transform child = parent.GetChild( i );

            if ( child.name.StartsWith( "Waypoint" ) )
            {
                children[waypointCount] = child;
                waypointCount++;
            }
        }

        if ( waypointCount == 0 ) return null;

        Transform[] result = new Transform[waypointCount];

        for ( int i = 0; i < waypointCount; i++ )
        {
            result[i] = children[i];
        }

        return result;
    }

    public override void OnUpdate( EnemyStateMachine machine )
    {
        if ( machine.Enemy.IsPlayerDetected )
        {
            machine.ChangeState( machine.GetState<ChaseState>() );
            return;
        }

        if ( _waypoints == null || _waypoints.Length == 0 ) return;

        Transform targetWaypoint = _waypoints[_currentWaypointIndex];
        machine.Enemy.NavAgent.SetDestination( targetWaypoint.position );

        float distanceToWaypoint = Vector3.Distance( machine.transform.position, targetWaypoint.position );

        if ( distanceToWaypoint < 1f )
        {
            _waitTimer += Time.deltaTime;

            if ( _waitTimer >= _waitTime )
            {
                _waitTimer = 0f;
                _currentWaypointIndex = ( _currentWaypointIndex + 1 ) % _waypoints.Length;
            }
        }
    }
}
