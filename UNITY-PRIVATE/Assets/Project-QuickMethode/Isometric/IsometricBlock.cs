using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class IsometricBlock : MonoBehaviour
{
    #region Varible: Block Manager

    [SerializeField] private string m_name = "";
    [SerializeField] private List<string> m_tag = new List<string>();

    #endregion

    #region Varible: World Manager

    [Space]
    [SerializeField] private IsometricPosType m_posType = IsometricPosType.Track;

    //After generate map, this pos will be use to renderer in scene!
    [SerializeField] private IsometricVector m_pos = new IsometricVector();

    //While generate map, this pos will be use to create this block in scene!
    [SerializeField] private IsometricVector m_posPrimary = new IsometricVector();

    #endregion

    #region Varible: Scene Manager

    [Space]
    [SerializeField] private IsometricGameDataScene m_sceneData = new IsometricGameDataScene();
    [SerializeField] private Vector3 m_centre = new Vector3();

    #endregion

    private IsometricManager m_worldManager;

    #region ================================================================== Mono

    private void Awake()
    {
        if (Application.isPlaying && this.name == IsometricManagerMap.NAME_CURSON)
            Destroy(this.gameObject);
    }

#if UNITY_EDITOR

    private void Update()
    {
        SetIsoTransform();
    }

#endif

    #endregion

    #region ================================================================== Block Manager

    public string Name => m_name != "" ? m_name : QGameObject.GetNameReplaceClone(name);

    public List<string> Tag => m_tag;

    public IsometricVector Pos { get => m_pos; set { m_pos = value; SetIsoTransform(); } }

    public IsometricPosType PosType => m_posType;

    public IsometricVector PosPrimary { get => m_posPrimary; set => m_posPrimary = value; }

    #endregion

    #region ================================================================== World Manager

    public IsometricManager WorldManager
    {
        get => m_worldManager;
        set
        {
            m_worldManager = value;
            m_sceneData = value.Scene;
        }
    }

    #endregion

    #region ================================================================== Scene Manager

    private Vector3 GetIsoTransform(IsometricVector Pos)
    {
        IsometricVector PosCentre = m_sceneData.Centre;
        float Angle = 0;
        //
        switch (m_sceneData.Rotate)
        {
            case IsometricRotateType._0:
                Angle = 0 * Mathf.Deg2Rad;
                break;
            case IsometricRotateType._90:
                Angle = 90f * Mathf.Deg2Rad;
                break;
            case IsometricRotateType._180:
                Angle = 180f * Mathf.Deg2Rad;
                break;
            case IsometricRotateType._270:
                Angle = 270f * Mathf.Deg2Rad;
                break;
        }
        IsometricVector PosValue = new IsometricVector(Pos)
        {
            X = (Pos.X - PosCentre.X) * Mathf.Cos(Angle) - (Pos.Y - PosCentre.Y) * Mathf.Sin(Angle) + PosCentre.X,
            Y = (Pos.X - PosCentre.X) * Mathf.Sin(Angle) + (Pos.Y - PosCentre.Y) * Mathf.Cos(Angle) + PosCentre.Y
        };
        //
        Vector3 PosTransform = new Vector3();
        IsometricVector PosValueScale = PosValue;
        //
        //
        switch (m_sceneData.Renderer)
        {
            case IsometricRendererType.H:
                PosValueScale.X *= m_sceneData.Scale.X * 0.5f * -1;
                PosValueScale.Y *= m_sceneData.Scale.Y * 0.5f;
                PosValueScale.H *= m_sceneData.Scale.H * 0.5f;
                //
                PosTransform.x = PosValueScale.X + PosValueScale.Y;
                PosTransform.y = 0.5f * (PosValueScale.Y - PosValueScale.X) + PosValueScale.H;
                PosTransform.z = PosValue.X + PosValue.Y - PosValue.H;
                //
                break;
            case IsometricRendererType.XY:
                PosValueScale.X *= m_sceneData.Scale.X * 0.5f * -1;
                PosValueScale.Y *= m_sceneData.Scale.Y * 0.5f;
                PosValueScale.H *= m_sceneData.Scale.H * 0.5f;
                //
                PosTransform.x = PosValueScale.X + PosValueScale.Y;
                PosTransform.y = 0.5f * (PosValueScale.Y - PosValueScale.X) + PosValueScale.H;
                PosTransform.z = (PosValue.Y + PosValue.X) - PosValue.H * 2;
                //
                break;
            case IsometricRendererType.None: //Testing
                PosValueScale.X *= m_sceneData.Scale.X * 0.5f * -1;
                PosValueScale.Y *= m_sceneData.Scale.Y * 0.5f;
                PosValueScale.H *= m_sceneData.Scale.H * 0.5f;
                //
                PosTransform.x = PosValueScale.X + PosValueScale.Y;
                PosTransform.y = 0.5f * (PosValueScale.Y - PosValueScale.X) + PosValueScale.H;
                PosTransform.z = 0;
                //
                break;
        }
        //
        return PosTransform;
    }

    private void SetIsoTransform()
    {
        if (WorldManager != null)
        {
            m_sceneData = WorldManager.Scene;
        }

        Vector3 PosTransform = GetIsoTransform(m_pos);

        PosTransform += m_centre;

        transform.position = PosTransform;
    }

    #endregion

    #region ================================================================== World Check

    public List<IsometricBlock> GetCheck(IsometricVector Dir, int Length)
    {
        return WorldManager.World.Current.GetBlockCurrentAll(Pos.Fixed + Dir * Length);
    }

    public List<IsometricBlock> GetCheck(IsometricVector Dir, int Length, params string[] TagFind)
    {
        return WorldManager.World.Current.GetBlockCurrentAll(Pos.Fixed + Dir * Length, TagFind);
    }

    #endregion

    #region ================================================================== World Editor

    public void SetSpriteAlpha(float Alpha)
    {
        QColor.SetSprite(GetComponent<SpriteRenderer>(), Alpha);
    }

    public void SetSpriteColor(Color Color, float Alpha = 1)
    {
        GetComponent<SpriteRenderer>().color = Color;
        SetSpriteAlpha(Alpha);
    }

    #endregion
}