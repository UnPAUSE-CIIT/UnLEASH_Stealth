// ============================================================================================
// File: Hideable.cs
// Description: Use this to create objects the player can hide in (lockers, cardboard boxes).
// ============================================================================================

using UnityEngine;

public class Hideable : MonoBehaviour
{
    [Header( "Hide Settings" )]
    [SerializeField] Transform _exitPosition;
    [field: SerializeField] public bool IsOccupied { get; private set; }

    public void Occupy( PlayerController player )
    {
        IsOccupied = true;
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }

    public void Vacate()
    {
        IsOccupied = false;
    }

    public Transform GetExitPosition()
    {
        return _exitPosition;
    }
}
