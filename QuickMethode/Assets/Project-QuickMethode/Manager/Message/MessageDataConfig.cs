using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "message-data-config", menuName = "Message Config/Data Config", order = 0)]
public class MessageDataConfig : ScriptableObject
{
    public List<MessageDataConfigText> List;

    public List<MessageDataConfigChoice> Choice;

    public MessageDataConfig()
    {
        List = new List<MessageDataConfigText>();
    }

    public void SetWrite(string Text)
    {
        List.Add(new MessageDataConfigText(Text,  0f, 0f, 0f, false));
    }

    public void SetWrite(string Text, float DelayAlpha, float DelaySpace, float DelayFianl)
    {
        List.Add(new MessageDataConfigText(Text, DelayAlpha, DelaySpace, DelayFianl, false));
    }

    public void SetWriteByChar(string Text, float Delay)
    {
        List.Add(new MessageDataConfigText(Text, Delay, 0f, 0f, false));
    }

    public void SetWriteByWord(string Text, float Delay)
    {
        List.Add(new MessageDataConfigText(Text, 0f, Delay, 0f, false));
    }

    public void SetWriteDelay(float Delay)
    {
        List.Add(new MessageDataConfigText("", 0f, 0f, Delay, false));
    }

    public void SetWriteClear()
    {
        List.Add(new MessageDataConfigText("", 0f, 0f, 0f, true));
    }
}

[Serializable]
public class MessageDataConfigText
{
    public string Text;
    public bool Clear;
    public float DelayAlpha;
    public float DelaySpace;
    public float DelayFinal;

    public MessageDataConfigText(string Text, float DelayAlpha, float DelaySpace, float DelayFinal, bool Clear)
    {
        this.Text = Text;
        this.DelayAlpha = DelayAlpha;
        this.DelaySpace = DelaySpace;
        this.DelayFinal = DelayFinal;
        this.Clear = Clear;
    }
}

[Serializable]
public class MessageDataConfigChoice 
{
    public string Name;
    public string Description;
    public MessageDataConfig Next;
}