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

    protected override void OnImpact()
    {
    }

    public void Detonate()
    {
        BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();

        foreach ( BaseEnemy enemy in enemies )
        {
            float distance = Vector3.Distance( enemy.transform.position, transform.position );

            if ( distance <= _blastRadius )
            {
                enemy.TakeDamage( _damage );
            }
        }

        Debug.Log( $"Explosive detonated! {_blastRadius}m radius, {_damage} damage." );

        Destroy( gameObject );
    }
}
