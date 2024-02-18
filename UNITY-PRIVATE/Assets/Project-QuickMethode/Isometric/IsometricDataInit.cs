using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IsometricDataInit : MonoBehaviour
{
    public List<string> Data = new List<string>();

    public bool DataExist => Data == null ? false : Data.Count == 0 ? false : true;

    public void SetValue(IsometricDataInit Value)
    {
        Data = Value.Data;
    }
}