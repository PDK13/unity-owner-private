using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSortBottleLine : MonoBehaviour
{
    private WaterSortBottle m_bottle;
    private LineRenderer m_lineRenderer;

    private float BottleFillOffsetY => WaterSortManager.Instance.ManagerConfig.BottleFillOffsetY;

    private void Awake()
    {
        m_bottle = GetComponentInParent<WaterSortBottle>();
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        m_bottle.onBottleFill += OnBottleBFill;
        //
        m_lineRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        m_bottle.onBottleFill -= OnBottleBFill;
    }

    private void OnBottleBFill(WaterSortBottle Bottle, bool Stage)
    {
        if (Stage && Bottle.Targeted)
        {
            m_lineRenderer.enabled = true;
            //
            m_lineRenderer.startColor = Bottle.ColorList[Bottle.ColorList.Count -1];
            m_lineRenderer.endColor = Bottle.ColorList[Bottle.ColorList.Count - 1];
            //
            Vector3 PosStart = this.transform.position;
            Vector3 PosEnd = Vector3.forward * this.transform.position.z;
            PosEnd.x = this.transform.position.x;
            PosEnd.y = m_bottle.RotatePointActive.transform.position.y + BottleFillOffsetY;
            Vector3[] Line = new Vector3[2] { PosStart, PosEnd };
            m_lineRenderer.SetPositions(Line);
        }
        else
            m_lineRenderer.enabled = false;
    }
}