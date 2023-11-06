using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField][TextArea] private string m_text = "Hello World!!";
    [SerializeField] private TextMeshProUGUI m_tmpMyText;

    private IEnumerator Start()
    {
        m_tmpMyText.text = "";
        //
        yield return new WaitForSeconds(3f);
        //
        List<MessageSingle> MessageList = new List<MessageSingle>();
        MessageList.Add(new MessageSingle("Hello!", 0.1f, 0f));
        MessageList.Add(new MessageSingle(" ", 2f, 0f));
        MessageList.Add(new MessageSingle("I will kill you here!", 0f, 2f));
        //
        yield return MessageManager.Instance.ISetWrite(m_tmpMyText, MessageList);
        //
        Debug.Log("[Debug] Message End!!");
    }
}