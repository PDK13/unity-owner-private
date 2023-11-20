using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSortBottleMover : MonoBehaviour
{
    private Vector2 m_bottleAPosStart = Vector2.zero;
    private Vector2 m_bottleAPosTo = Vector2.zero;

    private Transform m_managerGroupBottle;
    private WaterSortBottle m_bottleA;
    private WaterSortBottle m_bottleB;

    private float BottleChoiceOffsetY => WaterSortManager.Instance.ManagerConfig.BottleChoiceOffsetY;

    private float BottleFillOffsetY => WaterSortManager.Instance.ManagerConfig.BottleFillOffsetY;

    private float BottleMoveDuration => WaterSortManager.Instance.ManagerConfig.BottleMoveDuration;

    private Ease BottleMoveEase => WaterSortManager.Instance.ManagerConfig.BottleMoveEase;

    private bool m_destroy = false;

    private void OnDestroy()
    {
        this.transform.DOKill();
    }

    public void SetInit(Transform ManagerGroupBottle, WaterSortBottle BottleA, WaterSortBottle BottleB)
    {
        m_managerGroupBottle = ManagerGroupBottle;
        m_bottleA = BottleA;
        m_bottleB = BottleB;
        //
        m_bottleA.onBottleFill += OnBottleAFill;
        m_bottleA.onBottleActive += OnBottleAActive;
        //
        this.transform.position = m_bottleA.RotatePointActive.position;
        //
        m_bottleAPosStart = m_bottleA.RotatePointActive.position + Vector3.down * BottleChoiceOffsetY;
        m_bottleAPosTo.x = m_bottleB.transform.position.x;
        m_bottleAPosTo.y = m_bottleB.RotatePointActive.position.y + BottleFillOffsetY;
        //
        m_bottleA.transform.parent = this.transform;
        //
        this.transform.DOMove(m_bottleAPosTo, BottleMoveDuration).SetEase(BottleMoveEase)
            .OnUpdate(() =>
            {
                m_bottleA.SetBottleValuePosYUpdate();
            })
            .OnComplete(() =>
            {
                m_bottleA.SetBottleValuePosYUpdate();
                m_bottleA.Wait = false;
            });
    }

    private void OnBottleAFill(WaterSortBottle Bottle, bool Stage)
    {
        if (Stage)
            return;
        //
        m_bottleA.onBottleFill -= OnBottleAFill;
        //
        this.transform.DOMove(m_bottleAPosStart, BottleMoveDuration).SetEase(WaterSortManager.Instance.ManagerConfig.BottleMoveEase)
            .OnUpdate(() =>
            {
                m_bottleA.SetBottleValuePosYUpdate();
            })
            .OnComplete(() =>
            {
                m_bottleA.transform.parent = m_managerGroupBottle;
                m_bottleA.SetBottleValuePosYUpdate();
                //
                if (m_destroy)
                    Destroy(this.gameObject);
                else
                    m_destroy = true;
            });
    }

    private void OnBottleAActive(WaterSortBottle Bottle, bool Stage)
    {
        if (Stage)
            return;
        //
        m_bottleA.onBottleActive -= OnBottleAActive;
        //
        if (m_destroy)
            Destroy(this.gameObject);
        else
            m_destroy = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_bottleAPosStart, m_bottleAPosTo);
    }
}