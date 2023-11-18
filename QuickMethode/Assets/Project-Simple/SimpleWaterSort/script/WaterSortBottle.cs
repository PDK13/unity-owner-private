using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using System;
using static UnityEngine.Rendering.DebugUI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaterSortBottle : MonoBehaviour
{
    //Varible: Color

    public const int COLOR_COUNT_MAX = 4;

    [SerializeField]
    private List<Color> m_color = new List<Color>()
    {
        Color.green,
        Color.blue,
        Color.yellow,
        Color.red,
    };
    private int m_colorCount;
    private int m_colorTopCount;
    private Color m_colorTop;

    private int m_colorTopOutCount;
    private Color m_colorTopOut;

    public List<Color> ColorList => m_color;

    /// <summary>
    /// Color count data is update when complete bottle active
    /// </summary>
    public int ColorCount => m_colorCount;

    /// <summary>
    /// Color top count data is update when complete bottle active
    /// </summary>
    public int ColorTopCount => m_colorTopCount;

    /// <summary>
    /// Color top data is update when complete bottle active
    /// </summary>
    public Color ColorTop => m_colorTop;

    public bool Finish
    {
        get
        {
            if (Emty)
                //None color in bottle, so complete!
                return true;
            //
            if (m_colorCount < COLOR_COUNT_MAX)
                //Not full color, so not finish!
                return false;
            //
            for (int i = 0; i < m_color.Count - 1; i++)
            {
                if (m_color[i].ToString() != m_color[i + 1].ToString())
                    //Not all color same with each other, so not complete!
                    return false;
            }
            //
            //All color same with each other, so finish!
            return true;
        }
    }

    public bool Emty => m_colorCount == 0;

    //Varible: Image & Material

    [Space]
    [SerializeField] private SpriteRenderer m_spriteColorMask;
    [SerializeField] private float m_valueInitScale = 1f;

    private Material MaskMaterial => m_spriteColorMask.material;

    private string m_nodeValuePosY = "_ValuePosY";
    private string m_nodeValueAdd = "_ValueAdd";
    private string m_nodeValueScale = "_ValueScale";
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
    private Transform m_rotatePointActive;

    public Transform RotatePointActive => m_rotatePointActive;

    public enum RotateDirType { None = 0, Left = -1, Right = 1, }

    private RotateDirType m_rotateDir;
    private float m_rotateAngle;
    private float m_rotateAngleLast;

    public RotateDirType RotateDir => m_rotateDir;

    //Varible: Config

    [SerializeField] private WaterSortBottleConfig m_bottleConfig; //If not null, value below will be replace!

    public bool BottleConfigAvaible => m_bottleConfig != null;

    [Space]
    [SerializeField] private float m_rotateDuration = 2f;
    private float m_rotateDurationCurrent;
    private float m_rotateDurationLerp;

    [SerializeField]
    private List<float> m_rotateLimit = new List<float>()
    {
        90f,
        80f,
        60f,
        40f,
    };
    private float m_rotateLimitCurrent;

    [SerializeField]
    private AnimationCurve m_rotateValueAdd = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(90, 3.5f));

    [Tooltip("Should create 4 point follow list rotate limit")]
    [SerializeField]
    private AnimationCurve m_rotateValueOut = new AnimationCurve(
        new Keyframe(0.00f, 0),
        new Keyframe(40.0f, 1),
        new Keyframe(60.0f, 2),
        new Keyframe(80.0f, 3),
        new Keyframe(90.0f, 4));

    //Varible: Bottle

    [SerializeField] private KeyCode m_bottleTargetDebug = KeyCode.None;
    [SerializeField] private WaterSortBottle m_bottleTarget;

    private bool m_bottleActive;
    private bool m_bottleFill;
    private bool m_bottleWait; //Delay at first rotate until continue rotate called!
    private bool m_bottleLock;

    /// <summary>
    /// Bottle is current active
    /// </summary>
    public bool BottleActive => !BottleLock && m_bottleActive;

    /// <summary>
    /// Bottle is current active and fill
    /// </summary>
    public bool BottleFill => !BottleLock && m_bottleActive && m_bottleFill;

    /// <summary>
    /// Bottle is current active and wait
    /// </summary>
    public bool BottleWait => !BottleLock && m_bottleActive && m_bottleWait;

    public bool BottleLock { get => m_bottleLock; set => m_bottleLock = value; }

    /// <summary>
    /// Bottle is avaible for active
    /// </summary>
    public bool BottleAvaible => !BottleLock && !m_bottleActive;

    //Event

    public Action<WaterSortBottle, bool> onBottleActive;
    public Action<WaterSortBottle, bool> onBottleFill;

    private void Start()
    {
        if (m_color.Count > COLOR_COUNT_MAX)
            m_color.RemoveRange(COLOR_COUNT_MAX, m_color.Count - COLOR_COUNT_MAX);
        //
        SetBottleColorDataUpdate();
        SetBottleColorValueUpdate();
        //
        ValueOut = COLOR_COUNT_MAX - m_colorCount;
        ValueAdd = 0;
        //
        if (BottleConfigAvaible)
        {
            m_rotateDuration = m_bottleConfig.RotateDuration;
            m_rotateLimit = m_bottleConfig.RotateLimit;
            m_rotateValueAdd = m_bottleConfig.RotateValueAdd;
            m_rotateValueOut = m_bottleConfig.RotateValueOut;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(m_bottleTargetDebug))
        {
            SetFillOutContinue();
            SetFillOut(m_bottleTarget);
        }
    }

