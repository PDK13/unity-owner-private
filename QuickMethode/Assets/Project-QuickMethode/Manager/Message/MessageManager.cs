using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    #region Varible: Setting

    [SerializeField] private StringConfig m_stringConfig;

    #endregion

    #region Event

    public Action<bool> onWait;
    public Action<bool> onText;
    public Action<bool> onChoice;
    public Action<bool> onEnd;

    #endregion

    #region Varible: Message Manager

    private enum MessageCommandType
    {
        None,
        Text,
        Done,
        Wait,
        Next,
        Skip,
        Choice,
    }

    private MessageCommandType m_command = MessageCommandType.Text;
    private MessageDataConfig m_data;

    private TextMeshProUGUI m_tmp;
    private string m_current = "";

    private Coroutine m_iSetMessageShowSingle;

    public List<MessageDataConfigChoice> ChoiceList => m_data.Choice;

    private bool m_active = false;
    private bool m_choice = false;

    public MessageStageType Stage
    {
        get
        {
            if (m_active & m_choice)
                return MessageStageType.Choice;
            //
            if (m_active & m_command == MessageCommandType.Wait)
                return MessageStageType.Wait;
            //
            if (m_active & m_command == MessageCommandType.Text)
                return MessageStageType.Text;
            //
            return MessageStageType.None;
        }
    }

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //
        Instance = this;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    #region Main

    public void SetStart(TextMeshProUGUI TextMessPro, MessageDataConfig MessageData)
    {
        if (m_active)
            return;
        //
        m_data = MessageData;
        m_tmp = TextMessPro;
        //
        StartCoroutine(ISetMessageShow());
    }

    private IEnumerator ISetMessageShow()
    {
        m_command = MessageCommandType.None;
        m_active = true;
        m_choice = false;
        //
        Debug.Log("[Message] Start!");
        //
        for (int i = 0; i < m_data.Message.Count; i++)
        {
            MessageDataConfigText MessageSingle = m_data.Message[i];
            m_current = MessageSingle.Text;
            //
            //MESSAGE:
            if (m_current == null)
                m_tmp.text = "...";
            else
            if (m_current == "")
                m_tmp.text = "...";
            else
            {
                //BEGIN:
                m_tmp.text = "";
                //
                if (m_stringConfig != null)
                    m_current = m_stringConfig.GetColorHexFormatReplace(m_current);
                //
                //PROGESS:
                m_command = MessageCommandType.Text;
                onText?.Invoke(true);
                //
                Debug.Log("[Message] " + m_current);
                //
                m_iSetMessageShowSingle = StartCoroutine(ISetMessageShowSingle(MessageSingle));
                yield return new WaitUntil(() => m_command == MessageCommandType.Skip || m_command == MessageCommandType.Done);
                //
                //DONE:
                m_command = MessageCommandType.Wait;
                onText?.Invoke(false);
                //
                m_tmp.text = m_current;
            }
            //
            //FINAL:
            if (MessageSingle.DelayFinal > 0)
                yield return new WaitForSeconds(MessageSingle.DelayFinal);
            //
            //WAIT:
            if (m_current != "" && i < m_data.Message.Count - 1)
            {
                m_command = MessageCommandType.Wait;
                //
                Debug.Log("[Message] Next?");
                //
                onWait?.Invoke(true);
                yield return new WaitUntil(() => m_command == MessageCommandType.Next);
                onWait?.Invoke(false);
            }
        }
        //
        m_command = m_data.ChoiceAvaible ? MessageCommandType.Wait : MessageCommandType.None;
        m_active = m_data.ChoiceAvaible;
        m_choice = m_data.ChoiceAvaible;
        //
        onChoice?.Invoke(m_active);
        onEnd?.Invoke(!m_active);
        //
        if (m_choice)
            Debug.Log("[Message] Choice?");
        else
            Debug.Log("[Message] End!");
    }

    private IEnumerator ISetMessageShowSingle(MessageDataConfigText MessageSingle)
    {
        bool HtmlFormat = false;
        //
        foreach (char MessageChar in m_current)
        {
            //TEXT:
            m_tmp.text += MessageChar;
            //
            //COLOR:
            if (!HtmlFormat && MessageChar == '<')
            {
                HtmlFormat = true;
                continue;
            }
            else
            if (HtmlFormat && MessageChar == '>')
            {
                HtmlFormat = false;
                continue;
            }
            //
            //DELAY:
            if (HtmlFormat)
                continue;
            //
            if (MessageSingle.DelaySpace > 0 && (MessageChar == ' '))
                yield return new WaitForSeconds(MessageSingle.DelaySpace);
            else
            if (MessageSingle.DelayAlpha > 0)
                yield return new WaitForSeconds(MessageSingle.DelayAlpha);
        }
        //
        m_command = MessageCommandType.Done;
    }

    #endregion

    #region Control

    public void SetNext()
    {
        if (Stage != MessageStageType.Wait)
            return;
        //
        if (m_command != MessageCommandType.Wait)
            //When current message in done show up, press Next to move on next message!
            return;
        //
        m_command = MessageCommandType.Next;
    }

    public void SetSkip()
    {
        if (Stage != MessageStageType.Text)
            return;
        //
        if (m_command != MessageCommandType.Text)
            //When current message is showing up, press Next to skip and show full message!
            return;
        //
        StopCoroutine(m_iSetMessageShowSingle);
        //
        m_command = MessageCommandType.Skip;
        //
        Debug.Log("[Message] Skip!");
    }

    public void SetChoice(int ChoiceIndex)
    {
        if (Stage != MessageStageType.Choice)
            return;
        //
        if (m_command != MessageCommandType.Wait)
            //When current message in done show up and got choice option, press Choice Option to move on next message!
            return;
        //
        if (ChoiceIndex < 0 || ChoiceIndex > m_data.Choice.Count - 1)
            return;
        //
        m_command = MessageCommandType.Choice;
        m_data = m_data.Choice[ChoiceIndex].Next;
        StartCoroutine(ISetMessageShow());
        //
        Debug.Log("[Message] Choice " + ChoiceIndex);
    }

    public void SetStop()
    {
        StopAllCoroutines();
        StopCoroutine(m_iSetMessageShowSingle);
        //
        m_tmp.text = "";
    }

    #endregion
}

public enum MessageStageType
{
    None,
    Text,
    Wait,
    Choice,
}