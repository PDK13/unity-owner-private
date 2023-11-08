using UnityEngine;

public class SimpleCameraPointWorld : MonoBehaviour
{
    [SerializeField] private Camera m_cameraMain;

    private void Start()
    {
        if (m_cameraMain == null)
        {
            m_cameraMain = Camera.main;
        }
    }

    private void Update()
    {
        Vector3 Pos = QCamera2D.GetPosMouseToWorld();
        Pos.z = transform.position.z;
        transform.position = Pos;
    }
}