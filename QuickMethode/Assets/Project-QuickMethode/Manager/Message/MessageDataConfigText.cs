using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "message-text-config", menuName = "Message/Text Config", order = 0)]
public class MessageDataConfigText : ScriptableObject
{
    public List<MessageDataText> Message = new List<MessageDataText>();

    public List<MessageDataChoice> Choice = new List<MessageDataChoice>();

    public bool ChoiceAvaible => Choice == null ? false : Choice.Count > 0;
}

#if UNITY_EDITOR

[CustomEditor(typeof(MessageDataConfigText))]
public class MessageDataConfigTextEditor : Editor
{
    private const float POPUP_HEIGHT = 150f * 2;
    private const float LABEL_WIDTH = 65f;

    private MessageDataConfigText m_target;
    private MessageDataConfig m_authorConfig;

    private string m_authorConfigError = "";

    private int m_messageCount = 0;
    private int m_choiceCount = 0;

    private List<string> m_messageAuthorName;
    private List<bool> m_messageDelayShow;
    private List<bool> m_messageTriggerShow;

    private Vector2 m_scrollMessage;
    private Vector2 m_scrollChoice;

    private void OnEnable()
    {
        m_target = target as MessageDataConfigText;
        //
        m_messageCount = m_target.Message.Count;
        m_choiceCount = m_target.Message.Count;
        //
        SetConfigAuthorFind();
    }

