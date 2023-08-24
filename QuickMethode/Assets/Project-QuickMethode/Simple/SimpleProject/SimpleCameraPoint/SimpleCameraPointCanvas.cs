using UnityEngine;

public class SimpleCameraPointCanvas : MonoBehaviour
{
    [SerializeField] private Camera m_cameraMain;

    [Space]
    [SerializeField] private Vector2 m_anchorPos;
    [SerializeField] private Vector2 m_anchorPosMouse;
    [SerializeField] private Vector2 m_anchorPosCentre; //Start!
    private RectTransform m_recTransform;

    private void Start()
    {
        if (m_cameraMain == null)
            m_cameraMain = Camera.main;

        m_recTransform = GetComponent<RectTransform>();

        m_anchorPosCentre = QRecTransform.GetAnchorPos(m_recTransform, Vector2.one * 0.5f, Vector2.one * 0.5f, Vector2.one * 0.5f);
    }

    private void Update()
    {
        m_anchorPos = m_recTransform.anchoredPosition;

        m_anchorPosMouse = QCamera.GetPosMouseToCanvas(); //Target

        m_recTransform.anchoredPosition = m_anchorPosMouse;
    }
}