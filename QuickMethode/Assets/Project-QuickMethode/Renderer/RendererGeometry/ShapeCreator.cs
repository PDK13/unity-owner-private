using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class ShapeCreator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController m_spriteShape;

    private QMeshCircum m_circum;

    [Space]
    [Min(3)] public int FilledPoints = 3;
    [Min(0)] public float FilledRadius = 2f;
    public float FilledDeg = 0f;

    [Space]
    [Min(0)] public float HollowRadius = 0f;

    [Space]
    public Vector3[] Points;

    public void SetGenerate()
    {

    }

    public void SetGenerateFilled()
    {

    }

    public void SetGenerateHollow()
    {

    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(ShapeCreator))]
public class ShapeCreatorEditor : Editor
{
    private ShapeCreator m_target;

    private SerializedProperty m_spriteShape;

    private SerializedProperty FilledPoints;
    private SerializedProperty FilledRadius;
    private SerializedProperty FilledDeg;

    private SerializedProperty HollowRadius;

    private SerializedProperty Points;

    private void OnEnable()
    {
        m_target = target as ShapeCreator;
        //
        m_spriteShape = QEditorCustom.GetField(this, "m_spriteShape");
        //
        FilledPoints = QEditorCustom.GetField(this, "FilledPoints");
        FilledRadius = QEditorCustom.GetField(this, "FilledRadius");
        FilledDeg = QEditorCustom.GetField(this, "FilledDeg");
        //
        HollowRadius = QEditorCustom.GetField(this, "HollowRadius");
        //
        Points = QEditorCustom.GetField(this, "Points");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditorCustom.SetField(m_spriteShape);
        //
        QEditorCustom.SetField(FilledPoints);
        QEditorCustom.SetField(FilledRadius);
        QEditorCustom.SetField(FilledDeg);
        //
        if (QEditor.SetButton("Generate Filled"))
            m_target.SetGenerateFilled();
        //
        QEditorCustom.SetField(HollowRadius);
        //
        if (QEditor.SetButton("Generate Hollow"))
            m_target.SetGenerateHollow();
        //
        QEditorCustom.SetField(Points);
        //
        if (QEditor.SetButton("Generate"))
            m_target.SetGenerate();
        //
        QEditorCustom.SetApply(this);
    }
}

#endif