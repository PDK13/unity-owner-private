using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMessage : MonoBehaviour
{
    [SerializeField] private MessageDataConfigText m_messageConfig;
    [SerializeField] private TextMeshProUGUI m_tmpMyText;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        //
        MessageManager.Instance.SetStart(m_tmpMyText, m_messageConfig);
    }

    private void Update()
    {
        switch (MessageManager.Instance.Stage)
        {
            case MessageStageType.Wait:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    MessageManager.Instance.SetNext();
                }
                break;
            case MessageStageType.Text:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    MessageManager.Instance.SetSkip();
                }
                break;
            case MessageStageType.Choice:
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    MessageManager.Instance.SetChoiceActive(0);
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    MessageManager.Instance.SetChoiceActive(1);
                }
                break;
        }
    }
}