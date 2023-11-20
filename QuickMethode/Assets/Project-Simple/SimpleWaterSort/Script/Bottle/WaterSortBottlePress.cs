using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSortBottlePress : MonoBehaviour
{
    private WaterSortBottle m_bottle;

    private void Awake()
    {
        m_bottle = GetComponent<WaterSortBottle>();
    }

    private void OnPress()
    {
        WaterSortManager.Instance.SetPress(m_bottle);
    } //Send message by "UIPress" component!
}