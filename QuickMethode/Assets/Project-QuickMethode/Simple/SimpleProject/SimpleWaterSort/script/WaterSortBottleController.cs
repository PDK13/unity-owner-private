using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaterSortBottleController : MonoBehaviour
{
    //Material used with URP, should set camera 'Background Type (Clear Flag)' to "Solid Color' to avoid ghosting-image!

    private const int COLOR_MAX = 4;
    //
    private const string NODE_COLOR_FILL_AMOUNT = "_ColorFillAmount";
    private const string NODE_COLOR_SCALE_AND_ROTATE = "_ColorScaleAndRotate";
    private const string NODE_OBJECT_POSITION = "_ObjectPosition";
    private const string NODE_OBJECT_SCALE = "_ObjectScale";
    //
    [SerializeField] private List<Color> m_bottleColor = new List<Color>();
    //
    private Color BottleColorTop => m_bottleColor.Count > 0 ? m_bottleColor[m_bottleColor.Count - 1] : Color.clear;

    private int BottleColorTopCount
    {
        get
        {
            int Count = 0;
            for (int i = m_bottleColor.Count - 1; i >= 0; i--)
            {
                if (m_bottleColor[i].ToString() != BottleColorTop.ToString())
                    return Count;
                else
                    Count++;
            }
            return Count;
        }
    }
    //
    [Space]
    [SerializeField] private WaterSortBottleConfig m_bottleConfig;

    public WaterSortBottleConfig BottleConfig { get => m_bottleConfig; set => m_bottleConfig = value; }
    //
    private float m_timeRotateCurrent;
    private float m_timeRotateLerp;
    //
    private float m_rotationLimit;
    private int m_rotationDir;
    //
    private bool m_rotateActive = false;
    //
    [Space]
    [Tooltip("Mask color from Image component")]
    [SerializeField] private Image m_bottleMaskImage;
    [Tooltip("Mask color from Sprite Renderer component")]
    [SerializeField] private SpriteRenderer m_bottleMaskSprite;
    //
    [SerializeField] private float m_bottleMaskScaleStart = 1f; //Scale of renderer mask material! 
    //
    private float ValueFillAmount
    {
        set
        {
            if (m_bottleMaskImage != null)
                m_bottleMaskImage.material.SetFloat(NODE_COLOR_FILL_AMOUNT, value);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                Material.SetFloat(NODE_COLOR_FILL_AMOUNT, value);
            }
        }
        get
        {
            if (m_bottleMaskImage != null)
                return m_bottleMaskImage.material.GetFloat(NODE_COLOR_FILL_AMOUNT);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                return Material.GetFloat(NODE_COLOR_FILL_AMOUNT);
            }
            return 0;
        }
    }

    private float ValueScaleAndRotate
    {
        set
        {
            if (m_bottleMaskImage != null)
                m_bottleMaskImage.material.SetFloat(NODE_COLOR_SCALE_AND_ROTATE, value);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                Material.SetFloat(NODE_COLOR_SCALE_AND_ROTATE, value);
            }
        }
        get
        {
            if (m_bottleMaskImage != null)
                return m_bottleMaskImage.material.GetFloat(NODE_COLOR_SCALE_AND_ROTATE);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                return Material.GetFloat(NODE_COLOR_SCALE_AND_ROTATE);
            }
            return 0;
        }
    }

    private (int Index, Color Color) ValueColor
    {
        set
        {
            if (m_bottleMaskImage != null)
                m_bottleMaskImage.material.SetColor(string.Format("_Color{0}", value.Index), value.Color);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                Material.SetColor(string.Format("_Color{0}", value.Index), value.Color);
            }
        }
    }

    private Vector2 ValueObjectPosition
    {
        set
        {
            if (m_bottleMaskImage != null)
                m_bottleMaskImage.material.SetVector(NODE_OBJECT_POSITION, value);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                Material.SetVector(NODE_OBJECT_POSITION, value);
            }
        }
        get
        {
            if (m_bottleMaskImage != null)
                return m_bottleMaskImage.material.GetVector(NODE_OBJECT_POSITION);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                return Material.GetVector(NODE_OBJECT_POSITION);
            }
            return Vector2.zero;
        }
    }

    private float ValueObjectScale
    {
        set
        {
            if (m_bottleMaskImage != null)
                m_bottleMaskImage.material.SetFloat(NODE_OBJECT_SCALE, value);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                Material.SetFloat(NODE_OBJECT_SCALE, value);
            }
        }
        get
        {
            if (m_bottleMaskImage != null)
                return m_bottleMaskImage.material.GetFloat(NODE_OBJECT_SCALE);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                return Material.GetFloat(NODE_OBJECT_SCALE);
            }
            return 1f;
        }
    }

    //

    public Action<WaterSortBottleController, bool> onRotateActive;

    //

    [Space]
    [Tooltip("Press Enter to start fill color from this to target")]
    [SerializeField] private WaterSortBottleController m_debugBottleColorIn;

    private void Awake()
    {
        if (m_bottleMaskImage != null)
        {
            m_bottleMaskImage.enabled = true;
            m_bottleMaskImage.material = new Material(m_bottleMaskImage.material);
        }
        else
        if (m_bottleMaskSprite != null)
        {
            m_bottleMaskSprite.enabled = true;
        }
    }

    private IEnumerator Start()
    {
        yield return null; //Wait for object done init transform before init material position and scale!
        //
        ValueFillAmount = m_bottleConfig.LimitFillAmount[m_bottleColor.Count];
        ValueObjectPosition = this.transform.position;
        ValueObjectScale = m_bottleMaskScaleStart;
        //
        SetUpdateColorStart();
    }

    private void SetUpdateColorStart()
    {
        for (int i = 0; i < m_bottleColor.Count; i++)
            ValueColor = (i, m_bottleColor[i]);
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (m_debugBottleColorIn == null)
            return;
        //
        if (Input.GetKeyDown(KeyCode.Space))
            SetColorOutActive(m_debugBottleColorIn);
    }

