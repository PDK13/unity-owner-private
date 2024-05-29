using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectPath : MonoBehaviour
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

    [SerializeField] private ElevatorTween m_tweenType = ElevatorTween.Rigidbody;
    [SerializeField] private Ease m_easeType = Ease.Linear;
    [SerializeField] private ElevatorPath m_pathType = ElevatorPath.Local;

    [SerializeField] private bool m_loop = true;
    [SerializeField] private bool m_loopInfinite = true;
    [SerializeField][Min(1)] private int m_loopCount = 1;
    [SerializeField] private LoopType m_loopType = LoopType.Yoyo;

    [SerializeField][Min(0)] private float m_duration = 4f;
    [SerializeField][Min(0)] private float m_durationRevertScale = 1f;

    [SerializeField] private List<Vector2> m_pathList;
    private Vector2 m_posStart;

    public int PathCount => m_pathList.Count;

    #endregion

    #region Varible: Component

    [SerializeField] protected Collider2D m_colliderBase;
    [SerializeField] protected Rigidbody2D m_rigidbodyBase;

    #endregion

    #region Get

    public (bool Loop, bool Infinite, int Count) TweenLoop => (m_loop, m_loop ? m_loopInfinite : false, m_loop ? (m_loopInfinite ? -1 : m_loopCount) : 0);

    public ElevatorActive MoveType => m_activeType;

    public ElevatorPath PathType => m_pathType;

    #endregion

    private Tweener m_tweenMove;

    protected virtual void Awake()
    {
        m_posStart = transform.position;
        //
        if (m_tweenType == ElevatorTween.Rigidbody && m_rigidbodyBase == null)
        {
            m_tweenType = ElevatorTween.Transform;
        }
        //
        if (m_pathType == ElevatorPath.Local)
        {
            for (int i = 0; i < PathCount; i++)
            {
                m_pathList[i] += m_posStart;
            }
        }
        //
        if (m_activeType == ElevatorActive.Start)
        {
            SetStart();
        }
    }

    protected virtual void OnDestroy()
    {
        if (m_tweenMove != null)
        {
            m_tweenMove.Kill();
        }
        //
        transform.DOKill();
    }

    #region ======================================= Elevator

    #region Tween Move

    [ContextMenu("Start")]
    public void SetStart()
    {
        if (PathCount == 0)
        {
            return;
        }
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
                    {
                        Path[i] = m_pathList[i];
                    }
                    //
                    if (m_loop)
                    {
                        m_tweenMove = transform.DOPath(Path, m_duration).SetEase(m_easeType).SetLoops(m_loopInfinite ? -1 : m_loopCount, m_loopType)
                            .OnStart(() => onStart?.Invoke())
                            .OnUpdate(() => onUpdate?.Invoke())
                            .OnKill(() => onKill?.Invoke());
                    }
                    else
                    {
                        m_tweenMove = transform.DOPath(Path, m_duration).SetEase(m_easeType)
                            .OnStart(() => onStart?.Invoke())
                            .OnUpdate(() => onUpdate?.Invoke())
                            .OnKill(() => onKill?.Invoke());
                    }
                    break;
                case ElevatorTween.Rigidbody:
                    if (m_loop)
                    {
                        m_tweenMove = m_rigidbodyBase.DOPath(m_pathList.ToArray(), m_duration).SetEase(m_easeType).SetLoops(m_loopInfinite ? -1 : m_loopCount, m_loopType)
                            .OnStart(() => onStart?.Invoke())
                            .OnUpdate(() => onUpdate?.Invoke())
                            .OnKill(() => onKill?.Invoke());
                    }
                    else
                    {
                        m_tweenMove = m_rigidbodyBase.DOPath(m_pathList.ToArray(), m_duration).SetEase(m_easeType)
                            .OnStart(() => onStart?.Invoke())
                            .OnUpdate(() => onUpdate?.Invoke())
                            .OnKill(() => onKill?.Invoke());
                    }
                    break;
            }
        }
    }

    [ContextMenu("Move")]
    public void SetMove()
    {
        if (PathCount == 0)
        {
            return;
        }
        //
        if (m_tweenMove == null)
        {
            SetStart();
        }
        else
        {
            m_tweenMove.PlayForward();
        }
        //
        m_tweenMove.timeScale = 1;
    }

    [ContextMenu("Move Revert")]
    public void SetMoveRevert()
    {
        if (m_tweenMove == null)
        {
            return;
        }
        //
        m_tweenMove.timeScale = m_durationRevertScale;
        m_tweenMove.PlayBackwards();
    }

    [ContextMenu("Move Stop")]
    public void SetMoveStop()
    {
        if (m_tweenMove == null)
        {
            return;
        }
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
        {
            return Vector3.zero;
        }
        //
        return m_pathList[Index];
    }

    public void SetPath(Vector3 Pos, int Index)
    {
        if (Index > PathCount - 1)
        {
            return;
        }
        //
        m_pathList[Index] = Pos;
    }

    public void SetPath(Vector2 Pos, int Index)
    {
        if (Index > PathCount - 1)
        {
            return;
        }
        //
        m_pathList[Index] = Pos;
    }

    public void SetPathAdd()
    {
        if (PathCount == 0)
        {
            m_pathList.Add(Vector2.right * 10f);
        }
        else
        if (PathCount > 0)
        {
            m_pathList.Add(m_pathList[PathCount - 1] + Vector2.right * 10f);
        }
    }

    public void SetPathRemove()
    {
        if (PathCount > 0)
        {
            m_pathList.RemoveAt(PathCount - 1);
        }
    }

    #endregion

    protected virtual void OnDrawGizmosSelected()
    {
        if (m_colliderBase == null)
        {
            return;
        }

        if (m_pathList != null)
        {
            if (!Application.isPlaying)
            {
                for (int i = 0; i < PathCount; i++)
                {
                    switch (m_pathType)
                    {
                        case ElevatorPath.Local:
                            SetCollider2D(transform.TransformPoint(m_pathList[i]), m_colliderBase, Color.white);
                            break;
                        case ElevatorPath.World:
                            SetCollider2D(m_pathList[i], m_colliderBase, Color.white);
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < PathCount; i++)
                {
                    SetCollider2D(m_pathList[i], m_colliderBase, Color.white);
                }
            }
        }
    }

    public void SetCollider2D(Vector2 Pos, Collider2D From, Color Color)
    {
        SetWireCube(Pos, (Vector2)From.bounds.size, Color);
    }

    public void SetWireCube(Vector2 Pos, Vector3 Size, Color Color)
    {
        Gizmos.color = Color;
        Gizmos.DrawWireCube(Pos, Size);
    }
}

