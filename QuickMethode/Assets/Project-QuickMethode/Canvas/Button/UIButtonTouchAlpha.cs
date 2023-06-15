using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonTouchAlpha : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float m_Normal = 1f;

    [SerializeField] private float m_Hold = 0.5f;

    private CanvasGroup m_CanvasGroup;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Application.isEditor)
        {
            return;
        }

        if (m_CanvasGroup == null)
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        m_CanvasGroup.alpha = m_Hold;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Application.isEditor)
        {
            return;
        }

        if (m_CanvasGroup == null)
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        m_CanvasGroup.alpha = m_Normal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Application.isEditor)
        {
            return;
        }

        if (m_CanvasGroup == null)
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        m_CanvasGroup.alpha = m_Hold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Application.isEditor)
        {
            return;
        }

        if (m_CanvasGroup == null)
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        m_CanvasGroup.alpha = m_Normal;
    }

}
