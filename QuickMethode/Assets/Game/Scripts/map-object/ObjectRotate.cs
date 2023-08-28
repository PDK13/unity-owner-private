using DG.Tweening;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    [SerializeField] private string m_key = "";
    [SerializeField] private float m_timeRotate = 1f;
    [SerializeField] private bool m_useLimits = true;
    [SerializeField] private Vector2 m_minAngle;
    [SerializeField] private Vector2 m_maxAngle;
    [SerializeField] private bool m_clockwise;
    [SerializeField] private bool m_loop;
    [SerializeField] private LoopType m_loopType = LoopType.Yoyo;
    [SerializeField] private bool m_canInvert = false;

    private Rigidbody2D m_rig;

    private bool m_state;
    private Tweener m_tweener;
    private float m_minAngleValue;
    private float m_maxAngleValue;
    private bool m_tweenerKilled;

    private void Awake()
    {
        //GameController.triggerEvent += OnTrigger;
        m_state = false;
        m_rig = GetComponent<Rigidbody2D>();
        //
        Vector2 minDirection = m_minAngle - (Vector2)transform.position;
        Vector2 maxDirection = m_maxAngle - (Vector2)transform.position;
        m_minAngleValue = Mathf.Atan2(minDirection.y, minDirection.x) * Mathf.Rad2Deg;
        m_maxAngleValue = Mathf.Atan2(maxDirection.y, maxDirection.x) * Mathf.Rad2Deg;
        if (m_useLimits)
        {
            transform.eulerAngles = new Vector3(0, 0, m_minAngleValue);
        }
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(m_key))
        {
            m_state = true;
            Play();
        }
    }

    private void OnDestroy()
    {
        //GameController.triggerEvent -= OnTrigger;
        transform.DOKill();
        m_tweener?.Kill();
    }

    private void OnTrigger(string key, bool state, GameObject triggerSource)
    {
        if (m_key != key)
        {
            return;
        }

        m_state = !m_state;
        if (m_canInvert && m_tweener != null)
        {
            if (m_useLimits && m_tweenerKilled)
            {
                m_clockwise = !m_clockwise;
                float tempAngle = m_minAngleValue;
                m_minAngleValue = m_maxAngleValue;
                m_maxAngleValue = tempAngle;
                m_tweener = null;
                Play();
                return;
            }
            if (m_tweener.isBackwards)
            {
                m_tweener.PlayForward();
            }
            else
            {
                m_tweener.PlayBackwards();
            }
        }
        else
        {
            if (m_state)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    private void Play()
    {
        if (m_tweener == null)
        {
            if (m_useLimits)
            {
                float offset = 0;
                //min 0 -> 180, -180 -> 0
                //max 0 -> 180, -180 -> 0
                //setp 1: calculate min, max to positive
                if (m_minAngleValue < 0)
                {
                    m_minAngleValue += 360;
                }

                if (m_maxAngleValue < 0)
                {
                    m_maxAngleValue += 360;
                }
                //step 2: convert min, max by clockwise
                if (m_clockwise)
                {
                    if (m_minAngleValue < m_maxAngleValue)// move min to 360
                    {
                        offset = 360 - m_minAngleValue;
                        m_minAngleValue = 360;
                        m_maxAngleValue += offset - 360;
                    }
                }
                else
                {
                    if (m_minAngleValue > m_maxAngleValue)//move min to 0
                    {
                        offset = 360 - m_minAngleValue;
                        m_minAngleValue = 0;
                        m_maxAngleValue += offset;
                    }
                }
                if (m_rig)
                {
                    m_tweener = DOTween.To(() => m_minAngleValue, x => m_rig.MoveRotation(x), m_maxAngleValue, m_timeRotate).SetEase(Ease.Linear).OnStart(() => m_tweenerKilled = false).OnKill(() => m_tweenerKilled = true);
                }
                else
                {
                    m_tweener = DOTween.To(() => m_minAngleValue, x => transform.eulerAngles = new Vector3(0, 0, x - offset), m_maxAngleValue, m_timeRotate).SetEase(Ease.Linear).OnStart(() => m_tweenerKilled = false).OnKill(() => m_tweenerKilled = true);
                }
            }
            else
            {
                if (m_rig)
                {
                    float startAngle = 0;
                    m_tweener = DOTween.To(() => startAngle, x => m_rig.MoveRotation(x), m_clockwise ? -360 : 360, m_timeRotate).SetEase(Ease.Linear);
                }
                else
                {
                    m_tweener = transform.DORotate(new Vector3(0, 0, (m_clockwise ? -360 : 360)), m_timeRotate, RotateMode.FastBeyond360).SetEase(Ease.Linear);
                }

            }
            if (m_loop)
            {
                m_tweener.SetLoops(-1, m_loopType);
            }
        }
        else
        {
            m_tweener.timeScale = 1;
        }
    }

    private void Stop()
    {
        m_tweener.timeScale = 0;
    }

    ////////////////////////////EDITOR ONLY////////////////////////////
    public Vector2 localMinAngle => transform.InverseTransformPoint(m_minAngle);

    public Vector2 localMaxAngle => transform.InverseTransformPoint(m_maxAngle);

    public void UpdateLocalPosition(Vector2 localMin, Vector2 localMax)
    {
        m_minAngle = transform.TransformPoint(localMin);
        m_maxAngle = transform.TransformPoint(localMax);
    }

    private void OnDrawGizmos()
    {
        // if (Application.isPlaying)
        //     return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 10);
        if (m_useLimits)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, m_minAngle);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, m_maxAngle);
        }
    }

}
