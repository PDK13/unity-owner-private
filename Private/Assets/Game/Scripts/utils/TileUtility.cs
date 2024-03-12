#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileUtility : MonoBehaviour
{
    public TileBase from;
    public TileBase to;

    public void Change()
    {
        Tilemap t = GetComponent<Tilemap>();
        t.SwapTile(from, to);
    }
}

[CustomEditor(typeof(TileUtility))]
public class TileUtilityEditor : Editor
{
    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Replace Tile"))
        {
            (target as TileUtility).Change();
        }
    }
}
#endif
