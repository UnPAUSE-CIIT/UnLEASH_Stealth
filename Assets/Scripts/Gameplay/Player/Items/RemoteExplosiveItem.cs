// ============================================================================================
// File: RemoteExplosiveItem.cs
// Description: Use this to create a remote explosive item. Detonates on command.
// ============================================================================================

using UnityEngine;

public class RemoteExplosiveItem : ThrowableItem
{
    [Header( "Explosive Settings" )]
    [SerializeField] float _blastRadius = 5f;
    [SerializeField] int _damage = 50;

    public float BlastRadius => _blastRadius;
    public int Damage => _damage;

    Vector3 _explosionPosition;

    public override void OnUse()
    {
        GameObject player = GameObject.FindGameObjectWithTag( "Player" );

        if ( player == null ) return;

        Vector3 throwPosition = player.transform.position + player.transform.forward * _throwRange;

        GameObject explosive = new GameObject( "RemoteExplosive" );
        explosive.transform.position = player.transform.position;

        RemoteExplosiveItem explosiveComponent = explosive.AddComponent<RemoteExplosiveItem>();
        explosiveComponent._blastRadius = _blastRadius;
        explosiveComponent._damage = _damage;
        explosiveComponent.ItemName = ItemName;

        explosiveComponent.Throw( throwPosition );

        Debug.Log( $"Remote explosive placed! Press Q to detonate." );
    }

    protected override void OnImpact()
    {
        _explosionPosition = transform.position;
    }

    public void Detonate()
    {
        BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();

        foreach ( BaseEnemy enemy in enemies )
        {
            float distance = Vector3.Distance( enemy.transform.position, _explosionPosition );

            if ( distance <= _blastRadius )
            {
                enemy.TakeDamage( _damage );
            }
        }

        Debug.Log( $"Explosive detonated! {_blastRadius}m radius, {_damage} damage." );

        Destroy( gameObject );
    }
}
