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
        MessageConfig MessageData = new MessageConfig();
        MessageData.SetWriteByCharacter("Hello there!\n", 0.1f);
        MessageData.SetDelay(2f);
        MessageData.SetWriteByWord("I WILL KILL YOU!\n", 1f);
        MessageData.SetDelay(2f);
        MessageData.SetWrite("HA HA HA!\n", Color.red, 0.1f, 1f);
        //
        yield return MessageManager.Instance.ISetWrite(m_tmpMyText, MessageData);
        //
        Debug.Log("[Debug] Message End!!");
    }
}