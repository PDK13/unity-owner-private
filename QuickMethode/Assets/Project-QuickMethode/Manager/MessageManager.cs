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

    public IEnumerator ISetWrite(TextMeshProUGUI TextMessPro, List<MessageSingle> Message)
    {
        foreach (MessageSingle MessageItem in Message)
        {

            foreach(char MessageChar in MessageItem.Text)
            {
                TextMessPro.text += MessageChar;
                //
                if (MessageChar == ' ')
                    yield return new WaitForSeconds(MessageItem.DelaySpace);
                else
                    yield return new WaitForSeconds(MessageItem.DelayAlpha);
            }
        }
    }
}

[Serializable]
public class MessageSingle
{
    public string Text;
    public float DelayAlpha;
    public float DelaySpace;

    public MessageSingle(string Text, float DelayAlpha, float DelaySpace)
    {
        this.Text = Text;
        this.DelayAlpha = DelayAlpha;
        this.DelaySpace = DelaySpace;
    }
}