#endif

    //

    private void SetBottleColorDataUpdate()
    {
        m_colorCount = m_color.Count;
        //
        m_colorTop = m_colorCount == 0 ? Color.clear : m_color[m_colorCount - 1];
        m_colorTopCount = 0;
        for (int i = m_colorCount - 1; i >= 0; i--)
        {
            if (m_color[i].ToString() == m_colorTop.ToString())
                m_colorTopCount++;
            else
                break;
        }
    }

    private void SetBottleColorValueUpdate()
    {
        for (int i = 0; i < m_colorCount; i++)
            ValueColor = (i, m_color[i]);
    }

    //Fill-Out

    public bool SetFillOut(WaterSortBottle BottleTarget, bool Wait = false)
    {
        if (m_bottleLock)
            return false;
        //
        if (m_bottleActive)
            return false;
        //
        if (BottleTarget == null)
            return false;
        //
        if (m_colorCount == 0)
            return false;
        //
        m_colorTopOutCount = BottleTarget.GetColorFillInAvaible(m_colorTop, m_colorTopCount);
        if (m_colorTopOutCount == 0)
            return false;
        m_colorTopOut = m_colorTop;
        //
        m_bottleTarget = BottleTarget;
        m_rotateDir = this.transform.position.x > m_bottleTarget.transform.position.x ? RotateDirType.Left : RotateDirType.Right;
        m_rotatePointActive = m_rotateDir == RotateDirType.Left ? m_rotatePointL : m_rotatePointR;
        m_bottleTarget.m_rotatePointActive = m_rotateDir == RotateDirType.Left ? m_rotatePointR : m_rotatePointL;
        //
        m_bottleWait = Wait;
        //
        StartCoroutine(ISetRotate());
        //
        return true;
    }

    private IEnumerator ISetRotate()
    {
        this.m_bottleActive = true;
        m_bottleTarget.m_bottleActive = true;
        this.onBottleActive?.Invoke(this, true);
        m_bottleTarget.onBottleActive?.Invoke(m_bottleTarget, true);
        //
        m_rotateDurationCurrent = 0;
        m_rotateDurationLerp = 0;
        m_rotateLimitCurrent = m_rotateLimit[m_colorCount - m_colorTopOutCount] * (int)m_rotateDir;
        m_rotateAngle = 0;
        m_rotateAngleLast = 0;
        //
        this.SetColorFillOut();
        m_bottleTarget.SetColorFillIn(m_colorTopOut, m_colorTopOutCount);
        //
        while (m_rotateDurationCurrent < m_rotateDuration)
        {
            m_rotateDurationLerp = m_rotateDurationCurrent / m_rotateDuration;
            m_rotateAngle = Mathf.Lerp(0.00f, m_rotateLimitCurrent, m_rotateDurationLerp);
            this.transform.RotateAround(m_rotatePointActive.position, Vector3.forward, m_rotateAngleLast - m_rotateAngle);
            //
            if (m_rotateAngle > m_rotateLimit[COLOR_COUNT_MAX - 1])
                yield return new WaitUntil(() => !m_bottleWait);
            //
            ValueAdd = m_rotateValueAdd.Evaluate(Mathf.Abs(m_rotateAngle));
            if (ValueOut < m_rotateValueOut.Evaluate(Mathf.Abs(m_rotateAngle)))
            {
                if (!m_bottleFill)
                {
                    m_bottleFill = true;
                    this.onBottleFill?.Invoke(this, true);
                    m_bottleTarget.onBottleFill?.Invoke(m_bottleTarget, true);
                }
                //
                ValueOut = m_rotateValueOut.Evaluate(Mathf.Abs(m_rotateAngle));
                m_bottleTarget.ValueOut += (m_rotateValueOut.Evaluate(Mathf.Abs(m_rotateAngleLast)) - m_rotateValueOut.Evaluate(Mathf.Abs(m_rotateAngle)));
            }
            //
            m_rotateAngleLast = m_rotateAngle;
            //
            m_rotateDurationCurrent += Time.deltaTime;
            //
            yield return new WaitForEndOfFrame();
        }
        //
        m_rotateAngle = m_rotateLimitCurrent;
        this.transform.RotateAround(m_rotatePointActive.position, Vector3.forward, m_rotateAngleLast - m_rotateAngle);
        //
        yield return ISetRotateBack();
        //
        this.SetBottleColorDataUpdate();
        m_bottleTarget.SetBottleColorDataUpdate();
        //
        this.m_rotateDir = RotateDirType.None;
        m_bottleTarget.m_rotateDir = RotateDirType.None;
        //
        this.m_bottleActive = false;
        m_bottleTarget.m_bottleActive = false;
        this.onBottleActive?.Invoke(this, false);
        m_bottleTarget.onBottleActive?.Invoke(m_bottleTarget, false);
        //
        m_bottleTarget = null;
        m_rotatePointActive = null;
    }

    private IEnumerator ISetRotateBack()
    {
        m_bottleFill = false;
        this.onBottleFill?.Invoke(this, false);
        m_bottleTarget.onBottleFill?.Invoke(m_bottleTarget, false);
        //
        m_rotateDurationCurrent = 0;
        m_rotateDurationLerp = 0;
        m_rotateAngle = 0;
        m_rotateAngleLast = m_rotateLimitCurrent;
        //
        while (m_rotateDurationCurrent < m_rotateDuration)
        {
            m_rotateDurationLerp = m_rotateDurationCurrent / m_rotateDuration;
            m_rotateAngle = Mathf.Lerp(m_rotateLimitCurrent, 0.00f, m_rotateDurationLerp);
            this.transform.RotateAround(m_rotatePointActive.position, Vector3.forward, m_rotateAngleLast - m_rotateAngle);
            //
            ValueAdd = m_rotateValueAdd.Evaluate(Mathf.Abs(m_rotateAngle));
            //
            m_rotateAngleLast = m_rotateAngle;
            //
            m_rotateDurationCurrent += Time.deltaTime;
            //
            yield return new WaitForEndOfFrame();
        }
        //
        m_rotateAngle = 0;
        this.transform.RotateAround(m_rotatePointActive.position, Vector3.forward, m_rotateAngleLast - m_rotateAngle);
    }

    private void SetColorFillOut()
    {
        for (int i = m_colorCount - 1; i >= m_colorCount - m_colorTopOutCount; i--)
            m_color.RemoveAt(m_color.Count - 1);
    }

    public void SetFillOutContinue()
    {
        m_bottleWait = false;
    }

    //Fill-In

    private int GetColorFillInAvaible(Color ColorOut, int ColorOutCount)
    {
        if (m_colorCount == 0)
            return ColorOutCount;
        //
        if (m_colorTop.ToString() != ColorOut.ToString())
            //None color will be consume by this bottle!
            return 0;
        //
        if (m_colorCount + ColorOutCount > COLOR_COUNT_MAX)
            return COLOR_COUNT_MAX - m_colorCount;
        //
        return ColorOutCount;
    }

    private void SetColorFillIn(Color ColorOut, int ColorOutCount)
    {
        for (int i = 0; i < ColorOutCount; i++)
        {
            m_color.Add(ColorOut);
            ValueColor = (m_color.Count - 1, ColorOut);
        }
    }

    //Editor

    public void SetEditorColorAddTop()
    {
        m_color.Add(Color.clear);
    }

    public void SetEditorColorRemoveTop()
    {
        m_color.RemoveAt(m_color.Count - 1);
    }
}

