using DG.Tweening;
using QuickMethode;
using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TweenMovePath : MonoBehaviour
{
    #region Enum

    public enum ElevatorTween 
    {
        Rigidbody, //Got Physic Event!!
        Transform, //Non-Physic event!!
    }

    public enum ElevatorActive 
    {
        Start, 
        Wait,
    }

    public enum ElevatorPath 
    {
        Local, 
        World, 
    }

    #endregion

    #region Event

    public Action onStart;
    public Action onUpdate;
    public Action onKill;

    #endregion

    #region Varible: Elevator

    [SerializeField] private ElevatorActive m_activeType = ElevatorActive.Start;
    //
    [SerializeField] private ElevatorTween m_tweenType = ElevatorTween.Rigidbody;
    [SerializeField] private Ease m_easeType = Ease.Linear;
    [SerializeField] private ElevatorPath m_pathType = ElevatorPath.Local;

    public ElevatorActive MoveType => m_activeType;
    public ElevatorPath PathType => m_pathType;
    //
    [SerializeField] [Min(0)] private float m_duration = 4f;
    [SerializeField] [Min(0)] private float m_timeScaleRevert = 1f;
    
    [SerializeField] private List<Vector2> m_pathList;
    private Vector2 m_posStart;

    public int PathCount => m_pathList.Count;

    #endregion

    private Tweener m_tweenMove;
    //
    [SerializeField] protected Collider2D m_colliderBase;
    [SerializeField] protected Rigidbody2D m_rigidbodyBase;

    protected virtual void Awake()
    {
        m_posStart = transform.position;
        //
        if (m_tweenType == ElevatorTween.Rigidbody && m_rigidbodyBase == null)
            m_tweenType = ElevatorTween.Transform;
        //
        if (m_pathType == ElevatorPath.Local)
        {
            for (int i = 0; i < PathCount; i++)
                m_pathList[i] += m_posStart;
        }
        //
        if (m_activeType == ElevatorActive.Start)
            SetStart();
    }

    protected virtual void OnDestroy()
    {
        if (m_tweenMove != null)
            m_tweenMove.Kill();
        //
        transform.DOKill();
    }

    #region ======================================= Elevator

    #region Tween Move

    protected void SetStart()
    {
        if (PathCount == 0)
            return;
        //
        if (m_tweenMove != null)
        {
            SetMove();
        }
        else
        {
            switch (m_tweenType)
            {
                case ElevatorTween.Transform:
                    Vector3[] Path = new Vector3[PathCount];
                    for (int i = 0; i < PathCount; i++) 
                        Path[i] = m_pathList[i];
                    //
                    m_tweenMove = transform.DOPath(Path, m_duration).SetEase(m_easeType).SetLoops(-1, LoopType.Yoyo)
                        .OnStart(() => onStart?.Invoke())
                        .OnUpdate(() => onUpdate?.Invoke())
                        .OnKill(() => onKill?.Invoke());
                    break;
                case ElevatorTween.Rigidbody:
                    m_tweenMove = m_rigidbodyBase.DOPath(m_pathList.ToArray(), m_duration).SetEase(m_easeType).SetLoops(-1, LoopType.Yoyo)
                        .OnStart(() => onStart?.Invoke())
                        .OnUpdate(() => onUpdate?.Invoke())
                        .OnKill(() => onKill?.Invoke());
                    break;
            }
        }
    }

    protected void SetMove()
    {
        if (PathCount == 0)
            return;
        //
        if (m_tweenMove == null)
            SetStart();
        else
            m_tweenMove.PlayForward();
        //
        m_tweenMove.timeScale = 1;
    }

    protected void SetMoveInvert()
    {
        if (m_tweenMove == null)
            return;
        //
        m_tweenMove.timeScale = m_timeScaleRevert;
        m_tweenMove.PlayBackwards();
    }

    protected void SetMoveStop()
    {
        if (m_tweenMove == null)
            return;
        //
        m_tweenMove.timeScale = 0;
    }

    #endregion

    #endregion

    #region Editor

    public Vector2[] GetPath()
    {
        return m_pathList.ToArray();
    }

    public Vector3 GetPath(int Index)
    {
        if (Index > PathCount - 1)
            return Vector3.zero;
        //
        return m_pathList[Index];
    }

    public void SetPath(Vector3 Pos, int Index)
    {
        if (Index > PathCount - 1)
            return;
        //
        m_pathList[Index] = (Vector2)Pos;
    }

    public void SetPath(Vector2 Pos, int Index)
    {
        if (Index > PathCount - 1)
            return;
        //
        m_pathList[Index] = Pos;
    }

    public void SetPathAdd()
    {
        if (PathCount == 0)
            m_pathList.Add(Vector2.right * 10f);
        else
        if (PathCount > 0)
            m_pathList.Add(m_pathList[PathCount - 1] + Vector2.right * 10f);
    }

    public void SetPathRemove()
    {
        if (PathCount > 0)
            m_pathList.RemoveAt(PathCount - 1);
    }

    #endregion

    protected virtual void OnDrawGizmosSelected()
    {
        if (m_colliderBase == null)
            return;

        if (m_pathList != null)
        {
            if (!Application.isPlaying)
            {
                for (int i = 0; i < PathCount; i++)
                {
                    switch (m_pathType)
                    {
                        case ElevatorPath.Local:
                            QGizmos.SetCollider2D(transform.TransformPoint(m_pathList[i]), m_colliderBase, Color.gray);
                            break;
                        case ElevatorPath.World:
                            QGizmos.SetCollider2D(m_pathList[i], m_colliderBase, Color.gray);
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < PathCount; i++)
                    QGizmos.SetCollider2D(m_pathList[i], m_colliderBase, Color.gray);
            }
        }
    }
}

#if UNITY_EDITOR

[CanEditMultipleObjects]
[CustomEditor(typeof(TweenMovePath))]
public class TweenMovePathEditor : Editor
{
    private TweenMovePath m_target;

    private SerializedProperty m_activeType;
    //
    private SerializedProperty m_tweenType;
    private SerializedProperty m_easeType;
    private SerializedProperty m_pathType;
    //
    private SerializedProperty m_duration;
    private SerializedProperty m_timeScaleRevert;
    //
    private SerializedProperty m_pathList;
    //
    private SerializedProperty m_colliderBase;
    private SerializedProperty m_rigidbodyBase;

    void OnEnable()
    {
        m_target = (target as TweenMovePath);
        //
        m_activeType = serializedObject.FindProperty("m_activeType");
        //
        m_tweenType = serializedObject.FindProperty("m_tweenType");
        m_easeType = serializedObject.FindProperty("m_easeType");
        m_pathType = serializedObject.FindProperty("m_pathType");
        //
        m_duration = serializedObject.FindProperty("m_duration");
        m_timeScaleRevert = serializedObject.FindProperty("m_timeScaleRevert");

        m_pathList = serializedObject.FindProperty("m_pathList");

        m_colliderBase = serializedObject.FindProperty("m_colliderBase");
        m_rigidbodyBase = serializedObject.FindProperty("m_rigidbodyBase");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditor.SetDisableGroupBegin(Application.isPlaying);
        //
        QEditorCustom.SetField(m_activeType);
        //
        QEditor.SetSpace(10);
        //
        QEditorCustom.SetField(m_tweenType);
        QEditorCustom.SetField(m_easeType);
        QEditorCustom.SetField(m_pathType);
        //
        QEditor.SetSpace(10);
        //
        QEditorCustom.SetField(m_duration);
        QEditorCustom.SetField(m_timeScaleRevert);
        //
        QEditor.SetSpace(10);
        //
        if (QEditor.SetButton("Add Path"))
            m_target.SetPathAdd();
        if (QEditor.SetButton("Remove Path"))
            m_target.SetPathRemove();
        //
        QEditorCustom.SetField(m_pathList);
        //
        QEditor.SetSpace(10);
        //
        QEditorCustom.SetField(m_colliderBase);
        QEditorCustom.SetField(m_rigidbodyBase);
        //
        QEditor.SetDisableGroupEnd();
        //
        QEditorCustom.SetApply(this);
    }

    void OnSceneGUI()
    {
        if (Application.isPlaying)
            return;
        //
        Handles.color = Color.red;
        //
        for (int i = 0; i < m_target.PathCount; i++)
        {
            if (m_target.PathType == TweenMovePath.ElevatorPath.World)
            {
                EditorGUI.BeginChangeCheck();
                Vector2 WorldPos = Handles.PositionHandle(m_target.GetPath()[i], Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_target, "change path");
                    m_target.SetPath((Vector3)WorldPos, i);
                }
                //
                if (i != 0)
                    Handles.DrawDottedLine(m_target.GetPath()[i], m_target.GetPath()[i - 1], 10);
                else
                    Handles.DrawDottedLine(m_target.GetPath()[i], m_target.transform.position, 10);
            }
            else
            if (m_target.PathType == TweenMovePath.ElevatorPath.Local)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 WorldPos = m_target.transform.TransformPoint(m_target.GetPath(i));
                WorldPos = Handles.PositionHandle(WorldPos, Quaternion.identity);
                //
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "change path");
                    m_target.SetPath((Vector2)m_target.transform.InverseTransformPoint(WorldPos), i);
                }
                //
                if (i != 0)
                {
                    Vector3 WorldPosLast = m_target.transform.TransformPoint(m_target.GetPath(i - 1));
                    Handles.DrawDottedLine(WorldPos, WorldPosLast, 10);
                }
                else
                {
                    Vector3 WorldPosLast = m_target.transform.TransformPoint(Vector3.zero);
                    Handles.DrawDottedLine(WorldPos, WorldPosLast, 10);
                }
            }
        }
    }
}

#endif