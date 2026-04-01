// ============================================================================================
// File: BaseEnemyEditor.cs
// Description: Use this to customize how BaseEnemy appears in the inspector and edit waypoints.
// ============================================================================================

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

[CustomEditor( typeof( BaseEnemy ) )]
public class BaseEnemyEditor : Editor
{
    bool _showWaypoints = true;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();

        GUIStyle buttonStyle = new GUIStyle( GUI.skin.button );
        buttonStyle.fontStyle = FontStyle.Bold;

        if ( _showWaypoints )
        {
            if ( GUILayout.Button( "Hide Waypoints", buttonStyle ) )
            {
                _showWaypoints = false;
            }
        }
        else
        {
            if ( GUILayout.Button( "Show Waypoints", buttonStyle ) )
            {
                _showWaypoints = true;
            }
        }

        if ( _showWaypoints )
        {
            EditorGUILayout.HelpBox( "Shift + Click in Scene view to add waypoints. Drag red spheres to move them.", MessageType.Info );
        }
    }

    void OnSceneGUI()
    {
        BaseEnemy enemy = (BaseEnemy)target;

        if ( !_showWaypoints ) return;

        Event currentEvent = Event.current;

        if ( currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && currentEvent.shift )
        {
            Vector3 clickPoint = HandleUtility.GUIPointToWorldRay( currentEvent.mousePosition ).GetPoint( 10f );
            clickPoint.y = enemy.transform.position.y;

            CreateWaypoint( clickPoint, enemy.transform );
            currentEvent.Use();
        }

        Transform[] waypoints = GetWaypoints( enemy.transform );

        if ( waypoints == null || waypoints.Length == 0 ) return;

        for ( int i = 0; i < waypoints.Length; i++ )
        {
            Vector3 waypointPos = waypoints[i].position;

            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle( waypointPos, Quaternion.identity );

            if ( EditorGUI.EndChangeCheck() )
            {
                Undo.RecordObject( waypoints[i], "Move Waypoint" );
                waypoints[i].position = newPos;
            }

            float size = HandleUtility.GetHandleSize( waypointPos ) * 0.4f;

            Handles.color = Color.yellow;
            Handles.DrawSolidDisc( waypointPos, Vector3.up, size * 0.8f );
            Handles.DrawWireDisc( waypointPos, Vector3.up, size * 1.3f );

            GUIStyle style = new GUIStyle();
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.yellow;
            Handles.Label( waypointPos + Vector3.up * ( size + 0.3f ), $"Waypoint {i}", style );

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

        Handles.color = Color.yellow;
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 12;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = Color.cyan;
        Handles.Label( enemy.transform.position + Vector3.up * 3f, "PATROL PATH", labelStyle );
    }

    void CreateWaypoint( Vector3 position, Transform enemyTransform )
    {
        string waypointPrefix = $"Waypoint_{enemyTransform.name}";
        int waypointCount = 0;

        GameObject[] allObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach ( GameObject obj in allObjects )
        {
            if ( obj.name.StartsWith( waypointPrefix ) )
            {
                waypointCount++;
            }
        }

        GameObject waypoint = new GameObject( $"{waypointPrefix}_{waypointCount}" );
        waypoint.transform.position = position;

        Undo.RegisterCreatedObjectUndo( waypoint, "Create Waypoint" );
    }

    Transform[] GetWaypoints( Transform parent )
    {
        string waypointPrefix = $"Waypoint_{parent.name}";
        List<Transform> waypoints = new List<Transform>();

        GameObject[] allObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

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
}
