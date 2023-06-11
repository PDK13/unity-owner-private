#if UNITY_EDITOR

using QuickMethode;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(TweenMovePath))]
public class TweenMovePathEditor : Editor
{
    private SerializedProperty m_tween;
    private SerializedProperty m_ease;
    private SerializedProperty m_elevator;

    private SerializedProperty m_key;
    private SerializedProperty m_keyStop;
    private SerializedProperty m_once;

    private SerializedProperty m_time;
    private SerializedProperty m_timeRatioRevert;
    private SerializedProperty m_path;

    void OnEnable()
    {
        m_tween = serializedObject.FindProperty("m_tween");
        m_ease = serializedObject.FindProperty("m_ease");
        m_elevator = serializedObject.FindProperty("m_elevator");

        m_key = serializedObject.FindProperty("m_key");
        m_keyStop = serializedObject.FindProperty("m_keyStop");
        m_once = serializedObject.FindProperty("m_once");

        m_time = serializedObject.FindProperty("m_time");
        m_timeRatioRevert = serializedObject.FindProperty("m_timeRatioRevert");
        m_path = serializedObject.FindProperty("m_path");
    }

    public override void OnInspectorGUI()
    {
        TweenMovePath Target = (target as TweenMovePath);

        serializedObject.Update();

        //EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_tween);
        EditorGUILayout.PropertyField(m_ease);
        EditorGUILayout.PropertyField(m_elevator);

        QEditor.SetSpace(10);

        if (Target.Elevator == TweenMovePath.ElevatorMove.Key)
        {
            EditorGUILayout.PropertyField(m_key);
            EditorGUILayout.PropertyField(m_once);
        }
        //EditorGUI.EndChangeCheck();
        EditorGUILayout.PropertyField(m_keyStop);

        QEditor.SetSpace(10);

        EditorGUILayout.PropertyField(m_time);
        EditorGUILayout.PropertyField(m_timeRatioRevert);
        
        EditorGUILayout.PropertyField(m_path);
        serializedObject.ApplyModifiedProperties();

        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        TweenMovePath Target = (target as TweenMovePath);

        if (Target == null)
            return;

        Vector2[] Path = Target.GetPath();

        if (Path == null)
            return;

        if (Path.Length == 0)
            return;
        
        Vector3[] PathConvert = new Vector3[Path.Length];
        for (int i = 0; i < Path.Length; i++) PathConvert[i] = (Vector3)Path[i];

        Handles.color = Color.red;
        Handles.DrawPolyLine(PathConvert);

        for (int i = 0; i < Path.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector2 Pos = Handles.PositionHandle(Path[i], Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(Target, "change path");
                Target.SetUpdatePath(Pos, i);
            }
        }
    }
}

#endif