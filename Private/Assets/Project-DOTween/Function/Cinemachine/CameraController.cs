using Cinemachine;
using DG.Tweening;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { private set; get; }

    [SerializeField] private CinemachineVirtualCamera m_virtualCamera;
    [SerializeField] private Transform m_follow;

    private float m_baseOrthographicSize;
    private Tween m_tweenZoom;

    public static Action<float, float> onZoom; //<ZoomTo, Duration>

    private void Awake()
    {
        Instance = this;
        //
        m_baseOrthographicSize = m_virtualCamera.m_Lens.OrthographicSize;
        m_virtualCamera.Follow = m_follow;
    }

    public static void SetScale(float Scale, float Duration)
    {
        if (Instance.m_tweenZoom != null)
        {
            Instance.m_tweenZoom.Kill();
        }

        Instance.m_tweenZoom = DOTween.To(() => Instance.m_virtualCamera.m_Lens.OrthographicSize, x => Instance.m_virtualCamera.m_Lens.OrthographicSize = x, Instance.m_baseOrthographicSize * Scale, Duration).SetEase(Ease.Linear);
        //
        onZoom?.Invoke(Instance.m_baseOrthographicSize * Scale, Duration);
    }

    public static void SetFollow(Transform Follow)
    {
        Instance.m_follow = Follow;
        Instance.m_virtualCamera.Follow = Instance.m_follow;
    }
}