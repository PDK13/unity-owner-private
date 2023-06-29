using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectShaping))]
public class ObjectShapingEditor : Editor
{
    private ObjectShaping m_shaper;

    private SerializedProperty m_type;
    private SerializedProperty radius;
    private SerializedProperty childSpace;
    private SerializedProperty m_targetLocalNodes; 
    private SerializedProperty m_pathLength;

    private void OnEnable() 
    {
        m_shaper = target as ObjectShaping;

        m_type = serializedObject.FindProperty("shapeType");
        radius = serializedObject.FindProperty("radius");
        childSpace = serializedObject.FindProperty("childSpace");
        m_pathLength = serializedObject.FindProperty("pathLength");

        m_targetLocalNodes = serializedObject.FindProperty("localNodes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_type);
        serializedObject.ApplyModifiedProperties();
        if(m_shaper.shapeType == ShapingType.Circle)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(radius);
            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
                m_shaper.UpdateCirlePosition();
        }
        else if(m_shaper.shapeType == ShapingType.Polygon)
        {
            m_shaper.UpdatePathLength();

            if(GUILayout.Button("Add Node"))
            {
                Undo.RecordObject(target, "added node");
                Vector3 position = m_shaper.localNodes[m_shaper.localNodes.Length - 1] + Vector3.right;

                int index = m_targetLocalNodes.arraySize;
                m_targetLocalNodes.InsertArrayElementAtIndex(index);
                m_targetLocalNodes.GetArrayElementAtIndex(index).vector3Value = position;
            }
            EditorGUIUtility.labelWidth = 64;
            int delete = -1;

            for(int i = 0; i < m_shaper.localNodes.Length; ++i)
            {
                int size = 64;
                EditorGUILayout.BeginVertical(GUILayout.Width(size));
                EditorGUILayout.LabelField("Node " + i, GUILayout.Width(size));

                if (i != 0)
                {
                    EditorGUILayout.PropertyField(m_targetLocalNodes.GetArrayElementAtIndex(i), new GUIContent("Local Pos:"), GUILayout.Width(size * 5));
                }                        

                if (i != 0 && GUILayout.Button("Delete Node " + i, GUILayout.Width(size * 3)))
                {
                    delete = i;
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUIUtility.labelWidth = 0;

            if(delete != -1)
            {
                m_targetLocalNodes.DeleteArrayElementAtIndex(delete);
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(m_pathLength);

            if(GUILayout.Button("Refresh"))
            {
                m_shaper.UpdatePolygonPosition();
            }
        }
    }
    private void OnSceneGUI() 
    {
        if(m_shaper.shapeType == ShapingType.Circle)
            return;

        for (int i = 0; i < m_shaper.localNodes.Length; ++i)
        {
            Vector3 worldPos;
            if (Application.isPlaying)
            {
                worldPos = m_shaper.localNodes[i];
            }
            else
            {
                worldPos = m_shaper.transform.TransformPoint(m_shaper.localNodes[i]);
            }
            Vector3 newWorld = worldPos; 
            if(i != 0)
                newWorld = Handles.PositionHandle(worldPos, Quaternion.identity);

            Handles.color = Color.red;
            if(i != 0)
            {
                if (Application.isPlaying)
                {
                    Handles.DrawDottedLine(worldPos, m_shaper.localNodes[i - 1], 10);
                }
                else 
                {
                    Handles.DrawDottedLine(worldPos, m_shaper.transform.TransformPoint(m_shaper.localNodes[i - 1]), 10);
                }

                if (worldPos != newWorld)
                {
                    Undo.RecordObject(target, "moved point");
                    
                    m_targetLocalNodes.GetArrayElementAtIndex(i).vector3Value = m_shaper.transform.InverseTransformPoint(newWorld);
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
