using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCode : MonoBehaviour
{
    private int value = 3;
    private int? number = null;

    private List<int> numbers;

    private void Awake()
    {
        
    }

    private void Test01()
    {
        Debug.Log("[number] == null = " + (number == null));
        Debug.Log("[number] is null = " + (number is null));
        Debug.Log("[number] ?? 5 = " + (number ?? 5));
        Debug.Log("-------------------------------------");
        //
        number = (number ??= value);
        Debug.Log("[number] ??= value (value = " + value + ")");
        Debug.Log("-------------------------------------");
        //
        Debug.Log("[number] == null = " + (number == null));
        Debug.Log("[number] is null = " + (number is null));
        Debug.Log("[number] ?? 5 = " + (number ?? 5));
        Debug.Log("-------------------------------------");
        //
        Debug.Log("[number] value = " + number);
    }

    private void Test02()
    {
        Debug.Log("[numbers] count " + (numbers != null ? numbers.Count : "null"));
        (numbers ??= new List<int>()).Add(value);
        Debug.Log("[numbers] count " + (numbers != null ? numbers.Count : "null"));
    }
}