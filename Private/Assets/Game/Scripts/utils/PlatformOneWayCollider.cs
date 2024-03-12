using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformOneWayCollider : MonoBehaviour
{
    [SerializeField] private Vector2 m_offset = Vector2.zero;
    
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            EdgeCollider2D collider = gameObject.AddComponent<EdgeCollider2D>();
            collider.usedByEffector = true;
            Vector2 start = new Vector2(renderer.bounds.min.x, renderer.bounds.max.y);
            Vector2 end = new Vector2(renderer.bounds.max.x, renderer.bounds.max.y);
            start = (Vector2)transform.InverseTransformPoint(start) + m_offset;
            end = (Vector2)transform.InverseTransformPoint(end) + m_offset;
            collider.points = new Vector2[]{start, end};
        }
    }

    private void OnDrawGizmosSelected()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Gizmos.color = Color.red;
            Vector2 start = new Vector2(renderer.bounds.min.x, renderer.bounds.max.y) + m_offset;
            Vector2 end = new Vector2(renderer.bounds.max.x, renderer.bounds.max.y) + m_offset;
            Gizmos.DrawLine(start, end);
        }
    }
}
