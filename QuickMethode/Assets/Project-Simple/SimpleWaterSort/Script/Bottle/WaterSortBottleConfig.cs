using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "water-sort-bottle-config", menuName = "Water Sort/Bottle Config", order = 0)]
public class WaterSortBottleConfig : ScriptableObject
{
    public float RotateDuration = 2f;

    [Space]
    public List<float> RotateLimit = new List<float>() 
    { 
        90f, 
        80f, 
        60f, 
        40f, 
    };

    [Space]
    public AnimationCurve RotateValueAdd = new AnimationCurve(
        new Keyframe(0.00f, 0.00f),
        new Keyframe(90.0f, 1.50f));

    [Tooltip("Should create 4 point follow list rotate limit")]
    public AnimationCurve RotateValueOut = new AnimationCurve(
        new Keyframe(0.00f, 0),
        new Keyframe(40.0f, 1),
        new Keyframe(60.0f, 2),
        new Keyframe(80.0f, 3),
        new Keyframe(90.0f, 4));

    public AnimationCurve RotateValueSpeed = new AnimationCurve(
        new Keyframe(0.00f, 0.50f),
        new Keyframe(10.0f, 1.00f),
        new Keyframe(90.0f, 1.00f));

    public AnimationCurve RotateValueBackSpeed = new AnimationCurve(
        new Keyframe(0.00f, 5.00f),
        new Keyframe(90.0f, 5.00f));
}