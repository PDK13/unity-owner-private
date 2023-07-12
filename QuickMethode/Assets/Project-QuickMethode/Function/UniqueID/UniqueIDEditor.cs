using QuickMethode;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(UniqueID))]
public class UniqueIDEditor : Editor
{
    private UniqueID m_target;

    private SerializedProperty m_id;

    private void OnEnable()
    {
        m_target = (target as UniqueID);

        m_id = serializedObject.FindProperty("m_id");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_id);

        if (QGameObject.GetCheckPrefab(m_target.gameObject))
        {
            if (QEditor.SetButton("Refresh"))
                m_target.SetUpdateRefresh();
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif