using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCreator : MonoBehaviour
{
    public List<Vector2> Points;

    public void SetCreate()
    {
        if (GetComponent<MeshFilter>() == null)
            return;
        //
        GetComponent<MeshFilter>().mesh = QMesh.GetMesh(Points);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MeshCreator))]
public class MeshCreatorEditor : Editor
{
    private MeshCreator m_target;

    private SerializedProperty Points;

    private void OnEnable()
    {
        m_target = target as MeshCreator;
        //
        Points = QEditorCustom.GetField(this, "Points");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditorCustom.SetField(Points);
        //
        if (QEditor.SetButton("Generate"))
            m_target.SetCreate();
        //
        QEditorCustom.SetApply(this);
    }
}

#endif