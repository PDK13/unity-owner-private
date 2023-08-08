using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_virtualCamera;

    [Space]
    [SerializeField] private List<string> m_tag;

    private Tween m_tweenZoom;

    private void Awake()
    {
        CameraController.onZoom += SetZoom;
    }

    private void OnDestroy()
    {
        CameraController.onZoom -= SetZoom;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_tag.Contains(collision.tag))
            return;
        //
        m_virtualCamera.gameObject.SetActive(true);
        m_virtualCamera.Follow = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!m_tag.Contains(collision.tag))
            return;
        //
        m_virtualCamera.gameObject.SetActive(false);
        m_virtualCamera.Follow = null;
    }

    private void SetZoom(float ZoomTo, float Duration)
    {
        if (m_tweenZoom != null)
            m_tweenZoom.Kill();
        m_tweenZoom = DOTween.To(() => m_virtualCamera.m_Lens.OrthographicSize, x => m_virtualCamera.m_Lens.OrthographicSize = x, ZoomTo, 2f).SetEase(Ease.Linear);
    }
}