using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "message-config", menuName = "Message/Message Config", order = 0)]
public class MessageDataConfig : ScriptableObject
{
    public List<MessageDataConfigTextAuthor> Author = new List<MessageDataConfigTextAuthor>();

    public MessageDataTextDelayDefault MessageDataDelayDefault;

    public List<string> AuthorName
    {
        get
        {
            List<string> NameFound = new List<string>();
            //
            if (Author == null)
                return NameFound;
            //
            if (Author.Count == 0)
                return NameFound;
            //
            foreach(MessageDataConfigTextAuthor AuthorItem in Author)
                NameFound.Add(AuthorItem.Name);
            //
            return NameFound;
        }
    }

    public List<Sprite> AuthorAvatar
    {
        get
        {
            if (Author == null)
                return null;
            //
            if (Author.Count == 0)
                return null;
            //
            List<Sprite> NameFound = new List<Sprite>();
            //
            foreach (MessageDataConfigTextAuthor AuthorItem in Author)
                NameFound.Add(AuthorItem.Avatar);
            //
            return NameFound;
        }
    }

    public Sprite GetAvatar(string Name)
    {
        return Author.Find(t => t.Name == Name).Avatar;
    }
}

[Serializable]
public class MessageDataConfigTextAuthor
{
    public string Name;
    public Sprite Avatar;
}

[Serializable]
public class MessageDataText
{
    public int AuthorIndex;
    //
    public string Message;
    //
    public float DelayAlpha;
    public float DelaySpace;
    public float DelayMark;
    //
    public string TriggerCode;
    public GameObject TriggerObject;

    public MessageDataText()
    {
        //...
    }

    public MessageDataText(MessageDataTextDelayDefault Default)
    {
        DelayAlpha = Default.DelayAlpha;
        DelaySpace = Default.DelaySpace;
        DelayMark = Default.DelayMark;
    }
}

[Serializable]
public class MessageDataTextDelayDefault
{
    public float DelayAlpha;
    public float DelaySpace;
    public float DelayMark;
}

[Serializable]
public class MessageDataChoice
{
    public string Text;
    public string Message;
    public int AuthorIndex;
    public MessageDataConfigText Next;
}