using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    [SerializeField] private StringConfig m_colorConfig;

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

    public IEnumerator ISetWrite(TextMeshProUGUI TextMessPro, MessageDataConfig MessageData)
    {
        foreach (MessageDataConfigText MessageSingle in MessageData.List)
        {
            bool HtmlFormat = false;
            //
            string Text = MessageSingle.Text;
            //
            if (Text == null)
                continue;
            //
            if (MessageSingle.Clear)
                TextMessPro.text = "";
            //
            if (Text != "")
            {
                if (m_colorConfig != null)
                    Text = m_colorConfig.GetColorHexFormatReplace(Text);
                //
                foreach (char MessageChar in Text)
                {
                    //TEXT:
                    TextMessPro.text += MessageChar;
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
            }
            //
            if (MessageSingle.DelayFinal > 0)
                yield return new WaitForSeconds(MessageSingle.DelayFinal);
        }
    }
}