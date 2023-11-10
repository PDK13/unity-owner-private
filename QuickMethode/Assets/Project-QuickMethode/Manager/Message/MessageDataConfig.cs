using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "message-data-config", menuName = "Message Config/Data Config", order = 0)]
public class MessageDataConfig : ScriptableObject
{
    public List<MessageDataConfigText> Message;

    public List<MessageDataConfigChoice> Choice;

    public bool ChoiceAvaible => Choice == null ? false : Choice.Count > 0;
}

[Serializable]
public class MessageDataConfigText
{
    public string Text;
    public MessageDataConfigTextAuthor Author;
    public MessageDataConfigTextDelay Delay;
    public MessageDataConfigTextTrigger Trigger;
}

[Serializable]
public class MessageDataConfigTextDelay
{
    public float Alpha;
    public float Space;
    public float Mark;
}

[Serializable]
public class MessageDataConfigTextAuthor
{
    public Sprite Avatar;
    public string Name;
}

[Serializable]
public class MessageDataConfigTextTrigger
{
    public string Code;
    public GameObject Object;
}

[Serializable]
public class MessageDataConfigChoice 
{
    public string Name;
    public string Description;
    public MessageDataConfig Next;
}