    public override void OnInspectorGUI()
    {
        if (m_authorConfigError != "")
        {
            QEditorCustom.SetLabel(m_authorConfigError, QEditorCustom.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter));
            return;
        }
        //
        SetGUIGroupMessage();
        //

    }

    //

    private void SetConfigAuthorFind()
    {
        var AuthorConfigFound = QAssetsDatabase.GetScriptableObject<MessageDataConfig>("");
        //
        if (AuthorConfigFound == null)
        {
            m_authorConfigError = "Author Config not found, please create one";
            Debug.Log("[Message] " + m_authorConfigError);
            return;
        }
        //
        if (AuthorConfigFound.Count == 0)
        {
            m_authorConfigError = "Author Config not found, please create one";
            Debug.Log("[Message] " + m_authorConfigError);
            return;
        }
        //
        if (AuthorConfigFound.Count > 1)
            Debug.Log("[Message] Author Config found more than one, get the first one found");
        //
        m_authorConfig = AuthorConfigFound[0];
        //
        if (m_authorConfig.Author.Count == 0)
        {
            m_authorConfigError = "Author Config not have any data, please add one";
            Debug.Log("[Message] " + m_authorConfigError);
            return;
        }
        //
        //CONTINUE:
        //
        m_messageAuthorName = m_authorConfig.AuthorName;
        //
        m_messageDelayShow = new List<bool>();
        while (m_messageDelayShow.Count < m_target.Message.Count) m_messageDelayShow.Add(false);
        //
        m_messageTriggerShow = new List<bool>();
        while (m_messageTriggerShow.Count < m_target.Message.Count) m_messageTriggerShow.Add(false);
        //
        m_authorConfigError = "";
    }

    private void SetGUIGroupMessage()
    {
        QEditorCustom.SetLabel("MESSAGE", QEditorCustom.GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        //COUNT:
        QEditorCustom.SetHorizontalBegin();
        QEditorCustom.SetLabel("Count", null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
        //
        m_messageCount = QEditorCustom.SetField(m_messageCount);
        //
        if (QEditorCustom.SetButton("+"))
            m_messageCount++;
        //
        if (QEditorCustom.SetButton("-"))
            if (m_messageCount > 0)
                m_messageCount--;
        //
        QEditorCustom.SetHorizontalEnd();
        //COUNT:
        //
        while (m_messageCount > m_target.Message.Count)
        {
            m_target.Message.Add(new MessageDataText(m_authorConfig.MessageTextDelayDefault));
            m_messageDelayShow.Add(false);
            m_messageTriggerShow.Add(false);
        }
        while (m_messageCount < m_target.Message.Count)
        {
            m_target.Message.RemoveAt(m_target.Message.Count - 1);
            m_messageDelayShow.RemoveAt(m_messageDelayShow.Count - 1);
            m_messageTriggerShow.RemoveAt(m_messageTriggerShow.Count - 1);
        }
        //
        QEditorCustom.SetSpace(10);
        //
        m_scrollMessage = QEditorCustom.SetScrollViewBegin(m_scrollMessage, QEditorCustom.GetGUIHeight(POPUP_HEIGHT));
        for (int i = 0; i < m_target.Message.Count; i++)
        {
            //ITEM:
            QEditorCustom.SetHorizontalBegin();
            QEditorCustom.SetLabel(i.ToString(), QEditorCustom.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter), QEditorCustom.GetGUIWidth(25));
            //
            QEditorCustom.SetVerticalBegin();
            //ITEM - AUTHOR
            QEditorCustom.SetHorizontalBegin();
            QEditorCustom.SetLabel("Author", null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
            m_target.Message[i].AuthorIndex = QEditorCustom.SetPopup(m_target.Message[i].AuthorIndex, m_messageAuthorName);
            QEditorCustom.SetHorizontalEnd();
            //ITEM - AUTHOR
            //
            //ITEM - MESSAGE:
            QEditorCustom.SetHorizontalBegin();
            QEditorCustom.SetLabel("Message", null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
            m_target.Message[i].Message = QEditorCustom.SetField(m_target.Message[i].Message);
            QEditorCustom.SetHorizontalEnd();
            //ITEM - MESSAGE:
            //
            //ITEM - DELAY
            QEditorCustom.SetHorizontalBegin();
            if (QEditorCustom.SetButton("Delay", null, QEditorCustom.GetGUIWidth(LABEL_WIDTH)))
                m_messageDelayShow[i] = !m_messageDelayShow[i];
            //
            if (m_messageDelayShow[i])
            {
                QEditorCustom.SetVerticalBegin();
                //
                QEditorCustom.SetHorizontalBegin();
                QEditorCustom.SetLabel("Alpha", null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
                m_target.Message[i].DelayAlpha = QEditorCustom.SetField(m_target.Message[i].DelayAlpha);
                QEditorCustom.SetHorizontalEnd();
                //
                QEditorCustom.SetHorizontalBegin();
                QEditorCustom.SetLabel("Space", null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
                m_target.Message[i].DelaySpace = QEditorCustom.SetField(m_target.Message[i].DelaySpace);
                QEditorCustom.SetHorizontalEnd();
                //
                QEditorCustom.SetHorizontalBegin();
                QEditorCustom.SetLabel("Mark", null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
                m_target.Message[i].DelayMark = QEditorCustom.SetField(m_target.Message[i].DelayMark);
                QEditorCustom.SetHorizontalEnd();
                QEditorCustom.SetVerticalEnd();
            }
            else
            {
                QEditorCustom.SetLabel("Alpha: " + m_target.Message[i].DelayAlpha, null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
                QEditorCustom.SetLabel("Space: " + m_target.Message[i].DelaySpace, null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
                QEditorCustom.SetLabel("Mark: " + m_target.Message[i].DelayMark, null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
            }
            //
            QEditorCustom.SetHorizontalEnd();
            //
            //ITEM - DELAY
            //
            //ITEM - TRIGGER:
            QEditorCustom.SetHorizontalBegin();
            if (QEditorCustom.SetButton("Trigger", null, QEditorCustom.GetGUIWidth(LABEL_WIDTH)))
                m_messageTriggerShow[i] = !m_messageTriggerShow[i];
            //
            if (m_messageTriggerShow[i])
            {
                QEditorCustom.SetVerticalBegin();
                //
                QEditorCustom.SetHorizontalBegin();
                QEditorCustom.SetLabel("Code", null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
                m_target.Message[i].TriggerCode = QEditorCustom.SetField(m_target.Message[i].TriggerCode);
                QEditorCustom.SetHorizontalEnd();
                //
                QEditorCustom.SetHorizontalBegin();
                QEditorCustom.SetLabel("Object", null, QEditorCustom.GetGUIWidth(LABEL_WIDTH));
                m_target.Message[i].TriggerObject = QEditorCustom.SetField(m_target.Message[i].TriggerObject);
                QEditorCustom.SetHorizontalEnd();
                //
                QEditorCustom.SetVerticalEnd();
            }
            else
            {
                QEditorCustom.SetLabel("Code: " + m_target.Message[i].TriggerCode, null, QEditorCustom.GetGUIWidth(LABEL_WIDTH * 2 + 4));
                QEditorCustom.SetLabel("" + (m_target.Message[i].TriggerObject != null ? m_target.Message[i].TriggerObject.name : ""));
            }
            //
            QEditorCustom.SetHorizontalEnd();
            //
            //ITEM - TRIGGER:
            //
            QEditorCustom.SetHorizontalEnd();
            //ITEM:
            QEditorCustom.SetVerticalEnd();
            //
            QEditorCustom.SetSpace(10);
        }
        QEditorCustom.SetScrollViewEnd();
    }
}

#endif