#endif

    #region Update

    public void SetPositionUpdate()
    {
        ValueObjectPosition = this.transform.position;
    }

    public void SetFillAmountUpdate()
    {
        ValueFillAmount = m_bottleConfig.LimitFillAmount[m_bottleColor.Count];
    }

    #endregion

    #region Out

    public bool GetColorOutCheck(WaterSortBottleController BottleFillIn)
    {
        if (m_rotateActive)
            return false;
        //
        if (m_bottleColor.Count == 0)
            return false;
        //
        if (BottleFillIn == null)
            return false;
        //
        if (BottleFillIn.Equals(this))
            return false;
        //
        int BottleColorTopCountUsed = BottleFillIn.GetColorInOffset(BottleColorTop, BottleColorTopCount);
        if (BottleColorTopCountUsed == 0)
            //If offset between 2 bottle isn't more than 0, they can't fill up or down with each other!
            return false;
        //
        return true;
    }

    public bool SetColorOutActive(WaterSortBottleController BottleFillIn)
    {
        if (m_rotateActive)
            return false;
        //
        if (m_bottleColor.Count == 0)
            return false;
        //
        if (BottleFillIn == null)
            return false;
        //
        if (BottleFillIn.Equals(this))
            return false;
        //
        int BottleColorTopCountUsed = BottleFillIn.GetColorInOffset(BottleColorTop, BottleColorTopCount);
        if (BottleColorTopCountUsed == 0)
            //If offset between 2 bottle isn't more than 0, they can't fill up or down with each other!
            return false;
        //
        StartCoroutine(ISetColorOutRotate(BottleFillIn, BottleColorTopCountUsed));
        //
        return true;
    }

    private IEnumerator ISetColorOutRotate(WaterSortBottleController BottleFillIn, int BottleColorTopCountUsed)
    {
        m_rotateActive = true;
        onRotateActive?.Invoke(this, true);
        //
        m_rotationDir = this.transform.position.x > BottleFillIn.transform.position.x ? 1 : -1; //Rotate Dir!
        //
        int BottleColorCountSaved = m_bottleColor.Count;
        //
        Color BottleColorTopUsed = BottleColorTop;
        for (int i = 0; i < BottleColorTopCountUsed; i++)
            m_bottleColor.RemoveAt(m_bottleColor.Count - 1);
        //
        m_timeRotateCurrent = 0; //Time in curve to get value at the time of lerp value!
        m_timeRotateLerp = 0; //Time in ratio of Time Rotate from 0..1 value!
        float AngleValue = 0;
        float AngleValueLast = 0;
        m_rotationLimit = m_bottleConfig.LimitRotation[m_bottleColor.Count];
        //
        BottleFillIn.SetColorIn(BottleColorTopUsed, BottleColorTopCountUsed); //Fill in another bottle!!
        //
        while (m_timeRotateCurrent < m_bottleConfig.DurationRotate)
        {
            m_timeRotateLerp = m_timeRotateCurrent / m_bottleConfig.DurationRotate;
            AngleValueLast = AngleValue;
            AngleValue = Mathf.Lerp(0.0f, m_rotationLimit, m_timeRotateLerp);
            //
            //Rotate
            this.transform.eulerAngles = Vector3.forward * AngleValue * m_rotationDir;
            //Rotate
            //
            if (m_bottleConfig.LimitFillAmount[BottleColorCountSaved] > m_bottleConfig.CurveFillAmount.Evaluate(AngleValue))
            {
                ValueFillAmount = m_bottleConfig.CurveFillAmount.Evaluate(AngleValue);
                ValueObjectPosition = this.transform.position;
                BottleFillIn.SetColorInFillAmount(m_bottleConfig.CurveFillAmount.Evaluate(AngleValueLast) - m_bottleConfig.CurveFillAmount.Evaluate(AngleValue));
            }
            ValueScaleAndRotate = m_bottleConfig.CurveScaleAndRotation.Evaluate(AngleValue);
            //
            m_timeRotateCurrent += Time.deltaTime * m_bottleConfig.CurveRotationSpeed.Evaluate(AngleValue);
            //
            yield return new WaitForEndOfFrame();
        }
        //
        AngleValue = m_rotationLimit;
        //
        //Rotate
        this.transform.eulerAngles = Vector3.forward * AngleValue * m_rotationDir;
        //Rotate
        //
        ValueFillAmount = m_bottleConfig.CurveFillAmount.Evaluate(AngleValue);
        ValueObjectPosition = this.transform.position;
        ValueScaleAndRotate = m_bottleConfig.CurveScaleAndRotation.Evaluate(AngleValue);
        //
        yield return ISetColorOutRotateBack();
        //
        m_rotateActive = false;
        onRotateActive?.Invoke(this, false);
    }

    private IEnumerator ISetColorOutRotateBack()
    {
        m_timeRotateCurrent = 0; //Time in curve to get value at the time!
        m_timeRotateLerp = 0;
        float AngleValue = 0;
        float AngleValueLast = m_rotationDir * m_bottleConfig.LimitRotation[m_bottleColor.Count];
        //
        while (m_timeRotateCurrent < m_bottleConfig.DurationRotate)
        {
            m_timeRotateLerp = m_timeRotateCurrent / m_bottleConfig.DurationRotate;
            AngleValueLast = AngleValue;
            AngleValue = Mathf.Lerp(m_rotationLimit, 0.00f, m_timeRotateLerp);
            //
            //Rotate
            this.transform.eulerAngles = Vector3.forward * AngleValue * m_rotationDir;
            //Rotate
            //
            ValueScaleAndRotate = m_bottleConfig.CurveScaleAndRotation.Evaluate(AngleValue);
            //
            m_timeRotateCurrent += Time.deltaTime;
            //
            yield return new WaitForEndOfFrame();
        }
        //
        AngleValue = 0f;
        //Rotate
        this.transform.eulerAngles = Vector3.forward * AngleValue * m_rotationDir;
        //Rotate
        ValueScaleAndRotate = m_bottleConfig.CurveScaleAndRotation.Evaluate(AngleValue);
    }

    #endregion

    #region In

    private int GetColorInOffset(Color Color, int Count)
    {
        if (m_bottleColor.Count == 0)
        {
            if (Count > COLOR_MAX)
                return COLOR_MAX;
            else
                return Count;
        }
        //
        if (BottleColorTop.ToString() != Color.ToString())
            //If bottle color top of 2 not same with each other, can't fill each other!
            return 0;
        //
        if (m_bottleColor.Count + Count > COLOR_MAX)
            //If bottle color of top this will be fill more than max, just get some from another bottle!
            return COLOR_MAX - m_bottleColor.Count;
        //
        return Count;
    }

    private void SetColorIn(Color Color, int Count)
    {
        int IndexStart = m_bottleColor.Count;
        for (int i = 0; i < Count; i++)
        {
            m_bottleColor.Add(Color);
            ValueColor = (IndexStart + i, Color);
        }
    }

    private void SetColorInFillAmount(float CurveFillAmountValue)
    {
        if (ValueFillAmount + CurveFillAmountValue > m_bottleConfig.LimitFillAmount[m_bottleColor.Count])
        {
            ValueFillAmount = m_bottleConfig.LimitFillAmount[m_bottleColor.Count];
            return;
        }
        //
        ValueFillAmount = ValueFillAmount + CurveFillAmountValue;
        ValueObjectPosition = this.transform.position;
    }

    #endregion
}

#if UNITY_EDITOR

//[CustomEditor(typeof(WaterSortBottleController))]
public class BottleControllerEditor : Editor
{
}

#endif