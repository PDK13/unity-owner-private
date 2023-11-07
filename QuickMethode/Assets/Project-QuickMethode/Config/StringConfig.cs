using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "string-config", menuName = "", order = 0)]
public class StringConfig : ScriptableObject
{
    [SerializeField] private string m_colorClear = "#clear";
    [SerializeField] private List<MessageColorConfigSingle> m_color = new List<MessageColorConfigSingle>()
    {
        new MessageColorConfigSingle("#red", Color.red),
        new MessageColorConfigSingle("#blue", Color.blue),
        new MessageColorConfigSingle("#green", Color.green),
        new MessageColorConfigSingle("#yellow", Color.yellow),
        new MessageColorConfigSingle("#white", Color.white),
        new MessageColorConfigSingle("#cyan", Color.cyan),
        new MessageColorConfigSingle("#mageta", Color.magenta),
        new MessageColorConfigSingle("#black", Color.black),
    };

    [Space]
    [SerializeField] private string m_codeReturn = "#return";

    public string GetColorHexFormatReplace(string Value)
    {
        //COLOR:
        Value = Value.Replace(m_colorClear, "</color>");
        foreach (MessageColorConfigSingle Code in m_color)
            Value = Value.Replace(Code.Code, string.Format("<{0}>", QColor.GetColorHexFormat(Code.Color)));
        //
        //CODE:
        Value = Value.Replace(m_codeReturn, "\n");
        //
        return Value;
    }
}

[Serializable]
public class MessageColorConfigSingle
{
    public string Code;
    public Color Color;

    public MessageColorConfigSingle(string Code, Color Color)
    {
        this.Code = Code;
        this.Color = Color;
    }
}