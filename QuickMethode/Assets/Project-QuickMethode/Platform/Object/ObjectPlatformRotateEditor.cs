using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using QuickMethode;

#if UNITY_EDITOR

[CustomEditor(typeof(ObjectPlatformRotate))]
public class ObjectPlatformRotateEditor : Editor
{
    ObjectPlatformRotate m_target;

    private SerializedProperty m_dir;
    private SerializedProperty m_speed;
    private SerializedProperty m_circleCollider;
    private SerializedProperty m_radius;

    private void OnEnable()
    {
        m_target = (target as ObjectPlatformRotate);

        m_dir = serializedObject.FindProperty("m_dir");
        m_speed = serializedObject.FindProperty("m_speed");
        m_circleCollider = serializedObject.FindProperty("m_circleCollider");
        m_radius = serializedObject.FindProperty("m_radius");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_dir);
        EditorGUILayout.PropertyField(m_speed);
        QEditor.SetChanceCheckBegin();
        EditorGUILayout.PropertyField(m_circleCollider);
        if (QEditor.SetChanceCheckEnd())
            if (!m_target.CircleCollider)
                EditorGUILayout.PropertyField(m_radius);
        serializedObject.ApplyModifiedProperties();
    }
}

#endif