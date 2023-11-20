using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "water-sort-manager-config", menuName = "Water Sort/Manager Config", order = 0)]
public class WaterSortManagerConfig : ScriptableObject
{
    public float BottleChoiceOffsetY = 0.5f;
    public float BottleFillOffsetY = 2f;

    [Space]
    public float BottleMoveDuration = 0.25f;
    public Ease BottleMoveEase = Ease.OutQuad;
}