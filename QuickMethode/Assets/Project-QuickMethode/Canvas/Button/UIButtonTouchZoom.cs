using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButtonTouchZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Vector3 m_Scale_Normal = new Vector3(1f, 1f, 1f);

    [SerializeField] private Vector3 m_Scale_Hold = new Vector3(1.2f, 1.2f, 1);

    private Vector3 m_Scale_Primary;

    [Space]

    [SerializeField]
    private UnityEvent m_EventPointerDown;

    [SerializeField]
    private UnityEvent m_EventPointerUp;

    private void Start()
    {
        m_Scale_Primary = this.transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Application.isEditor)
        {
            return;
        }

        transform.localScale = new Vector3(m_Scale_Hold.x * m_Scale_Primary.x, m_Scale_Hold.y * m_Scale_Primary.y, m_Scale_Hold.z * m_Scale_Primary.z);

        m_EventPointerDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Application.isEditor)
        {
            return;
        }

        transform.localScale = new Vector3(m_Scale_Normal.x * m_Scale_Primary.x, m_Scale_Normal.y * m_Scale_Primary.y, m_Scale_Normal.z * m_Scale_Primary.z);

        m_EventPointerUp?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Application.isEditor)
        {
            return;
        }

        transform.localScale = new Vector3(m_Scale_Hold.x * m_Scale_Primary.x, m_Scale_Hold.y * m_Scale_Primary.y, m_Scale_Hold.z * m_Scale_Primary.z);
        
        m_EventPointerDown?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Application.isEditor)
        {
            return;
        }

        transform.localScale = new Vector3(m_Scale_Normal.x * m_Scale_Primary.x, m_Scale_Normal.y * m_Scale_Primary.y, m_Scale_Normal.z * m_Scale_Primary.z);

        m_EventPointerUp?.Invoke();
    }
}