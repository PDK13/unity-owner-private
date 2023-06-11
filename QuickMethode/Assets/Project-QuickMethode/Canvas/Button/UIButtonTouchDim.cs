using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonTouchDim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Color m_ColorNormal = Color.white;

    [SerializeField] private Color m_ColorHold = Color.gray;

    [SerializeField] private Image m_Button;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Application.isEditor)
        {
            return;
        }

        if (m_Button == null)
        {
            m_Button = GetComponent<Image>();
        }

        m_Button.color = m_ColorHold;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Application.isEditor)
        {
            return;
        }

        if (m_Button == null)
        {
            m_Button = GetComponent<Image>();
        }

        m_Button.color = m_ColorNormal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Application.isEditor)
        {
            return;
        }

        if (m_Button == null)
        {
            m_Button = GetComponent<Image>();
        }

        m_Button.color = m_ColorHold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Application.isEditor)
        {
            return;
        }

        if (m_Button == null)
        {
            m_Button = GetComponent<Image>();
        }

        m_Button.color = m_ColorNormal;
    }
}
