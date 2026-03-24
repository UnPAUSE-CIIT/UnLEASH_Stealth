// ============================================================================================
// File: LureItem.cs
// Description: Use this to create a lure item. Distracts enemies to a specific location.
// ============================================================================================

using UnityEngine;

public class LureItem : ThrowableItem
{
    [Header( "Lure Settings" )]
    [SerializeField] float _distractionRadius = 5f;
    [SerializeField] float _duration = 10f;

    public float DistractionRadius => _distractionRadius;
    public float Duration => _duration;

    public override void OnUse()
    {
        GameObject player = GameObject.FindGameObjectWithTag( "Player" );

        if ( player == null ) return;

        Vector3 throwPosition = player.transform.position + player.transform.forward * _throwRange;

        GameObject lure = new GameObject( "Lure" );
        lure.transform.position = player.transform.position;

        LureItem lureComponent = lure.AddComponent<LureItem>();
        lureComponent._distractionRadius = _distractionRadius;
        lureComponent._duration = _duration;
        lureComponent.ItemName = ItemName;

        lureComponent.Throw( throwPosition );

        Debug.Log( $"Lure thrown!" );
    }

    protected override void OnImpact()
    {
        BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();

        foreach ( BaseEnemy enemy in enemies )
        {
            float distance = Vector3.Distance( enemy.transform.position, transform.position );

            if ( distance <= _distractionRadius )
            {
                enemy.GoToPosition( transform.position );
                Debug.Log( $"{enemy.name} is distracted by the lure!" );
            }
        }

        Debug.Log( $"Lure active! Enemies within {_distractionRadius}m distracted for {_duration} seconds." );

        Destroy( gameObject, _duration );
    }
}
