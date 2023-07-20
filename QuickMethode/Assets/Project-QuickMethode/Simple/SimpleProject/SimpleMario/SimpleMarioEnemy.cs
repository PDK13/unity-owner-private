using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMarioEnemy : MonoBehaviour
{
    [SerializeField] private float m_stompDelay = 1f;

    [Space]
    [SerializeField] private Collider2D m_bodyTrigger;
    [SerializeField] private Collider2D m_headTrigger;

    private void OnStomp(string Message)
    {
        StartCoroutine(ISetStompDelay());
    }

    private IEnumerator ISetStompDelay()
    {
        m_bodyTrigger.enabled = false;
        m_headTrigger.enabled = false;

        yield return new WaitForSeconds(m_stompDelay);

        m_bodyTrigger.enabled = true;
        m_headTrigger.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("[Debug] Enemy Hit Player!!");
    }
}
