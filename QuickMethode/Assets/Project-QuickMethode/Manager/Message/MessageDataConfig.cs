using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "message-data-config", menuName = "Message Config/Data Config", order = 0)]
public class MessageDataConfig : ScriptableObject
{
    public List<MessageDataConfigText> Message;

    public List<MessageDataConfigChoice> Choice;

    public bool ChoiceAvaible => Choice == null ? false : Choice.Count > 0;

    public MessageDataConfig()
    {
        Message = new List<MessageDataConfigText>();
    }

    public void SetWrite(string Text)
    {
        Message.Add(new MessageDataConfigText(Text,  0f, 0f, 0f));
    }

    public void SetWrite(string Text, float DelayAlpha, float DelaySpace, float DelayFianl)
    {
        Message.Add(new MessageDataConfigText(Text, DelayAlpha, DelaySpace, DelayFianl));
    }

    public void SetWriteByChar(string Text, float Delay)
    {
        Message.Add(new MessageDataConfigText(Text, Delay, 0f, 0f));
    }

    public void SetWriteByWord(string Text, float Delay)
    {
        Message.Add(new MessageDataConfigText(Text, 0f, Delay, 0f));
    }

    public void SetWriteDelay(float Delay)
    {
        Message.Add(new MessageDataConfigText("", 0f, 0f, Delay));
    }
}

[Serializable]
public class MessageDataConfigText
{
    public string Text;
    public float DelayAlpha;
    public float DelaySpace;
    public float DelayFinal;

    public MessageDataConfigText(string Text, float DelayAlpha, float DelaySpace, float DelayFinal)
    {
        this.Text = Text;
        this.DelayAlpha = DelayAlpha;
        this.DelaySpace = DelaySpace;
        this.DelayFinal = DelayFinal;
    }
}

[Serializable]
public class MessageDataConfigChoice 
{
    public string Name;
    public string Description;
    public MessageDataConfig Next;
}