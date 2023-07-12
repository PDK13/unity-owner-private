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

    private SerializedProperty m_idAuto;

    private void OnEnable()
    {
        m_target = (target as UniqueID);

        m_idAuto = serializedObject.FindProperty("m_idAuto");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_idAuto);

        if (QGameObject.GetCheckPrefab(m_target.gameObject))
        {
            if (QEditor.SetButton("Refresh"))
                m_target.SetUpdateRefresh();
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif