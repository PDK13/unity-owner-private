using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private Transform m_from;
    [SerializeField] private Transform m_target;
    [SerializeField] private float m_force = 15f;
    [SerializeField] private float m_gravityScale = 1;
    [SerializeField] private bool m_AngleHigh = true;

    private LineRenderer m_lineRenderer;

    private void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float? Deg = QTrajectory.GetDegToTarget(m_from.position, m_target.position, m_force, m_gravityScale, m_AngleHigh);

            if (Deg.HasValue)
            {
                Deg = transform.localScale.x == 1 ? Deg : QCircle.GetDegOppositeUD(Deg.Value);
                Debug.Log("Deg Found: " + Deg.Value);
                List<Vector3> Path = QTrajectory.GetTrajectory(m_from.position, Deg.Value, m_force, m_gravityScale);
                m_lineRenderer.positionCount = Path.Count;
                m_lineRenderer.SetPositions(Path.ToArray());
            }
            else
            {
                m_lineRenderer.positionCount = 0;
            }
        }
    }
}