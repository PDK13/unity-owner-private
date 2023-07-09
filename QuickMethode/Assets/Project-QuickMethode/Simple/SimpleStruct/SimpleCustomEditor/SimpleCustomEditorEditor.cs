using UnityEditor;
using UnityEngine;

//Script file can use '#if UNITY_EDITOR' to avoid build muti line of code, but file is still in build.
//Script file can put inside folder 'Editor' (Folder can be exist every where in project) to avoid build it.

#if UNITY_EDITOR

[CustomEditor(typeof(SimpleCustomEditor))]
public class SimpleCustomEditorEditor : Editor
{
    private SimpleCustomEditor m_target;

    private SerializedProperty m_AnotherVaribleA;

    private SerializedProperty m_AnotherVaribleB;
    private SerializedProperty m_AnotherVaribleC;

    private SerializedProperty m_AnotherVariblePath;

    private void OnEnable()
    {
        m_target = (target as SimpleCustomEditor);

        m_AnotherVaribleA = serializedObject.FindProperty("m_VaribleA");

        m_AnotherVaribleB = serializedObject.FindProperty("m_VaribleB");
        m_AnotherVaribleC = serializedObject.FindProperty("m_VaribleC");

        m_AnotherVariblePath = serializedObject.FindProperty("m_VariblePath");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck(); //Begin Change Check

        EditorGUILayout.PropertyField(m_AnotherVaribleA); //Show varible

        if (m_target.m_VaribleA == SimpleCustomEditor.Option.Option1) 
        {
            EditorGUILayout.PropertyField(m_AnotherVaribleB);
        }

        serializedObject.ApplyModifiedProperties(); //Apply All Chagce

        if (EditorGUI.EndChangeCheck()) //End Change Check
        {
            m_target.SetOptionDebug(); //Call Methode from Sript or Component
        }

        //EditorGUILayout.PropertyField(m_AnotherVaribleB);
        EditorGUILayout.PropertyField(m_AnotherVaribleC);

        EditorGUILayout.PropertyField(m_AnotherVariblePath);

        serializedObject.ApplyModifiedProperties(); //Apply All Chagce
    }

    private void OnSceneGUI()
    {
        SimpleCustomEditor m_Temp = (target as SimpleCustomEditor);

        if (m_Temp != null)
        {
            //Draw some line from GameObject

            Vector3[] m_Path = m_Temp.GetPath();

            Handles.color = Color.red; //Color line
            Handles.DrawPolyLine(m_Path); //Draw line

            for (int i = 0; i < m_Path.Length; i++)
            {
                EditorGUI.BeginChangeCheck();

                Vector3 m_HandlePos = Handles.PositionHandle(m_Path[i], Quaternion.identity); //...???

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_Temp, "change path"); //...???

                    m_Temp.SetPath(m_HandlePos, i);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif