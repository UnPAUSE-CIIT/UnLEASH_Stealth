// ============================================================================================
// File: EnemyState.cs
// Description: Use this to create custom enemy states as ScriptableObjects. Right-click to create new states.
// ============================================================================================

using UnityEngine;

public abstract class EnemyState : ScriptableObject
{
    [Header( "State Info" )]
    [field: SerializeField] public string StateName { get; private set; } = "New State";

    public virtual void OnEnter( EnemyStateMachine machine ) { }
    public virtual void OnUpdate( EnemyStateMachine machine ) { }
    public virtual void OnExit( EnemyStateMachine machine ) { }
}
