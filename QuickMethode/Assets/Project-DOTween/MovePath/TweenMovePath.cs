using DG.Tweening;
using QuickMethode;
using UnityEngine;
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

    public enum ElevatorMove 
    { 
        Loop, 
        Key, 
    }

    public enum ElevatorPath 
    { 
        Local, 
        World, 
    }

    #endregion

    #region Varible: Elevator

    [SerializeField] private ElevatorTween m_tweenType = ElevatorTween.Rigidbody;
    [SerializeField] private Ease m_easeType = Ease.Linear;
    [SerializeField] private ElevatorMove m_moveType = ElevatorMove.Loop;
    [SerializeField] private ElevatorPath m_pathType = ElevatorPath.Local;

    public ElevatorMove MoveType => m_moveType;
    public ElevatorPath PathType => m_pathType;

    //KEY
    [SerializeField] private string m_key = "Start";
    [SerializeField] private string m_keyStop = "Stop";
    [SerializeField] private bool m_once = false;
    private bool m_onceTrigged = false;
    //KEY

    [SerializeField] [Min(0)] private float m_duration = 4f;
    [SerializeField] [Min(0)] private float m_durationScaleRevert = 1f;
    
    [SerializeField] private Vector2[] m_pathList;
    private Vector2[] m_pathTween;
    private Vector2 m_posStart;

    public int PathCount => m_pathList.Length;

    #endregion

    private bool m_state;
    private Tweener m_tweenMove;

    [Space]
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
            for (int i = 0; i < m_pathList.Length; i++)
                m_pathList[i] += m_posStart;
        }
        //
        m_state = false;
    }

    protected virtual void Start()
    {
        if (m_moveType == ElevatorMove.Loop)
            SetLoop();
    }

    protected virtual void OnDestroy()
    {
        //Elevator

        if (m_tweenMove != null)
            m_tweenMove.Kill();
        transform.DOKill();
    }

    #region ======================================= Elevator

    #region Tween Move

    private void SetLoop()
    {
        if (m_pathList.Length == 0)
            return;

        switch (m_tweenType)
        {
            case ElevatorTween.Transform:
                Vector3[] Path = new Vector3[this.m_pathList.Length];
                for (int i = 0; i < this.m_pathList.Length; i++) Path[i] = (Vector3)this.m_pathList[i];
                m_tweenMove = transform.DOPath(Path, m_duration).SetEase(m_easeType).SetLoops(-1, LoopType.Yoyo)
                    .OnStart(() => SetTweenPathStart())
                    .OnUpdate(() => SetTweenPathUpdate())
                    .OnKill(() => SetTweenPathKill());
                break;
            case ElevatorTween.Rigidbody:
                m_tweenMove = m_rigidbodyBase.DOPath(this.m_pathList, m_duration).SetEase(m_easeType).SetLoops(-1, LoopType.Yoyo)
                    .OnStart(() => SetTweenPathStart())
                    .OnUpdate(() => SetTweenPathUpdate())
                    .OnKill(() => SetTweenPathKill());
                break;
        }
    }

    private void SetMove()
    {
        if (m_tweenMove == null)
        {
            switch (m_tweenType)
            {
                case ElevatorTween.Transform:
                    Vector3[] m_path = new Vector3[this.m_pathList.Length];
                    for (int i = 0; i < this.m_pathList.Length; i++) m_path[i] = (Vector3)this.m_pathList[i];
                    m_tweenMove = transform.DOPath(m_path, m_duration).SetEase(m_easeType)
                        .OnStart(() => SetTweenPathStart())
                        .OnUpdate(() => SetTweenPathUpdate())
                        .OnKill(() => SetTweenPathKill());
                    break;
                case ElevatorTween.Rigidbody:
                    m_tweenMove = m_rigidbodyBase.DOPath(this.m_pathList, m_duration).SetEase(m_easeType)
                        .OnStart(() => SetTweenPathStart())
                        .OnUpdate(() => SetTweenPathUpdate())
                        .OnKill(() => SetTweenPathKill());
                    break;
            }
            m_tweenMove.timeScale = 1;
        }
        else
        {
            m_tweenMove.timeScale = 1;
            m_tweenMove.PlayForward();
        }
    }

    private void SetMoveInvert()
    {
        if (m_tweenMove != null)
        {
            m_tweenMove.timeScale = m_durationScaleRevert;
            m_tweenMove.PlayBackwards();
        }
    }

    private void SetMoveStop()
    {
        if (m_tweenMove != null)
            m_tweenMove.timeScale = 0;
    }

    #endregion

    #region Tween Event

    protected virtual void SetTweenPathStart()
    {

    }

    protected virtual void SetTweenPathUpdate()
    {

    }

    protected virtual void SetTweenPathKill()
    {

    }

    #endregion

    #region Key Move

    protected void SetOnTrigger(string Key, bool State)
    {
        if (m_pathList.Length == 0) 
            return;

        if (m_moveType != ElevatorMove.Key) 
            return;

        if (m_key != Key || m_state == State) 
            return;

        if (m_once)
        {
            if (m_onceTrigged) 
                return;
            m_onceTrigged = true;
        }

        m_state = State;

        if (m_state)
            SetMove();
        else
            SetMoveInvert();
    }

    protected void SetOnTriggerStop(string Key, bool State)
    {
        if (m_pathList.Length == 0) 
            return;

        if (m_keyStop != Key) 
            return;

        SetMoveStop();
    }

    #endregion

    #endregion

    #region Editor

    public Vector2[] GetPath()
    {
        return m_pathList;
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

    #endregion

    protected virtual void OnDrawGizmosSelected()
    {
        if (m_colliderBase == null)
            return;

        if (m_pathList != null)
        {
            if (!Application.isPlaying)
            {
                for (int i = 0; i < m_pathList.Length; i++)
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
                for (int i = 0; i < m_pathList.Length; i++)
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

    private SerializedProperty m_tweenType;
    private SerializedProperty m_easeType;
    private SerializedProperty m_moveType;
    private SerializedProperty m_pathType;

    private SerializedProperty m_key;
    private SerializedProperty m_keyStop;
    private SerializedProperty m_once;

    private SerializedProperty m_duration;
    private SerializedProperty m_durationScaleRevert;

    private SerializedProperty m_pathList;

    private SerializedProperty m_colliderBase;
    private SerializedProperty m_rigidbodyBase;

    void OnEnable()
    {
        m_target = (target as TweenMovePath);
        //
        m_tweenType = serializedObject.FindProperty("m_tweenType");
        m_easeType = serializedObject.FindProperty("m_easeType");
        m_moveType = serializedObject.FindProperty("m_moveType");
        m_pathType = serializedObject.FindProperty("m_pathType");
        //
        m_key = serializedObject.FindProperty("m_key");
        m_keyStop = serializedObject.FindProperty("m_keyStop");
        m_once = serializedObject.FindProperty("m_once");
        //
        m_duration = serializedObject.FindProperty("m_duration");
        m_durationScaleRevert = serializedObject.FindProperty("m_durationScaleRevert");

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
        QEditorCustom.SetField(m_tweenType);
        QEditorCustom.SetField(m_easeType);
        QEditorCustom.SetField(m_moveType);
        QEditorCustom.SetField(m_pathType);
        //
        QEditor.SetSpace(10);
        //
        if (m_target.MoveType == TweenMovePath.ElevatorMove.Key)
        {
            QEditorCustom.SetField(m_key);
            QEditorCustom.SetField(m_once);
        }
        QEditorCustom.SetField(m_keyStop);
        //
        QEditor.SetSpace(10);
        //
        QEditorCustom.SetField(m_duration);
        QEditorCustom.SetField(m_durationScaleRevert);
        //
        QEditorCustom.SetField(m_pathList);
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