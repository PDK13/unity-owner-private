#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using QuickMethode;

[CustomEditor(typeof(RendererGeometryShape))]
[CanEditMultipleObjects]
public class RendererGeometryShapeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RendererGeometryShape Target = target as RendererGeometryShape;

        serializedObject.Update();

        if (QEditor.SetButton("Apply Shape"))
            Target.SetShape();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif