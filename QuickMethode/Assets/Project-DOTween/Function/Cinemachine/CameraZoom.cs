using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField][Min(0)] private float m_scale = 1f;
    [SerializeField][Min(0)] private float m_duration = 2f;

    [Space]
    [SerializeField] private List<string> m_tag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_tag.Contains(collision.tag))
        {
            return;
        }
        //
        CameraController.SetScale(m_scale, m_duration);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!m_tag.Contains(collision.tag))
        {
            return;
        }
        //
        CameraController.SetScale(1f, m_duration);
    }
}