using DG.Tweening;
using QuickMethode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMovePath : MonoBehaviour
{
    #region Enum

    public enum ElevatorTween 
    { 
        Transform, //Non-Physic event!!
        Rigidbody, //Got Physic Event!!
    }

    public enum ElevatorMove { Loop, Key, }

    #endregion

    #region Varible: Elevator

    [SerializeField] private ElevatorTween m_tween = ElevatorTween.Transform;
    [SerializeField] private Ease m_ease = Ease.Linear;
    [SerializeField] private ElevatorMove m_elevator = ElevatorMove.Loop;

    //KEY
    [SerializeField] private string m_key = "Start";
    [SerializeField] private string m_keyStop = "Stop";
    [SerializeField] private bool m_once = false;
    private bool m_onceTrigged = false;
    //KEY

    [SerializeField] private float m_time = 4f;
    [SerializeField] [Range(0, 3)] private float m_timeRatioRevert = 1f;
    
    [SerializeField] private Vector2[] m_path;

    #endregion

    #region Varible: Public

    public ElevatorTween TweenType => m_tween;
    public ElevatorMove Elevator => m_elevator;

    #endregion

    private bool m_state;
    private Tweener m_tweenMove;

    private Collider2D m_colliderBase;
    private Rigidbody2D m_rigidbodyBase;

    protected virtual void Awake()
    {
        if (m_colliderBase == null) 
            m_colliderBase = GetComponent<Collider2D>();

        if (m_tween == ElevatorTween.Rigidbody)
        {
            m_rigidbodyBase = GetComponent<Rigidbody2D>();

            if (m_rigidbodyBase == null)
                m_tween = ElevatorTween.Transform;
        }

        //Elevator
        m_state = false;
    }

    protected virtual void Start()
    {
        //Elevator

        if (m_elevator == ElevatorMove.Loop)
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
        if (m_path.Length == 0)
            return;

        switch (m_tween)
        {
            case ElevatorTween.Transform:
                Vector3[] Path = new Vector3[this.m_path.Length];
                for (int i = 0; i < this.m_path.Length; i++) Path[i] = (Vector3)this.m_path[i];
                m_tweenMove = transform.DOPath(Path, m_time).SetEase(m_ease).SetLoops(-1, LoopType.Yoyo)
                    .OnStart(() => SetTweenPathStart())
                    .OnUpdate(() => SetTweenPathUpdate())
                    .OnKill(() => SetTweenPathKill());
                break;
            case ElevatorTween.Rigidbody:
                m_tweenMove = m_rigidbodyBase.DOPath(this.m_path, m_time).SetEase(m_ease).SetLoops(-1, LoopType.Yoyo)
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
            switch (m_tween)
            {
                case ElevatorTween.Transform:
                    Vector3[] m_path = new Vector3[this.m_path.Length];
                    for (int i = 0; i < this.m_path.Length; i++) m_path[i] = (Vector3)this.m_path[i];
                    m_tweenMove = transform.DOPath(m_path, m_time).SetEase(m_ease)
                        .OnStart(() => SetTweenPathStart())
                        .OnUpdate(() => SetTweenPathUpdate())
                        .OnKill(() => SetTweenPathKill());
                    break;
                case ElevatorTween.Rigidbody:
                    m_tweenMove = m_rigidbodyBase.DOPath(this.m_path, m_time).SetEase(m_ease)
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
            m_tweenMove.timeScale = m_timeRatioRevert;
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
        if (m_path.Length == 0) 
            return;

        if (m_elevator != ElevatorMove.Key) 
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
        if (m_path.Length == 0) 
            return;

        if (m_keyStop != Key) 
            return;

        SetMoveStop();
    }

    #endregion

    #endregion

    public Vector2[] GetPath()
    {
        return m_path;
    }

    public void SetUpdatePath(Vector3 value, int index)
    {
        m_path[index] = (Vector2)value;
    }

    protected virtual void OnDrawGizmos()
    {
        if (m_colliderBase == null)
            m_colliderBase = GetComponent<Collider2D>();

        if (m_path != null)
        {
            for (int i = 0; i < m_path.Length; i++)
            {
                QGizmos.SetCollider2D(m_path[i], m_colliderBase, Color.gray);
                if (i > 0)
                    QGizmos.SetLine(m_path[i], m_path[i - 1], Color.gray);
                else
                if (!Application.isPlaying)
                    QGizmos.SetLine(m_path[i], transform.position, Color.gray);
            }
        }
    }
}
