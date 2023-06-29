using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectRotate))]
public class ObjectRotateEditor : Editor
{
    private SerializedProperty m_key;
    private SerializedProperty m_timeRotate;
    private SerializedProperty m_useLimits;
    private SerializedProperty m_minAngle;
    private SerializedProperty m_maxAngle;
    private SerializedProperty m_clockwise;
    private SerializedProperty m_loop;
    private SerializedProperty m_loopType;
    private SerializedProperty m_canInvert;

    private ObjectRotate m_target;
    private Vector2 m_localMin;
    private Vector2 m_localMax;
    private bool m_update;

    void OnEnable()
    {
        m_key = serializedObject.FindProperty("m_key");
        m_timeRotate = serializedObject.FindProperty("m_timeRotate");
        m_useLimits = serializedObject.FindProperty("m_useLimits");
        m_minAngle = serializedObject.FindProperty("m_minAngle");
        m_maxAngle = serializedObject.FindProperty("m_maxAngle");
        m_clockwise = serializedObject.FindProperty("m_clockwise");
        m_loop = serializedObject.FindProperty("m_loop");
        m_loopType = serializedObject.FindProperty("m_loopType");
        m_canInvert = serializedObject.FindProperty("m_canInvert");

        m_target = target as ObjectRotate;
        UpdateLocalPos();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_key);
        EditorGUILayout.PropertyField(m_timeRotate);
        EditorGUILayout.PropertyField(m_clockwise);
        EditorGUILayout.PropertyField(m_loop);
        EditorGUILayout.PropertyField(m_canInvert);
        if (m_loop.boolValue)
            EditorGUILayout.PropertyField(m_loopType);
        EditorGUILayout.PropertyField(m_useLimits);
        if (m_useLimits.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_minAngle);
            EditorGUILayout.PropertyField(m_maxAngle);
            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                m_update = true;
                UpdateLocalPos();
            }
            else
                m_update = false;
        }
        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        if (!m_useLimits.boolValue)
            return;
        Handles.color = Color.blue;
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        m_minAngle.vector2Value = Handles.PositionHandle(m_minAngle.vector2Value, Quaternion.identity);
        m_maxAngle.vector2Value = Handles.PositionHandle(m_maxAngle.vector2Value, Quaternion.identity);
        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
            UpdateLocalPos();
        else
            if (!m_update)
            m_target.UpdateLocalPosition(m_localMin, m_localMax);
    }

    void UpdateLocalPos()
    {
        m_localMin = m_target.localMinAngle;
        m_localMax = m_target.localMaxAngle;
    }
}
