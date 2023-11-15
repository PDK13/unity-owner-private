using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "bottle-curve-data", menuName = "Water Sort/Bottle Curve", order = 0)]
public class WaterSortBottleCurveData : ScriptableObject
{
    public AnimationCurve CurveFillAmount;
    public AnimationCurve CurveScaleAndRotation;
    public AnimationCurve CurveRotationSpeed;
    public List<float> LimitRotation = new List<float>();
    public List<float> LimitFillAmount = new List<float>();
}