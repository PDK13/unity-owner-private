using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class ShapeCircleCreator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController m_spriteShape;

    [Space]
    [Min(2f)] public float FilledRadius = 2f;

    [Space]
    [Min(1)] public int FilledCut = 1;

    [Space]
    [Min(1f)] public float HollowRadius = 1f;
    [Min(0)] public int HollowCut = 0;

    [Space]
    public QShapeCircle Circum;

    public void SetGenerateFilled()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new QShapeCircle(m_spriteShape);
        //
        m_spriteShape.spline.isOpenEnded = false;
        Circum.SetFilledGenerate(FilledRadius);
    }

    public void SetGenerateFilledOpen()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new QShapeCircle(m_spriteShape);
        //
        m_spriteShape.spline.isOpenEnded = true;
        Circum.SetFilledGenerate(FilledRadius, FilledCut);
    }

    public void SetGenerateHollow()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new QShapeCircle(m_spriteShape);
        //
        m_spriteShape.spline.isOpenEnded = false;
        Circum.SetHollowGenerate(FilledRadius, HollowRadius, HollowCut);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(ShapeCircleCreator))]
public class ShapeCreatorEditor : Editor
{
    private ShapeCircleCreator m_target;

    private SerializedProperty m_spriteShape;

    private SerializedProperty FilledRadius;
    private SerializedProperty FilledCut;

    private SerializedProperty HollowRadius;
    private SerializedProperty HollowCut;

    private void OnEnable()
    {
        m_target = target as ShapeCircleCreator;
        //
        m_spriteShape = QEditorCustom.GetField(this, "m_spriteShape");
        //
        FilledRadius = QEditorCustom.GetField(this, "FilledRadius");
        FilledCut = QEditorCustom.GetField(this, "FilledCut");
        //
        HollowRadius = QEditorCustom.GetField(this, "HollowRadius");
        HollowCut = QEditorCustom.GetField(this, "HollowCut");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditorCustom.SetField(m_spriteShape);
        //
        QEditorCustom.SetField(FilledRadius);
        //
        if (QEditor.SetButton("Generate Filled"))
            m_target.SetGenerateFilled();
        //
        QEditorCustom.SetField(FilledCut);
        //
        if (QEditor.SetButton("Generate Filled Open"))
            m_target.SetGenerateFilledOpen();
        //
        QEditorCustom.SetField(HollowRadius);
        QEditorCustom.SetField(HollowCut);
        //
        if (QEditor.SetButton("Generate Hollow"))
            m_target.SetGenerateHollow();
        //
        QEditorCustom.SetApply(this);
    }
}

#endif