#if UNITY_EDITOR

[CanEditMultipleObjects]
[CustomEditor(typeof(ObjectPath))]
public class TweenMovePathEditor : Editor
{
    private ObjectPath m_target;

    private SerializedProperty m_activeType;

    private SerializedProperty m_tweenType;
    private SerializedProperty m_easeType;
    private SerializedProperty m_pathType;

    private SerializedProperty m_loop;
    private SerializedProperty m_loopInfinite;
    private SerializedProperty m_loopCount;
    private SerializedProperty m_loopType;

    private SerializedProperty m_duration;
    private SerializedProperty m_durationRevertScale;

    private SerializedProperty m_pathList;

    private SerializedProperty m_colliderBase;
    private SerializedProperty m_rigidbodyBase;

    private void OnEnable()
    {
        m_target = (target as ObjectPath);

        m_activeType = serializedObject.FindProperty("m_activeType");

        m_tweenType = serializedObject.FindProperty("m_tweenType");
        m_easeType = serializedObject.FindProperty("m_easeType");
        m_pathType = serializedObject.FindProperty("m_pathType");

        m_loop = serializedObject.FindProperty("m_loop");
        m_loopInfinite = serializedObject.FindProperty("m_loopInfinite");
        m_loopCount = serializedObject.FindProperty("m_loopCount");
        m_loopType = serializedObject.FindProperty("m_loopType");

        m_duration = serializedObject.FindProperty("m_duration");
        m_durationRevertScale = serializedObject.FindProperty("m_durationRevertScale");

        m_pathList = serializedObject.FindProperty("m_pathList");

        m_colliderBase = serializedObject.FindProperty("m_colliderBase");
        m_rigidbodyBase = serializedObject.FindProperty("m_rigidbodyBase");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(Application.isPlaying);

        EditorGUILayout.PropertyField(m_activeType);

        GUILayout.Space(10);

        EditorGUILayout.PropertyField(m_tweenType);
        EditorGUILayout.PropertyField(m_easeType);
        EditorGUILayout.PropertyField(m_pathType);

        GUILayout.Space(10);

        EditorGUILayout.PropertyField(m_loop);
        if (m_target.TweenLoop.Loop)
        {
            EditorGUILayout.PropertyField(m_loopInfinite);
            if (!m_target.TweenLoop.Infinite)
                EditorGUILayout.PropertyField(m_loopCount);
            EditorGUILayout.PropertyField(m_loopType);
        }

        GUILayout.Space(10);

        EditorGUILayout.PropertyField(m_duration);
        EditorGUILayout.PropertyField(m_durationRevertScale);

        GUILayout.Space(10);

        if (SetButton("Add Path"))
        {
            m_target.SetPathAdd();
        }

        if (SetButton("Remove Path"))
        {
            m_target.SetPathRemove();
        }

        EditorGUILayout.PropertyField(m_pathList);

        GUILayout.Space(10);

        EditorGUILayout.PropertyField(m_colliderBase);
        EditorGUILayout.PropertyField(m_rigidbodyBase);

        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        if (Application.isPlaying)
        {
            return;
        }
        //
        Handles.color = Color.white;
        //
        for (int i = 0; i < m_target.PathCount; i++)
        {
            if (m_target.PathType == ObjectPath.ElevatorPath.World)
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
                {
                    Handles.DrawDottedLine(m_target.GetPath()[i], m_target.GetPath()[i - 1], 10);
                }
                else
                {
                    Handles.DrawDottedLine(m_target.GetPath()[i], m_target.transform.position, 10);
                }
            }
            else
            if (m_target.PathType == ObjectPath.ElevatorPath.Local)
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

    public static bool SetButton(string Label, GUIStyle GUIStyle = null, params GUILayoutOption[] GUILayoutOption)
    {
        if (GUIStyle == null)
            return GUILayout.Button(Label, GUILayoutOption);
        else
            return GUILayout.Button(Label, GUIStyle, GUILayoutOption);
    }
}

#endif