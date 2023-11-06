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

    public IEnumerator ISetWrite(TextMeshProUGUI TextMessPro, MessageConfig MessageData)
    {
        foreach (MessageConfigSingle MessageSingle in MessageData.List)
        {
            bool ColorFormat = false;
            //
            string Text = MessageSingle.Text;
            //
            if (Text == null)
            {
                yield return new WaitForSeconds(MessageSingle.DelayAlpha);
                continue;
            }
            else
            if (Text == "")
            {
                yield return new WaitForSeconds(MessageSingle.DelayAlpha);
                continue;
            }
            //
            foreach (char MessageChar in Text)
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
                if (MessageSingle.DelaySpace > 0 && MessageChar == ' ')
                    yield return new WaitForSeconds(MessageSingle.DelaySpace);
                else
                if (MessageSingle.DelayAlpha > 0)
                    yield return new WaitForSeconds(MessageSingle.DelayAlpha);
            }
        }
    }
}

[Serializable]
[CreateAssetMenu(fileName = "message-data", menuName = "", order = 0)]
public class MessageConfig : ScriptableObject
{
    public List<MessageConfigSingle> List;

    public MessageConfig()
    {
        List = new List<MessageConfigSingle>();
    }

    public void SetWrite(string Text)
    {
        List.Add(new MessageConfigSingle(Text, Color.clear, 0f, 0f));
    }

    public void SetWrite(string Text, Color Color)
    {
        List.Add(new MessageConfigSingle(Text, Color, 0f, 0f));
    }

    //

    public void SetWrite(string Text, float DelayAlpha, float DelaySpace)
    {
        List.Add(new MessageConfigSingle(Text, Color.clear, DelayAlpha, DelaySpace));
    }

    public void SetWrite(string Text, Color Color, float DelayAlpha, float DelaySpace)
    {
        List.Add(new MessageConfigSingle(Text, Color, DelayAlpha, DelaySpace));
    }

    //

    public void SetWriteByCharacter(string Text, float Delay)
    {
        List.Add(new MessageConfigSingle(Text, Color.clear, Delay, 0f));
    }

    public void SetWriteByCharacter(string Text, Color Color, float Delay)
    {
        List.Add(new MessageConfigSingle(Text, Color, Delay, 0f));
    }

    //

    public void SetWriteByWord(string Text, float Delay)
    {
        List.Add(new MessageConfigSingle(Text, Color.clear, 0f, Delay));
    }

    public void SetWriteByWord(string Text, Color Color, float Delay)
    {
        List.Add(new MessageConfigSingle(Text, Color, 0f, Delay));
    }
    
    //

    public void SetDelay(float Delay)
    {
        List.Add(new MessageConfigSingle(null, Color.clear, Delay, 0f));
    }
}

[Serializable]
public class MessageConfigSingle
{
    [SerializeField] private string m_text;
    [SerializeField] private Color m_color;
    public float DelayAlpha;
    public float DelaySpace;

    public string Text => m_color != Color.clear ? QColor.GetColorHexFormat(m_color, m_text) : m_text;

    public MessageConfigSingle(string Text, Color Color, float DelayAlpha, float DelaySpace)
    {
        this.m_text = Text;
        this.m_color = Color;
        this.DelayAlpha = DelayAlpha;
        this.DelaySpace = DelaySpace;
    }
}