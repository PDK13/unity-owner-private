using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

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

    public IEnumerator ISetWrite(TextMeshProUGUI TextMessPro, MessageManagerData MessageData)
    {
        foreach (MessageManagerDataSingle MessageSingle in MessageData.List)
        {
            bool ColorFormat = false;
            //
            if (MessageSingle.Text == null)
            {
                yield return new WaitForSeconds(MessageSingle.DelayAlpha);
                continue;
            }
            //
            foreach(char MessageChar in MessageSingle.Text)
            {
                //TEXT:
                TextMessPro.text += MessageChar;
                //
                //COLOR:
                if (!ColorFormat && MessageChar == '<')
                {
                    ColorFormat = true;
                    continue;
                }
                else
                if (ColorFormat && MessageChar == '>')
                {
                    ColorFormat = false;
                    continue;
                }
                //
                //DELAY:
                if (ColorFormat)
                    continue;
                //
                if (MessageChar == ' ')
                    yield return new WaitForSeconds(MessageSingle.DelaySpace);
                else
                    yield return new WaitForSeconds(MessageSingle.DelayAlpha);
            }
        }
    }
}

[Serializable]
public class MessageManagerData
{
    public List<MessageManagerDataSingle> List;

    public MessageManagerData()
    {
        List = new List<MessageManagerDataSingle>();
    }

    public void SetWrite(string Text)
    {
        List.Add(new MessageManagerDataSingle(Text, 0f, 0f));
    }

    public void SetWrite(string Text, float DelayAlpha, float DelaySpace)
    {
        List.Add(new MessageManagerDataSingle(Text, DelayAlpha, DelaySpace));
    }

    public void SetWriteByCharacter(string Text, float Delay)
    {
        List.Add(new MessageManagerDataSingle(Text, Delay, 0f));
    }

    public void SetWriteByWord(string Text, float Delay)
    {
        List.Add(new MessageManagerDataSingle(Text, 0f, Delay));
    }

    public void SetDelay(float Delay)
    {
        List.Add(new MessageManagerDataSingle(null, Delay, 0f));
    }
}

[Serializable]
public class MessageManagerDataSingle
{
    public string Text;
    public float DelayAlpha;
    public float DelaySpace;

    public MessageManagerDataSingle(string Text, float DelayAlpha, float DelaySpace)
    {
        this.Text = Text;
        this.DelayAlpha = DelayAlpha;
        this.DelaySpace = DelaySpace;
    }
}