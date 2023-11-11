using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "message-author-config", menuName = "Message Config/Author Config", order = 0)]
public class MessageDataConfigAuthor : ScriptableObject
{
    public List<MessageDataConfigTextAuthor> Author = new List<MessageDataConfigTextAuthor>();

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