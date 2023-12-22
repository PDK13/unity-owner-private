using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleWaterSortBottle : MonoBehaviour
{
    private const string REF_FILL_AMOUNT = "_FillAmount";
    private const string REF_FILL_SCALE = "_FillScale";
    private const string REF_COLOR = "_Color";

    [SerializeField] private float m_angleMin = 30f; //Deg

    [SerializeField] private SpriteRenderer m_mask;

    [SerializeField] private AnimationCurve m_fillAmountCurve;
    

    private float FillAmount
    {
        get => m_mask.material.GetFloat(REF_FILL_AMOUNT);
        set => m_mask.material.SetFloat(REF_FILL_AMOUNT, value);
    }

    private float FillScale
    {
        get => m_mask.material.GetFloat(REF_FILL_SCALE);
        set => m_mask.material.SetFloat(REF_FILL_SCALE, value);
    }

    private (int Index, Color Color) Color
    {
        set => m_mask.material.SetColor(string.Format("{0}{1}", REF_COLOR, value.Index), value.Color);
    }

    private void Awake()
    {

    }

    public void SetReset()
    {
        FillAmount = 1.5f;
        FillScale = 1f;
    }

    public void SetWaterLevel(float Angle)
    {
        //Angle = Mathf.Abs(Angle);
        SetWaterFillAmount(Angle);
        //SetWaterFillScale(Angle);
    }

    private void SetWaterFillAmount(float Angle)
    {
        FillAmount = m_fillAmountCurve.Evaluate(Angle);
    }

    private void SetWaterFillScale(float Angle)
    {
        float Step = Angle / 90;
        float Fill = 1f - 1f * Step;
        //
        FillScale = Fill;
    }
}