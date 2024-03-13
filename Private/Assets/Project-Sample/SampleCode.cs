using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SampleCode : MonoBehaviour
{
    private int value = 3;
    private int? number = null;

    [Serializable]
    public class SimpleClass
    {
        public int valueA;
        public int valueB;
    }

    public List<int> numbers = new List<int>();

    public List<SimpleClass> data = new List<SimpleClass>();

    private void Awake()
    {
        Test03();
    }

    private void Test03()
    {
        IEnumerable<int> numbercheck = 
            from valueData 
            in data 
            where (valueData.valueB % 2 == 0) 
            select valueData.valueA;
        //
        foreach(int valueCheck in numbercheck)
            Debug.Log(valueCheck);
    }

    //

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