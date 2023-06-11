using QuickMethode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleCameraPointWorld : MonoBehaviour
{
    [SerializeField] private Camera m_cameraMain;

    private void Start()
    {
        if (m_cameraMain == null)
            m_cameraMain = Camera.main;
    }

    private void Update()
    {
        Vector3 Pos = QCamera.GetPosMouseToWorld();
        Pos.z = this.transform.position.z;
        this.transform.position = Pos;
    }
}