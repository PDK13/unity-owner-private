using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class ShapeCreator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController m_spriteShape;

    private QShapeCircum m_circum;

    [Space]
    [Min(2f)] public float FilledRadius = 2f;
    public float FilledDeg = 0f;

    [Space]
    [Min(1)] public float HollowRadius = 1f;
    [Min(0)] public int HollowCut = 1;

    [Space]
    public Vector3[] Points;

    public void SetGenerate()
    {

    }

    public void SetGenerateFilled()
    {
        if (m_circum == null)
            m_circum = new QShapeCircum(m_spriteShape);
        //
        m_circum.SetFilledGenerate(FilledRadius);
        //
        Points = m_circum.Points;
    }

    public void SetGenerateHollow()
    {
        if (m_circum == null)
            m_circum = new QShapeCircum(m_spriteShape);
        //
        m_circum.SetHollowGenerate(FilledRadius, HollowRadius, HollowCut);
        //
        Points = m_circum.Points;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(ShapeCreator))]
public class ShapeCreatorEditor : Editor
{
    private ShapeCreator m_target;

    private SerializedProperty m_spriteShape;

    private SerializedProperty FilledRadius;
    private SerializedProperty FilledDeg;

    private SerializedProperty HollowRadius;
    private SerializedProperty HollowCut;

    private SerializedProperty Points;

    private void OnEnable()
    {
        m_target = target as ShapeCreator;
        //
        m_spriteShape = QEditorCustom.GetField(this, "m_spriteShape");
        //
        FilledRadius = QEditorCustom.GetField(this, "FilledRadius");
        FilledDeg = QEditorCustom.GetField(this, "FilledDeg");
        //
        HollowRadius = QEditorCustom.GetField(this, "HollowRadius");
        HollowCut = QEditorCustom.GetField(this, "HollowCut");
        //
        Points = QEditorCustom.GetField(this, "Points");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditorCustom.SetField(m_spriteShape);
        //
        QEditorCustom.SetField(FilledRadius);
        QEditorCustom.SetField(FilledDeg);
        //
        if (QEditor.SetButton("Generate Filled"))
            m_target.SetGenerateFilled();
        //
        QEditorCustom.SetField(HollowRadius);
        QEditorCustom.SetField(HollowCut);
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