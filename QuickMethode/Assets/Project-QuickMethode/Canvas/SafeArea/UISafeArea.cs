using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
#endif
public class UISafeArea : MonoBehaviour
{   
    void Start()
    {
        SetUpdateSafeArea();
    }

#if UNITY_EDITOR
    void Update()
    {
        SetUpdateSafeArea();
    }
#endif

    void SetUpdateSafeArea()
    {
        RectTransform m_RectTransform = GetComponent<RectTransform>();
        //m_RectTransform.sizeDelta = QResolution.GetCameraSizeUnit() - Vector2.one * 20;

        Rect SafeRect = Screen.safeArea;

        m_RectTransform.anchorMin = Vector2.zero;
        m_RectTransform.anchorMax = Vector2.one;

        Vector2 AnchorPos = m_RectTransform.anchoredPosition;
        AnchorPos.x = SafeRect.x / 2;
        m_RectTransform.anchoredPosition = AnchorPos;

        Vector2 Size = m_RectTransform.sizeDelta;
        Size.x = -SafeRect.x;
        m_RectTransform.sizeDelta = Size;
    }
}
