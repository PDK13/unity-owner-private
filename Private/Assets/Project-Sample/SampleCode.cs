using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCode : MonoBehaviour
{
    private int value = 3;
    private int? number = null;

    private void Awake()
    {
        //Debug.Log("[number] == null = " + (number == null));
        //Debug.Log("[number] is null = " + (number is null));
        //Debug.Log("[number] ?? 5 = " + (number ?? 5));
        //Debug.Log("-------------------------------------");
        ////
        //number = (number ??= value);
        //Debug.Log("[number] ??= value (value = " + value + ")");
        //Debug.Log("-------------------------------------");
        ////
        //Debug.Log("[number] == null = " + (number == null));
        //Debug.Log("[number] is null = " + (number is null));
        //Debug.Log("[number] ?? 5 = " + (number ?? 5));
        //Debug.Log("-------------------------------------");
        ////
        //Debug.Log("[number] value = " + number);
    }

    private void Start()
    {
        //QPlayerPrefs.SetValue(null, 3);
        Debug.Log("[pref] " + QPlayerPrefs.GetValueInt(""));
    }
}