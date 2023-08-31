#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(RendererGeometryShape))]
[CanEditMultipleObjects]
public class RendererGeometryShapeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RendererGeometryShape Target = target as RendererGeometryShape;

        serializedObject.Update();

        if (QEditor.SetButton("Apply Shape"))
        {
            Target.SetShape();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif