// ============================================================================================
// File: LDTool.cs
// Description: Use this editor window to access level design utilities like snapping objects to grid and baking NavMesh.
// Author: Seifer Albacete
// ============================================================================================

using UnityEngine;
using UnityEditor;
using Unity.AI.Navigation;

public class LDTool : EditorWindow
{
    bool _isWaypointEditing;

    [MenuItem( "Tools/LDTool" )]
    public static void ShowWindow()
    {
        GetWindow<LDTool>( "Helper" );
    }

    [MenuItem( "Tools/Snap to Grid %&s", true )]
    static bool ValidateSnapToGrid()
    {
        return Selection.activeTransform != null;
    }

    [MenuItem( "Tools/Snap to Grid %&s" )]
    static void SnapToGrid()
    {
		if ( Selection.activeTransform == null )
			return;

        Transform selected = Selection.activeTransform;
        Vector3 position = selected.position;

        position.x = Mathf.Round( position.x * 2f ) / 2f;
        position.y = Mathf.Round( position.y * 2f ) / 2f;
        position.z = Mathf.Round( position.z * 2f ) / 2f;

        selected.position = position;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField( "UnLEASH Helper", EditorStyles.boldLabel );

        if ( GUILayout.Button( "Snap to Grid (Ctrl+Alt+S)" ) )
        {
            SnapToGrid();
        }

        if ( GUILayout.Button( "Bake NavMesh" ) )
        {
            BakeNavMesh();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField( "Waypoint Editor", EditorStyles.boldLabel );

        _isWaypointEditing = EditorGUILayout.Toggle( "Edit Waypoints", _isWaypointEditing );

        if ( _isWaypointEditing )
        {
            EditorGUILayout.HelpBox( "Hold Shift and click in the Scene view to add waypoints as children of the selected enemy.", MessageType.Info );
        }
    }

    void OnSceneGUI()
    {
        if ( !_isWaypointEditing ) return;

        if ( Selection.activeGameObject == null )
        {
            return;
        }

        BaseEnemy enemy = Selection.activeGameObject.GetComponent<BaseEnemy>();

        if ( enemy == null )
        {
            return;
        }

        Event currentEvent = Event.current;

        if ( currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && currentEvent.shift )
        {
            Vector3 clickPoint = HandleUtility.GUIPointToWorldRay( currentEvent.mousePosition ).GetPoint( 10f );
            clickPoint.y = Selection.activeTransform.position.y;

            CreateWaypoint( clickPoint );
            currentEvent.Use();
        }
    }

    void CreateWaypoint( Vector3 position )
    {
        int waypointCount = 0;

        for ( int i = 0; i < Selection.activeTransform.childCount; i++ )
        {
            if ( Selection.activeTransform.GetChild( i ).name.StartsWith( "Waypoint" ) )
            {
                waypointCount++;
            }
        }

        GameObject waypoint = new GameObject( $"Waypoint_{waypointCount}" );
        waypoint.transform.position = position;
        waypoint.transform.parent = Selection.activeTransform;

        Undo.RegisterCreatedObjectUndo( waypoint, "Create Waypoint" );

        Debug.Log( $"Created waypoint at {position}" );
    }

    static void BakeNavMesh()
    {
        var surfaces = FindObjectsByType<NavMeshSurface>( FindObjectsSortMode.None );

        if ( surfaces.Length == 0 )
        {
            Debug.LogWarning( "No NavMeshSurface found in scene." );
            return;
        }

        foreach ( var surface in surfaces )
        {
            surface.BuildNavMesh();
        }
    }
}
