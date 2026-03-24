// ============================================================================================
// File: EnemyStateMachine.cs
// Description: Use this to manage enemy states. Assign states in the inspector to configure enemy behavior.
// ============================================================================================

using UnityEngine;

[RequireComponent( typeof( BaseEnemy ) )]
public class EnemyStateMachine : MonoBehaviour
{
    [Header( "States" )]
    [SerializeField] EnemyState _initialState;
    [SerializeField] EnemyState[] _states;

    [Header( "References" )]
    [SerializeField] BaseEnemy _enemy;

    [SerializeField] EnemyState _currentState;

    public BaseEnemy Enemy => _enemy;
    public EnemyState CurrentState => _currentState;

    void Start()
    {
        if ( _initialState != null )
        {
            ChangeState( _initialState );
        }
    }

    void Update()
    {
        _currentState?.OnUpdate( this );
    }

    public void ChangeState( EnemyState newState )
    {
        if ( newState == null ) return;

        if ( _currentState != null )
        {
            _currentState.OnExit( this );
        }

        _currentState = newState;
        _currentState.OnEnter( this );
    }

    public T GetState<T>() where T : EnemyState
    {
        foreach ( EnemyState state in _states )
        {
            if ( state is T typedState )
            {
                return typedState;
            }
        }
        return null;
    }
}
