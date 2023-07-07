using QuickMethode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class UniqueInstance : MonoBehaviour
{
    [SerializeField] private string m_id = "";

    public string ID => m_id;

    private void Awake()
    {
        if (m_id == "")
        {
            SetUpdate();
            return;
        }

        Event Event = Event.current;

        if (Event != null)
        {
            if (Event.type == EventType.ValidateCommand)
            {
                if (Event.commandName == "Paste" || Event.commandName == "Duplicate")
                {
                    SetUpdate();
                }
                Event.Use();
            }

            if (Event.type == EventType.ExecuteCommand)
            {
                if (Event.commandName == "Paste" || Event.commandName == "Duplicate")
                {
                    SetUpdate();
                }
            }
        }
    }

    private void OnValidate()
    {
        if (m_id == "")
            SetUpdate();
    }

    private void SetUpdate()
    {
        string ID1 = QDateTime.GetFormat(DateTime.Now, "yyMMddHHmmssfff").ToString();
        m_id = ID1;
    }
}