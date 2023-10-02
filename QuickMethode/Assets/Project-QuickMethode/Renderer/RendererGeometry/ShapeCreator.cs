using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class ShapeCreator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController m_spriteShape;

    [Space]
    [Min(2f)] public float FilledRadius = 2f;

    [Space]
    [Min(1)] public float HollowRadius = 1f;
    [Min(0)] public int HollowCut = 1;

    [Space]
    public QShapeCircum Circum;

    public void SetGenerateFilled()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new QShapeCircum(m_spriteShape);
        //
        Circum.SetFilledGenerate(FilledRadius);
    }

    public void SetGenerateHollow()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new QShapeCircum(m_spriteShape);
        //
        Circum.SetHollowGenerate(FilledRadius, HollowRadius, HollowCut);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(ShapeCreator))]
public class ShapeCreatorEditor : Editor
{
    private ShapeCreator m_target;

    private SerializedProperty m_spriteShape;

    private SerializedProperty FilledRadius;

    private SerializedProperty HollowRadius;
    private SerializedProperty HollowCut;

    private void OnEnable()
    {
        m_target = target as ShapeCreator;
        //
        m_spriteShape = QEditorCustom.GetField(this, "m_spriteShape");
        //
        FilledRadius = QEditorCustom.GetField(this, "FilledRadius");
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