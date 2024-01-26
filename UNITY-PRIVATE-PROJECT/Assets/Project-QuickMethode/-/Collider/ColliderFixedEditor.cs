#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ColliderFixed))]
public class ColliderFixedEditor : Editor
{
    private SerializedProperty m_baseCollider;

    private SerializedProperty m_fixed;

    private SerializedProperty m_fixedX;
    private SerializedProperty m_fixedOffsetX;

    private SerializedProperty m_fixedY;
    private SerializedProperty m_fixedOffsetY;

    private void OnEnable()
    {
        m_baseCollider = serializedObject.FindProperty("m_baseCollider");

        m_fixed = serializedObject.FindProperty("m_fixed");

        m_fixedX = serializedObject.FindProperty("m_fixedX");
        m_fixedOffsetX = serializedObject.FindProperty("m_fixedOffsetX");

        m_fixedY = serializedObject.FindProperty("m_fixedY");
        m_fixedOffsetY = serializedObject.FindProperty("m_fixedOffsetY");
    }

    public override void OnInspectorGUI()
    {
        ColliderFixed Target = target as ColliderFixed;

        serializedObject.Update();

        EditorGUILayout.PropertyField(m_baseCollider);

        if (Target.BaseCollider)
        {
            EditorGUILayout.PropertyField(m_fixed);

            EditorGUILayout.PropertyField(m_fixedX);
            if (Target.FixedX != DirectionX.None)
            {
                EditorGUILayout.PropertyField(m_fixedOffsetX);
            }

            EditorGUILayout.PropertyField(m_fixedY);
            if (Target.FixedY != DirectionY.None)
            {
                EditorGUILayout.PropertyField(m_fixedOffsetY);
            }

            QUnityEditor.SetSpace(10f);
            if (QUnityEditor.SetButton("Fixed"))
            {
                Target.SetFixed();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif