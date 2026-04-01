// ============================================================================================
// File: PatrolState.cs
// Description: Use this to make the enemy patrol between waypoints. The enemy will automatically find waypoints in the scene named "Waypoint_[EnemyName]_X".
// ============================================================================================

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    Transform[] FindWaypoints( Transform enemyTransform )
    {
        string waypointPrefix = $"Waypoint_{enemyTransform.name}";
        List<Transform> waypoints = new List<Transform>();

        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach ( GameObject obj in allObjects )
        {
            if ( obj.name.StartsWith( waypointPrefix ) )
            {
                waypoints.Add( obj.transform );
            }
        }

        if ( waypoints.Count == 0 ) return null;

        waypoints.Sort( ( a, b ) => a.name.CompareTo( b.name ) );

        return waypoints.ToArray();
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
