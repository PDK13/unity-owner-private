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
        MessageManagerData MessageData = new MessageManagerData();
        MessageData.SetWriteByCharacter("Hello there!\n", 0.1f);
        MessageData.SetDelay(2f);
        MessageData.SetWriteByWord("I WILL KILL YOU!\n", 1f);
        MessageData.SetDelay(2f);
        MessageData.SetWrite(QColor.GetColorHexFormat(Color.red, "HA HA HA!\n"), 0.1f, 1f);
        //
        yield return MessageManager.Instance.ISetWrite(m_tmpMyText, MessageData);
        //
        Debug.Log("[Debug] Message End!!");
    }
}