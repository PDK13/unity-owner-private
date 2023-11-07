using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private MessageDataConfig m_messageConfig;
    [SerializeField] private TextMeshProUGUI m_tmpMyText;

    private DateTime m_time = DateTime.Now;

    private IEnumerator Start()
    {
        m_tmpMyText.text = "";
        //
        yield return new WaitForSeconds(3f);
        //
        //MessageConfig MessageData = new MessageConfig();
        //MessageData.SetWriteByCharacter("Hello there!\n", 0.1f);
        //MessageData.SetDelay(2f);
        //MessageData.SetWriteByWord("I WILL KILL YOU!\n", 1f);
        //MessageData.SetDelay(2f);
        //MessageData.SetWrite("HA HA HA!\n", Color.red, 0.1f, 1f);
        //
        //yield return MessageManager.Instance.ISetWrite(m_tmpMyText, MessageData);
        //
        yield return MessageManager.Instance.ISetWrite(m_tmpMyText, m_messageConfig);
        //
        Debug.Log("[Debug] Message End!!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.LogFormat("[Debug] {0}", (DateTime.Now - m_time).TotalSeconds);
        }
    }
}