#if UNITY_EDITOR

[CanEditMultipleObjects]
[CustomEditor(typeof(WaterSortBottle))]
public class WaterSortBottleEditor : Editor
{
    private WaterSortBottle m_target;

    private SerializedProperty m_spriteColorMask;
    private SerializedProperty m_valueInitScale;
    private SerializedProperty m_rotatePointL;
    private SerializedProperty m_rotatePointR;

    private SerializedProperty m_bottleConfig;

    private SerializedProperty m_rotateDuration;
    private SerializedProperty m_rotateLimit;
    private SerializedProperty m_rotateValueAdd;
    private SerializedProperty m_rotateValueOut;

    private SerializedProperty m_bottleTargetDebug;
    private SerializedProperty m_bottleTarget;

    private int m_colorCount;
    private bool m_showReferenceSetting;
    private bool m_showConfigSetting;

    private void OnEnable()
    {
        m_target = target as WaterSortBottle;
        //
        m_spriteColorMask = serializedObject.FindProperty("m_spriteColorMask");
        m_valueInitScale = serializedObject.FindProperty("m_valueInitScale");
        m_rotatePointL = serializedObject.FindProperty("m_rotatePointL");
        m_rotatePointR = serializedObject.FindProperty("m_rotatePointR");

        m_bottleConfig = serializedObject.FindProperty("m_bottleConfig");

        m_rotateDuration = serializedObject.FindProperty("m_rotateDuration");
        m_rotateLimit = serializedObject.FindProperty("m_rotateLimit");
        m_rotateValueAdd = serializedObject.FindProperty("m_rotateValueAdd");
        m_rotateValueOut = serializedObject.FindProperty("m_rotateValueOut");

        m_bottleTargetDebug = serializedObject.FindProperty("m_bottleTargetDebug");
        m_bottleTarget = serializedObject.FindProperty("m_bottleTarget");
        //
        if (m_target.ColorList.Count > WaterSortBottle.COLOR_COUNT_MAX)
            m_target.ColorList.RemoveRange(WaterSortBottle.COLOR_COUNT_MAX, m_target.ColorList.Count - WaterSortBottle.COLOR_COUNT_MAX);
        //
        m_colorCount = m_target.ColorList.Count;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //
        GUILayout.Label("COLOR", GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Top"))
            if (m_colorCount < WaterSortBottle.COLOR_COUNT_MAX)
                m_colorCount++;
        if (GUILayout.Button("Remove Top"))
            if (m_colorCount > 0)
                m_colorCount--;
        GUILayout.EndHorizontal();
        //
        while (m_colorCount > m_target.ColorList.Count)
            m_target.SetEditorColorAddTop();
        while (m_colorCount < m_target.ColorList.Count)
            m_target.SetEditorColorRemoveTop();
        //
        if (m_target.ColorList.Count == 0)
            GUILayout.Label("Bottle is current emty with no color", GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter));
        else
        {
            GUILayout.BeginVertical();
            for (int i = m_target.ColorList.Count - 1; i >= 0; i--)
                m_target.ColorList[i] = EditorGUILayout.ColorField(m_target.ColorList[i]);
            GUILayout.EndVertical();
        }
        //
        GUILayout.Space(10f);
        GUILayout.Label("REFERENCE", GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        if (m_showReferenceSetting)
        {
            if (GUILayout.Button("Hide reference"))
                m_showReferenceSetting = !m_showReferenceSetting;
            EditorGUILayout.PropertyField(m_spriteColorMask);
            EditorGUILayout.PropertyField(m_valueInitScale);
            EditorGUILayout.PropertyField(m_rotatePointL);
            EditorGUILayout.PropertyField(m_rotatePointR);
        }
        else
        {
            if (GUILayout.Button("Show reference"))
                m_showReferenceSetting = !m_showReferenceSetting;
        }
        //
        GUILayout.Space(10f);
        GUILayout.Label("CONFIG", GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        EditorGUILayout.PropertyField(m_bottleConfig);
        //
        if (!m_target.BottleConfigAvaible)
        {
            GUILayout.Space(10f);
            //
            if (m_showConfigSetting)
            {
                if (GUILayout.Button("Hide config"))
                    m_showConfigSetting = !m_showConfigSetting;
                EditorGUILayout.PropertyField(m_rotateDuration);
                EditorGUILayout.PropertyField(m_rotateLimit);
                EditorGUILayout.PropertyField(m_rotateValueAdd);
                EditorGUILayout.PropertyField(m_rotateValueOut);
            }
            else
            {
                if (GUILayout.Button("Show config"))
                    m_showConfigSetting = !m_showConfigSetting;
            }
        }
        else
            GUILayout.Label("Bottle config is now follow scriptable object", GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter));
        //
        GUILayout.Space(10f);
        GUILayout.Label("TARGET", GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        EditorGUILayout.PropertyField(m_bottleTargetDebug);
        EditorGUILayout.PropertyField(m_bottleTarget);
        //
        serializedObject.ApplyModifiedProperties();
        //
        EditorUtility.SetDirty(m_target);
    }

    public GUIStyle GetGUILabel(FontStyle FontStyle, TextAnchor Alignment)
    {
        GUIStyle GUIStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle,
            alignment = Alignment,
        };
        return GUIStyle;
    }
}

#endif