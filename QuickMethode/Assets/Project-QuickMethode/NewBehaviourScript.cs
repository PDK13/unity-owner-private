using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private MessageDataConfig m_messageConfig;
    [SerializeField] private TextMeshProUGUI m_tmpMyText;

    private void Start()
    {
        MessageManager.Instance.SetStart(m_tmpMyText, m_messageConfig);
    }

    private void Update()
    {
        if (MessageManager.Instance.Wait)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MessageManager.Instance.SetNext();
            }
        }
        //
        if (MessageManager.Instance.Text)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MessageManager.Instance.SetSkip();
            }
        }
        //
    }
}