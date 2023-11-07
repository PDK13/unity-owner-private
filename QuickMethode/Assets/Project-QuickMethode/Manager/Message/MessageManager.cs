using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    #region Varible: Setting

    [SerializeField] private StringConfig m_stringConfig;

    #endregion

    #region Event

    public Action<bool> onWait;
    public Action onStart;
    public Action onChoice;
    public Action onEnd;

    #endregion

    #region Varible: Message Manager

    private enum MessageCommandType
    {
        Text,
        Wait,
        Next,
        Skip,
        Choice,
    }

    private MessageCommandType m_messageCommand = MessageCommandType.Text;
    private MessageDataConfig m_messageData;
    private TextMeshProUGUI m_textMessPro;
    private string m_messageCurrent = "";
    private int m_messageChoice = -1;

    private Coroutine m_iSetMessageShowSingle;

    public bool Text => m_messageCommand == MessageCommandType.Text;

    public bool Wait => m_messageCommand == MessageCommandType.Wait;

    public List<MessageDataConfigChoice> Choice => m_messageData.Choice;

    #endregion

    private bool m_stop = false;

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

    public void SetStart(TextMeshProUGUI TextMessPro, MessageDataConfig MessageData)
    {
        m_messageData = MessageData;
        m_textMessPro = TextMessPro;
        //
        StartCoroutine(ISetMessageShow());
    }

    private IEnumerator ISetMessageShow()
    {
        onStart?.Invoke();
        //
        foreach (MessageDataConfigText MessageSingle in m_messageData.Message)
        {
            m_messageCurrent = MessageSingle.Text;
            //
            //MESSAGE:
            if (m_messageCurrent == null)
                m_textMessPro.text = "...";
            else
            if (m_messageCurrent == "")
                m_textMessPro.text = "...";
            else
            {
                //BEGIN:
                m_textMessPro.text = "";
                //
                if (m_stringConfig != null)
                    m_messageCurrent = m_stringConfig.GetColorHexFormatReplace(m_messageCurrent);
                //
                //PROGESS:
                m_iSetMessageShowSingle = StartCoroutine(ISetMessageShowSingle(MessageSingle));
                yield return new WaitUntil(() => m_messageCommand == MessageCommandType.Skip || m_messageCommand == MessageCommandType.Wait);
                //
                //DONE:
                m_textMessPro.text = m_messageCurrent;
            }
            //
            //FINAL:
            if (MessageSingle.DelayFinal > 0)
                yield return new WaitForSeconds(MessageSingle.DelayFinal);
            //
            //WAIT:
            if (m_messageCurrent != "" && MessageSingle.DelayWait)
            {
                m_messageCommand = MessageCommandType.Wait;
                //
                onWait?.Invoke(true);
                yield return new WaitUntil(() => m_messageCommand == MessageCommandType.Next);
                onWait?.Invoke(false);
            }
        }
        //
        if (m_messageData.ChoiceAvaible)
            onChoice?.Invoke();
        else
            onEnd?.Invoke();
    }

    private IEnumerator ISetMessageShowSingle(MessageDataConfigText MessageSingle)
    {
        m_messageCommand = MessageCommandType.Text;
        //
        bool HtmlFormat = false;
        //
        foreach (char MessageChar in m_messageCurrent)
        {
            //TEXT:
            m_textMessPro.text += MessageChar;
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
        m_messageCommand = MessageCommandType.Wait;
    }

    public void SetNext()
    {
        if (m_messageCommand != MessageCommandType.Wait)
            //When current message in done show up, press Next to move on next message!
            return;
        //
        m_messageCommand = MessageCommandType.Next;
    }

    public void SetSkip()
    {
        if (m_messageCommand != MessageCommandType.Text)
            //When current message is showing up, press Next to skip and show full message!
            return;
        //
        StopCoroutine(m_iSetMessageShowSingle);
        m_messageCommand = MessageCommandType.Skip;
    }

    public void SetChoice(int ChoiceIndex)
    {
        if (m_messageCommand != MessageCommandType.Wait)
            //When current message in done show up and got choice option, press Choice Option to move on next message!
            return;
        //
        m_messageCommand = MessageCommandType.Choice;
        //

    }
}