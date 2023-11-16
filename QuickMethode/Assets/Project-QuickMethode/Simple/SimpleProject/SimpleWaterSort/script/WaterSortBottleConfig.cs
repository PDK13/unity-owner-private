using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "water-sort-bottle-config", menuName = "Water Sort/Bottle Config", order = 0)]
public class WaterSortBottleConfig : ScriptableObject
{
    public float DurationRotate = 1.0f;
    public AnimationCurve CurveFillAmount;
    public AnimationCurve CurveScaleAndRotation;
    public AnimationCurve CurveRotationSpeed;
    public List<float> LimitRotation = new List<float>();
    public List<float> LimitFillAmount = new List<float>();
}