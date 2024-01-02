using DG.Tweening;
using UnityEngine;

public class SampleWaterSortManager : MonoBehaviour
{
    [SerializeField] private SampleWaterSortBottle m_bottle;
    [SerializeField] private Transform m_rotate;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SetBottleRotate();
        //
        if (Input.GetKeyDown(KeyCode.R))
            SetBottleReset();
    }

    private void SetBottleRotate()
    {
        m_rotate
            .DORotate(Vector3.forward * -90, 10, RotateMode.Fast)
            .OnUpdate(() => m_bottle.SetWaterLevel(QCircle.GetDeg180(m_rotate.eulerAngles.z)))
            .SetEase(Ease.Linear);
    }

    private void SetBottleReset()
    {
        m_rotate.DOKill();
        m_rotate.eulerAngles = Vector3.zero;
        m_bottle.SetReset();
    }
}