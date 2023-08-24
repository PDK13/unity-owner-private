using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoopController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private BackgroundLoopType m_cameraLoop = BackgroundLoopType.Horizontal;
    [SerializeField] private Camera m_camera;
    private float m_cameraBasePosX = 0;

    [Header("Background")]
    [SerializeField] private SpriteRenderer m_background;
    private float m_backgroundLocalX = 0;
    private float m_backgroundBoundX;
    private Vector2 BackgroundSize => QSprite.GetSpriteSizeUnit(m_background.sprite);

    [SerializeField] private bool m_layerMask = false;
    [SerializeField] private bool m_layerLimitY = false;
    [SerializeField] private Vector2 m_layerScale = new Vector2(3f, 2f);
    [SerializeField] private Vector2 m_layerOffset = new Vector2(0f, 0f);
    [SerializeField] private float m_layerAlpha = 0f;
    private Transform m_layerMaskCheck;
    private Vector2 LayerScale => new Vector2(m_layerScale.x * m_background.transform.localScale.x, m_layerScale.y * transform.localScale.y);
    private Vector2 LayerOffset => new Vector2(m_layerOffset.x * m_background.transform.localScale.x, m_layerOffset.y * transform.localScale.y);
    private Vector2 LayerSize => new Vector2(BackgroundSize.x * LayerScale.x, BackgroundSize.y * LayerScale.y); //???

    [Header("Background Layer")]
    [SerializeField] private List<BackgroundLoopLayer> m_backgroundLayer;

    private void Start()
    {
        if (m_camera == null)
            m_camera = Camera.main;

        if (m_layerMask)
        {
            GameObject LayerClone = QGameObject.SetCreate("LayerMask", this.transform);
            LayerClone.transform.localScale = m_layerScale;
            LayerClone.transform.position = (Vector2)m_background.transform.position + m_layerOffset;
            m_layerMaskCheck = LayerClone.transform;
            //SpriteRenderer!!
            SpriteRenderer LayerSprite = QComponent.GetComponent<SpriteRenderer>(LayerClone);
            LayerSprite.sprite = m_background.sprite;
            LayerSprite.enabled = false;
            //SpriteMask!!
            SpriteMask LayerMask = QComponent.GetComponent<SpriteMask>(LayerClone);
            LayerMask.sprite = m_background.sprite;
            LayerMask.alphaCutoff = m_layerAlpha;
            //BackgroundLayer!!
            m_backgroundLayer.Add(new BackgroundLoopLayer(LayerSprite, 1f, false));
        }

        switch (m_cameraLoop)
        {
            case BackgroundLoopType.Horizontal:
                m_backgroundBoundX = m_background.bounds.size.x;
                m_backgroundLocalX = m_background.transform.localScale.x;
                m_cameraBasePosX = m_camera.transform.position.x;
                for (int i = 0; i < m_backgroundLayer.Count; i++)
                {
                    if (m_layerMask)
                        m_backgroundLayer[i].Layer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                    m_backgroundLayer[i].PrimaryPosX = GetCameraX(m_backgroundLayer[i]);
                    m_backgroundLayer[i].PrimaryPosY = m_backgroundLayer[i].Transform.localPosition.y;

                    m_backgroundLayer[i].Layer.drawMode = SpriteDrawMode.Tiled;
                    Vector2 SizeSprite = QSprite.GetSpriteSizeUnit(m_backgroundLayer[i].Layer.sprite);
                    m_backgroundLayer[i].Layer.size = new Vector2(SizeSprite.x * 3, SizeSprite.y);
                }
                break;
        }
    }

    private void LateUpdate()
    {
        switch (m_cameraLoop)
        {
            case BackgroundLoopType.Horizontal:
                for (int i = 0; i < m_backgroundLayer.Count; i++)
                {
                    if (m_layerMask)
                    {
                        if (m_backgroundLayer[i].Transform.Equals(m_layerMaskCheck))
                            continue;
                    }

                    float Temp = GetCameraX(m_backgroundLayer[i]) * (1 - m_backgroundLayer[i].SpeedX);
                    float Distance = GetCameraX(m_backgroundLayer[i]) * m_backgroundLayer[i].SpeedX;

                    float PosX = m_backgroundLayer[i].PrimaryPosX + Distance + m_cameraBasePosX;
                    float PosY = GetCameraY(m_backgroundLayer[i]);
                    if (m_layerLimitY)
                    {
                        if (PosY + m_backgroundLayer[i].Size.y / 2 > m_layerMaskCheck.transform.position.y + LayerSize.y / 2 + m_layerOffset.y)
                            PosY = m_layerMaskCheck.transform.position.y + LayerSize.y / 2 + m_layerOffset.y - m_backgroundLayer[i].Size.y / 2;
                        else
                        if (PosY - m_backgroundLayer[i].Size.y / 2 < m_layerMaskCheck.transform.position.y - LayerSize.y / 2 + m_layerOffset.y)
                            PosY = m_layerMaskCheck.transform.position.y - LayerSize.y / 2 + m_layerOffset.y + m_backgroundLayer[i].Size.y / 2;
                    }
                    m_backgroundLayer[i].Transform.position = new Vector2(PosX, PosY);

                    if (Temp > m_backgroundLayer[i].PrimaryPosX + m_backgroundBoundX * m_backgroundLocalX)
                        m_backgroundLayer[i].PrimaryPosX += (m_backgroundBoundX * m_backgroundLocalX);
                    else
                    if (Temp < m_backgroundLayer[i].PrimaryPosX - m_backgroundBoundX * m_backgroundLocalX)
                        m_backgroundLayer[i].PrimaryPosX += (-m_backgroundBoundX * m_backgroundLocalX);
                }
                break;
        }
    }

    private float GetCameraX(BackgroundLoopLayer Layer)
    {
        switch (m_cameraLoop)
        {
            case BackgroundLoopType.Horizontal:
                return m_camera.transform.position.x - m_cameraBasePosX;
        }

        return 0;
    }

    private float GetCameraY(BackgroundLoopLayer Layer)
    {
        switch (m_cameraLoop)
        {
            case BackgroundLoopType.Horizontal:
                if (Layer.FollowY)
                    return m_camera.transform.position.y + Layer.PrimaryPosY;
                return Layer.Transform.position.y;
        }
        return 0;
    }

    private void OnDrawGizmos()
    {
        if (m_layerMask)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 Pos = (Vector2)this.transform.position + LayerOffset;
                Vector2 Border = QSprite.GetSpriteSizeUnit(m_background.sprite) * LayerScale + Vector2.one * (-0.1f) * i;
                QGizmos.SetWireCube(Pos, Border, Color.cyan);
            }

            //float Y = m_layerMaskCheck.transform.position.y + LayerSize.y / 2 + m_layerOffset.y;
            //QGizmos.SetWireSphere(new Vector3(m_layerMaskCheck.transform.position.x, Y), 1f, Color.red);
            //???
        }
    }
}

[System.Serializable]
public class BackgroundLoopLayer
{
    public SpriteRenderer Layer;
    public Transform Transform => Layer.transform;

    [Range(0, 1)] 
    public float SpeedX = 0.5f;

    public bool FollowY = true;

    [HideInInspector]
    public float PrimaryPosX;
    [HideInInspector]
    public float PrimaryPosY;

    public Vector2 Size => QSprite.GetSpriteSizeUnit(Layer.sprite);

    public BackgroundLoopLayer(SpriteRenderer SpriteRenderer, float SpeedX = 0.5f, bool FollowY = true)
    {
        Layer = SpriteRenderer;
        this.SpeedX = SpeedX;
        this.FollowY = FollowY;
    }
}

public enum BackgroundLoopType { Horizontal, /*Vertical*/ }