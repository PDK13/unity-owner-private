using System.Collections.Generic;
using UnityEngine;

public class BackgroundScale : MonoBehaviour
{
    [SerializeField] private UnitScaleType m_scale = UnitScaleType.Span;
    [SerializeField] private Camera m_camera;

    [SerializeField] private List<SpriteRenderer> m_background = new List<SpriteRenderer>();

    private Vector2 m_cameraResolution = new Vector2();

    private void Start()
    {
        if (m_camera == null)
            m_camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (m_camera == null)
            return;
        //
        m_cameraResolution = QCamera.GetCameraSizeUnit(m_camera.GetComponent<Camera>());
        //
        foreach (SpriteRenderer Background in m_background)
            Background.size = QSpriteScale.GetSizeUnitScaled(QSprite.GetSizeUnit(Background.sprite), m_cameraResolution, m_scale);
    }
}