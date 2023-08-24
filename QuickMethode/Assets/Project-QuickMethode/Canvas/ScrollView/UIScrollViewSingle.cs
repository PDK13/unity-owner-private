using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UIScrollViewSingle : MonoBehaviour
{
    private enum ScrollViewType
    {
        Vertical,
        Horizontal,
    }

    [SerializeField] private ScrollViewType m_scrollViewType;
    [SerializeField] private ScrollRect.MovementType m_movementType = ScrollRect.MovementType.Clamped;

    [Space]
    [SerializeField] [Min(1)] private int m_itemConstraint = 1;
    [SerializeField] private Vector2 m_itemSize = new Vector2(100f, 100f);
    [SerializeField] private Vector2 m_itemSpacing = new Vector2(5f, 5f);

    [Space]
    [SerializeField] private ScrollRect m_scrollRect;
    [SerializeField] private RectTransform m_content;
    [SerializeField] private ContentSizeFitter m_contentSizeFitter;
    [SerializeField] private GridLayoutGroup m_gridLayoutGroup;

    public RectTransform Content => m_content;

    private void Awake()
    {
        if (Application.isEditor && !Application.isPlaying)
            SetInit();
    }

    private void Reset()
    {
        SetInit();
    }

#if UNITY_EDITOR

    private void Update()
    {
        SetScrollViewFixed();
    }

#endif

    private void SetInit()
    {
        if (GetComponent<ScrollRect>() == null)
            Debug.LogWarningFormat("{0}: Script not attach to Scroll View GameObject?!", name);
        else
            m_scrollRect = this.GetComponent<ScrollRect>();
        //
        if (this.transform.Find("Viewport/Content") == null)
        {
            Debug.LogWarningFormat("{0}: Something went wrong to get child Viewport/Content of Scroll View!", name);
            return;
        }
        else
        {
            m_content = this.transform.Find("Viewport/Content").GetComponent<RectTransform>();
            //
            //Grid Layout Group
            if (m_content.gameObject.GetComponent<ContentSizeFitter>() == null)
                m_contentSizeFitter = m_content.gameObject.AddComponent<ContentSizeFitter>();
            if (m_contentSizeFitter == null)
                m_contentSizeFitter = m_content.gameObject.GetComponent<ContentSizeFitter>();
            //
            //Content Size Fitter
            if (m_content.gameObject.GetComponent<GridLayoutGroup>() == null)
                m_gridLayoutGroup = m_content.gameObject.AddComponent<GridLayoutGroup>();
            if (m_gridLayoutGroup == null)
                m_gridLayoutGroup = m_content.gameObject.GetComponent<GridLayoutGroup>();
        }
        //
        SetScrollViewFixed();
    }

    #region Content Primary

    public void SetScrollViewTouch(bool ScrollViewTouch)
    {
        m_scrollRect.enabled = ScrollViewTouch;
    }

    private void SetScrollViewFixed()
    {
        switch (m_scrollViewType)
        {
            case ScrollViewType.Vertical:
                //Content
                if (m_scrollRect != null)
                {
                    m_content.anchoredPosition = new Vector2(0, m_content.anchoredPosition.y);
                }
                //Scroll Rect
                if (m_contentSizeFitter != null)
                {
                    m_scrollRect.vertical = true;
                    m_scrollRect.horizontal = false;
                    m_scrollRect.movementType = m_movementType;
                }
                //Grid Layout Group
                if (m_gridLayoutGroup != null)
                {
                    m_gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
                    m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                    m_gridLayoutGroup.constraintCount = m_itemConstraint;
                }
                //Content Size Fitter
                if (m_gridLayoutGroup == null)
                {
                    m_contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    m_contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                }
                break;
            case ScrollViewType.Horizontal:
                if (m_scrollRect != null)
                {
                    m_content.anchoredPosition = new Vector2(m_content.anchoredPosition.x, 0);
                }
                //Scroll Rect
                if (m_contentSizeFitter != null)
                {
                    m_scrollRect.vertical = false;
                    m_scrollRect.horizontal = true;
                    m_scrollRect.movementType = m_movementType;
                }
                //Grid Layout Group
                if (m_gridLayoutGroup != null)
                {
                    m_gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
                    m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                    m_gridLayoutGroup.constraintCount = m_itemConstraint;
                }
                //Content Size Fitter
                if (m_gridLayoutGroup != null)
                {
                    m_contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    m_contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                }
                break;
        }

        if (m_gridLayoutGroup != null)
        {
            m_gridLayoutGroup.cellSize = m_itemSize;
            m_gridLayoutGroup.spacing = m_itemSpacing;
        }
    }

    #endregion

    #region Content Pos

    public void SetContentPos(float ItemIndex)
    {
        if (ItemIndex < 0)
        {
            m_content.anchoredPosition = GetContentPos(0);
        }
        else
        if (ItemIndex > m_content.childCount - 1)
        {
            m_content.anchoredPosition = GetContentPos(m_content.childCount - 1);
        }
        else
        {
            m_content.anchoredPosition = GetContentPos(ItemIndex);
        }
    }

    public Vector2 GetContentPos(float ItemIndex)
    {
        switch (m_scrollViewType)
        {
            case ScrollViewType.Vertical:
                return (+1) * (ItemIndex / m_itemConstraint) * new Vector2(0, m_itemSize.y + m_itemSpacing.y);
            case ScrollViewType.Horizontal:
                return (-1) * (ItemIndex / m_itemConstraint) * new Vector2(m_itemSize.x + m_itemSpacing.x, 0);
        }
        return new Vector2();
    }

    public Vector2 GetContentPos(int ItemIndex)
    {
        switch (m_scrollViewType)
        {
            case ScrollViewType.Vertical:
                return (+1) * (ItemIndex / m_itemConstraint) * new Vector2(0, m_itemSize.y + m_itemSpacing.y);
            case ScrollViewType.Horizontal:
                return (-1) * (ItemIndex / m_itemConstraint) * new Vector2(m_itemSize.x + m_itemSpacing.x, 0);
        }
        return new Vector2();
    }

    #endregion

    #region Content RecTransform

    public RectTransform GetContentItem(int ItemIndex)
    {
        if (ItemIndex < 0)
        {
            return m_content.GetChild(0).GetComponent<RectTransform>();
        }
        else
        if (ItemIndex > m_content.childCount - 1)
        {
            return m_content.GetChild(m_content.childCount - 1).GetComponent<RectTransform>();
        }
        else
        {
            return m_content.GetChild(ItemIndex).GetComponent<RectTransform>();
        }
    }

    #endregion
}