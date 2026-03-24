// ============================================================================================
// File: BaseEnemyEditor.cs
// Description: Use this to customize how BaseEnemy appears in the inspector.
// ============================================================================================

using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( BaseEnemy ) )]
public class BaseEnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    void OnSceneGUI()
    {
        BaseEnemy enemy = (BaseEnemy)target;

        if ( enemy.transform.childCount == 0 ) return;

        Transform[] waypoints = GetWaypoints( enemy.transform );

        if ( waypoints == null || waypoints.Length == 0 ) return;

        for ( int i = 0; i < waypoints.Length; i++ )
        {
            Vector3 waypointPos = waypoints[i].position;

            Handles.color = Color.yellow;
            Handles.DrawSolidDisc( waypointPos, Vector3.up, 0.3f );

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.yellow;
            Handles.Label( waypointPos + Vector3.up * 0.5f, $"Waypoint {i}", style );

            if ( i > 0 )
            {
                Handles.color = new Color( 1f, 1f, 0f, 0.3f );
                Handles.DrawLine( waypoints[i - 1].position, waypointPos );
            }
        }

        if ( waypoints.Length > 1 )
        {
            Handles.color = new Color( 1f, 1f, 0f, 0.3f );
            Handles.DrawLine( waypoints[waypoints.Length - 1].position, waypoints[0].position );
        }
    }

    Transform[] GetWaypoints( Transform parent )
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
}
