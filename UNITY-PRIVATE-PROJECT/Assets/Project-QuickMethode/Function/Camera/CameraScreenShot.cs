using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScreenShot : MonoBehaviour
{
    public static CameraScreenShot Instance { private set; get; }

    [SerializeField] private KeyCode m_shot = KeyCode.Tab;

    private bool m_shotNextFrame = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_shot))
            m_shotNextFrame = true;
    }

    private void OnPostRender()
    {
        if (!m_shotNextFrame)
            return;
        m_shotNextFrame = false;
        //
        QScreenShot.SetScreenShotFullScreen();
    }

    public void SetShot()
    {
        m_shotNextFrame = true;
    }
}