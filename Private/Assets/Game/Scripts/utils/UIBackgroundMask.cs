using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIBackgroundMask : MonoBehaviour
{
    public static UIBackgroundMask Instance;

    [SerializeField] private Image m_background;

    public void Awake()
    {
        Instance = this;
    }

    public static void SetMaskActive(bool On)
    {
        Instance.m_background.enabled = On;
    }
}