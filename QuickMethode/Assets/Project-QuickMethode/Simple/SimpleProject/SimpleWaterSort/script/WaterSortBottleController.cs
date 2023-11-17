using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using System;
using static UnityEngine.Rendering.DebugUI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaterSortBottleController : MonoBehaviour
{
    private const int COLOR_COUNT_MAX = 4;

    [SerializeField] private List<Color> m_color = new List<Color>() { Color.green, Color.blue, Color.yellow, Color.red, };
    private int m_colorCount;
    private Color m_colorTop;

    private int m_colorTopOutCount;
    private Color m_colorTopOut;

    //Varible: Image & Material

    [Space]
    [SerializeField] private SpriteRenderer m_spriteColorMask;
    [SerializeField] private float m_valueInitScale = 1f;

    private Material MaskMaterial => m_spriteColorMask.material;

    private string m_nodeValuePosY = "_ValuePosY";
    private string m_nodeValueAdd = "_ValueAdd";
    private string m_nodeValueScale = "_ValueScale";
    private string m_nodeValueMax = "_ValueMax";
    private string m_nodeValueOut = "_ValueOut";
    private string m_nodeValueOutMuti = "_ValueOutMuti";
    private string m_nodeColor = "_Color{0}";

    //Should update position when bottle in move stage, not when bottle is in rotate stage.
    private float ValuePosY { get => MaskMaterial.GetFloat(m_nodeValuePosY); set => MaskMaterial.SetFloat(m_nodeValuePosY, value); }

    private float ValueAdd { get => MaskMaterial.GetFloat(m_nodeValueAdd); set => MaskMaterial.SetFloat(m_nodeValueAdd, value); }

    private float ValueScale { get => MaskMaterial.GetFloat(m_nodeValueScale); set => MaskMaterial.SetFloat(m_nodeValueScale, value); }

    private float ValueOut { get => MaskMaterial.GetFloat(m_nodeValueOut); set => MaskMaterial.SetFloat(m_nodeValueOut, value); }

    private float ValueOutMuti { get => MaskMaterial.GetFloat(m_nodeValueOutMuti); }

    private (int Index, Color Color) ValueColor
    {
        set
        {
            if (value.Index < 0 || value.Index > m_color.Count - 1)
                return;
            //
            MaskMaterial.SetColor(string.Format(m_nodeColor, value.Index), value.Color);
        }
    }

    //Varible: Rotate

    [Space]
    [SerializeField] private Transform m_rotatePointL;
    [SerializeField] private Transform m_rotatePointR;
    private Transform m_rotatePoint;

    public enum RotateDirType { None = 0, Left = -1, Right = 1, }

    private RotateDirType m_rotateDir;
    private float m_rotateAngle;
    private float m_rotateAngleLast;

    [Space]
    [SerializeField] private float m_rotateDuration = 2f;
    private float m_rotateDurationCurrent;
    private float m_rotateDurationLerp;

    [SerializeField] private List<float> m_rotateLimit = new List<float>() { 90f, 80f, 60f, 40f, };
    private float m_rotateLimitCurrent;

    [SerializeField] private AnimationCurve m_rotateValueAdd = new AnimationCurve(
        new Keyframe(0, 0), 
        new Keyframe(90, 3.5f));

    [Tooltip("Should create 4 point follow list rotate limit")]
    [SerializeField] private AnimationCurve m_rotateValueOut = new AnimationCurve(
        new Keyframe(0.00f, 0), 
        new Keyframe(40.0f, 1), 
        new Keyframe(60.0f, 2), 
        new Keyframe(80.0f, 3), 
        new Keyframe(90.0f, 4));

    private void Awake()
    {
        if (m_color.Count > COLOR_COUNT_MAX)
            m_color.RemoveRange(COLOR_COUNT_MAX, m_color.Count - COLOR_COUNT_MAX);
        //
        SetBottleColorDataUpdate();
        SetBottleColorValueUpdate();
    }

    private void Start()
    {
        ValueOut = 0;
        ValueAdd = 0;
    }

    private void Update()
    {
        //ValuePosY = this.transform.position.y;
        //
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.transform.position = Vector3.zero;
            this.transform.eulerAngles = Vector3.zero;
            ValueOut = 0;
            ValueAdd = 0;
        }
        else
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(ISetRotate(RotateDirType.Left));
        }
        else
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(ISetRotate(RotateDirType.Right));
        }
    }

    private void SetBottleColorDataUpdate()
    {
        m_colorCount = m_color.Count;
        m_colorTop = m_colorCount == 0 ? Color.clear : m_color[m_colorCount - 1];
    }

    private void SetBottleColorValueUpdate()
    {
        for (int i = 0; i < m_colorCount - 1; i++)
            ValueColor = (i, m_color[i]);
    }

    //

    private void SetFillOut()
    {

    }

    private IEnumerator ISetRotate(RotateDirType Dir)
    {
        if (m_colorCount == 0)
            yield break;
        //
        m_rotateDir = Dir;
        m_rotatePoint = Dir == RotateDirType.Left ? m_rotatePointL : m_rotatePointR;
        m_rotateDurationCurrent = 0;
        m_rotateDurationLerp = 0;
        m_rotateLimitCurrent = m_rotateLimit[m_colorCount - 1] * (int)m_rotateDir;
        m_rotateAngle = 0;
        m_rotateAngleLast = 0;
        //
        SetColorOutTop();
        //
        while (m_rotateDurationCurrent < m_rotateDuration)
        {
            m_rotateDurationLerp = m_rotateDurationCurrent / m_rotateDuration;
            m_rotateAngle = Mathf.Lerp(0.00f, m_rotateLimitCurrent, m_rotateDurationLerp);
            this.transform.RotateAround(m_rotatePoint.position, Vector3.forward, m_rotateAngleLast - m_rotateAngle);
            m_rotateAngleLast = m_rotateAngle;
            //
            ValueAdd = m_rotateValueAdd.Evaluate(Mathf.Abs(m_rotateAngle));
            if (ValueOut < m_rotateValueOut.Evaluate(Mathf.Abs(m_rotateAngle)))
                ValueOut = m_rotateValueOut.Evaluate(Mathf.Abs(m_rotateAngle));
            //
            m_rotateDurationCurrent += Time.deltaTime;
            //
            yield return new WaitForEndOfFrame();
        }
        //
        m_rotateAngle = m_rotateLimitCurrent;
        this.transform.RotateAround(m_rotatePoint.position, Vector3.forward, m_rotateAngleLast - m_rotateAngle);
        //
        yield return ISetRotateBack(Dir);
        //
        SetBottleColorDataUpdate();
    }

    private IEnumerator ISetRotateBack(RotateDirType Dir)
    {
        
        m_rotateDir = Dir;
        m_rotatePoint = Dir == RotateDirType.Left ? m_rotatePointL : m_rotatePointR;
        m_rotateDurationCurrent = 0;
        m_rotateDurationLerp = 0;
        m_rotateLimitCurrent = m_rotateLimit[m_colorCount - 1] * (int)m_rotateDir;
        m_rotateAngle = 0;
        m_rotateAngleLast = m_rotateLimitCurrent;
        //
        while (m_rotateDurationCurrent < m_rotateDuration)
        {
            m_rotateDurationLerp = m_rotateDurationCurrent / m_rotateDuration;
            m_rotateAngle = Mathf.Lerp(m_rotateLimitCurrent, 0.00f, m_rotateDurationLerp);
            this.transform.RotateAround(m_rotatePoint.position, Vector3.forward, m_rotateAngleLast - m_rotateAngle);
            m_rotateAngleLast = m_rotateAngle;
            //
            ValueAdd = m_rotateValueAdd.Evaluate(Mathf.Abs(m_rotateAngle));
            //ValueOut = m_rotateValueOut.Evaluate(Mathf.Abs(m_rotateAngle));
            //
            m_rotateDurationCurrent += Time.deltaTime;
            //
            yield return new WaitForEndOfFrame();
        }
        //
        m_rotateAngle = 0;
        this.transform.RotateAround(m_rotatePoint.position, Vector3.forward, m_rotateAngleLast - m_rotateAngle);
    }

    private void SetColorOutTop()
    {
        if (m_colorCount == 0)
            return;
        //
        m_colorTopOutCount = 0;
        m_colorTopOut = m_colorTop;
        for (int i = m_colorCount - 1; i >= 0; i--)
        {
            if (m_color[i].ToString() == m_colorTopOut.ToString())
            {
                m_color.RemoveAt(m_colorCount - 1);
                m_colorTopOutCount++;
            }
            else
                break;
        }
    }
}

#if UNITY_EDITOR

[CanEditMultipleObjects]
[CustomEditor(typeof(WaterSortBottleController))]
public class WaterSortBottleControllerEditor : Editor
{
    private WaterSortBottleController m_target;

    private void OnEnable()
    {
        m_target = target as WaterSortBottleController;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

#endif