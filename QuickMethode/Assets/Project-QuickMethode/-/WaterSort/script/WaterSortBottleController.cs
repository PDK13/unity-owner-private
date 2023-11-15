using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class WaterSortBottleController : MonoBehaviour
{
    //Material used with URP, should set camera 'Background Type (Clear Flag)' to "Solid Color' to avoid ghosting-image!

    private const int COLOR_MAX = 4;

    [SerializeField] private List<Color> m_bottleColor = new List<Color>();

    private Color BottleColorTop => m_bottleColor.Count > 0 ? m_bottleColor[m_bottleColor.Count - 1] : Color.clear;

    private int BottleColorTopCount
    {
        get
        {
            int Count = 0;
            for (int i = m_bottleColor.Count - 1; i >= 0; i--)
            {
                if (m_bottleColor[i].ToString() != m_bottleColorTop.ToString())
                    return Count;
                else
                    Count++;
            }
            return Count;
        }
    }

    private Color m_bottleColorTop;
    private int m_bottleColorTopCount;

    [Space]
    [SerializeField] private float m_timeRotate = 3.0f;
    private float m_timeRotateCurrent;
    private float m_timeRotateLerp;

    [Space]
    [SerializeField] private WaterSortBottleCurveData m_curveData;

    private int LimitRotationIndex
    {
        get
        {
            for (int i = m_bottleColor.Count - 1; i >= 0; i--)
                if (m_bottleColor[i].ToString() != m_bottleColorTop.ToString())
                    return i + 1;
            return 0;
        }
    }

    private float LimitRotationValue => m_curveData.LimitRotation[LimitRotationIndex];

    private float m_limitRotationValue;

    private bool m_rotateActive = false;

    [Space]
    [SerializeField] private Image m_bottleMaskImage;
    [SerializeField] private SpriteRenderer m_bottleMaskSprite;

    private float ValueFillAmount
    {
        set
        {
            if (m_bottleMaskImage != null)
                m_bottleMaskImage.material.SetFloat("_FillAmount", value);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                Material.SetFloat("_FillAmount", value);
            }
        }
        get
        {
            if (m_bottleMaskImage != null)
                return m_bottleMaskImage.material.GetFloat("_FillAmount");
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                return Material.GetFloat("_FillAmount");
            }
            return 0;
        }
    }

    private float ValueScaleAndRotate
    {
        set
        {
            if (m_bottleMaskImage != null)
                m_bottleMaskImage.material.SetFloat("_ScaleAndRotate", value);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                Material.SetFloat("_ScaleAndRotate", value);
            }
        }
        get
        {
            if (m_bottleMaskImage != null)
                return m_bottleMaskImage.material.GetFloat("_ScaleAndRotate");
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                return Material.GetFloat("_ScaleAndRotate");
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

    private Vector2 ValuePosition
    {
        set
        {
            if (m_bottleMaskImage != null)
                m_bottleMaskImage.material.SetVector("_Position", value);
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                Material.SetVector("_Position", value);
            }
        }
        get
        {
            if (m_bottleMaskImage != null)
                return m_bottleMaskImage.material.GetVector("_Position");
            else
            if (m_bottleMaskSprite != null)
            {
                Material Material = !Application.isPlaying ? m_bottleMaskSprite.sharedMaterial : m_bottleMaskSprite.material;
                return Material.GetVector("_Position");
            }
            return Vector2.zero;
        }
    }

    [Space]
    [SerializeField] private WaterSortBottleController m_bottleFillIn;

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

    private void Start()
    {
        m_bottleColorTop = BottleColorTop;
        m_bottleColorTopCount = BottleColorTopCount;
        //
        ValueFillAmount = m_curveData.LimitFillAmount[m_bottleColor.Count];
        ValuePosition = transform.position;
        //
        SetUpdateColorStart();
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (m_bottleFillIn == null)
            return;
        //
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_bottleFillIn.GetColorCheck(BottleColorTop, BottleColorTopCount)) 
                SetColorOut();
        }
    }

#endif

    private void SetUpdateColorStart()
    {
        for (int i = 0; i < m_bottleColor.Count; i++)
            ValueColor = (i, m_bottleColor[i]);
    }

    #region Out

    private void SetColorOut()
    {
        if (m_rotateActive)
            return;
        //
        if (m_bottleColor.Count == 0)
            return;
        //
        StartCoroutine(ISetRotate());
    }

    private IEnumerator ISetRotate()
    {
        m_rotateActive = true;
        //
        m_timeRotateCurrent = 0; //Time in curve to get value at the time of lerp value!
        m_timeRotateLerp = 0; //Time in ratio of Time Rotate from 0..1 value!
        float AngleValue = 0;
        float AngleValueLast = 0;
        m_bottleColorTop = BottleColorTop;
        m_bottleColorTopCount = BottleColorTopCount;
        m_limitRotationValue = LimitRotationValue;
        //
        m_bottleFillIn.SetColorFill(m_bottleColorTop, m_bottleColorTopCount); //Fill in another bottle!!
        //
        while (m_timeRotateCurrent < m_timeRotate)
        {
            m_timeRotateLerp = m_timeRotateCurrent / m_timeRotate;
            AngleValueLast = AngleValue;
            AngleValue = Mathf.Lerp(0.0f, m_limitRotationValue, m_timeRotateLerp);
            transform.eulerAngles = Vector3.forward * AngleValue;
            if (m_curveData.LimitFillAmount[m_bottleColor.Count] > m_curveData.CurveFillAmount.Evaluate(AngleValue))
            {
                ValueFillAmount = m_curveData.CurveFillAmount.Evaluate(AngleValue);
                ValuePosition = transform.position;
                m_bottleFillIn.SetColorFill(m_curveData.CurveFillAmount.Evaluate(AngleValueLast) - m_curveData.CurveFillAmount.Evaluate(AngleValue));
            }
            ValueScaleAndRotate = m_curveData.CurveScaleAndRotation.Evaluate(AngleValue);
            //
            m_timeRotateCurrent += Time.deltaTime * m_curveData.CurveRotationSpeed.Evaluate(AngleValue);
            //
            yield return new WaitForEndOfFrame();
        }
        //
        AngleValue = m_limitRotationValue;
        transform.eulerAngles = Vector3.forward * AngleValue;
        ValueFillAmount = m_curveData.CurveFillAmount.Evaluate(AngleValue);
        ValuePosition = transform.position;
        ValueScaleAndRotate = m_curveData.CurveScaleAndRotation.Evaluate(AngleValue);        
        //
        yield return ISetRotateBack();
        //
        for (int  i = m_bottleColor.Count - 1; i >= 0; i--)
        {
            if (m_bottleColor[i].ToString() != m_bottleColorTop.ToString())
                m_bottleColor.RemoveAt(m_bottleColor.Count - 1);
            else
                break;
        }
        //
        m_bottleFillIn.SetColorFillUpdate();
        //
        m_rotateActive = false;
    }

    private IEnumerator ISetRotateBack()
    {
        m_timeRotateCurrent = 0; //Time in curve to get value at the time!
        m_timeRotateLerp = 0;
        float AngleValue = 0;
        //
        while (m_timeRotateCurrent < m_timeRotate)
        {
            m_timeRotateLerp = m_timeRotateCurrent / m_timeRotate;
            AngleValue = Mathf.Lerp(m_limitRotationValue, 0.00f, m_timeRotateLerp);
            transform.eulerAngles = Vector3.forward * AngleValue;
            ValueScaleAndRotate = m_curveData.CurveScaleAndRotation.Evaluate(AngleValue);
            //
            m_timeRotateCurrent += Time.deltaTime;
            //
            yield return new WaitForEndOfFrame();
        }
        //
        AngleValue = 0f;
        transform.eulerAngles = Vector3.forward * AngleValue;
        ValueScaleAndRotate = m_curveData.CurveScaleAndRotation.Evaluate(AngleValue);
    }

    #endregion

    #region In

    private bool GetColorCheck(Color Color, int Count)
    {
        if (Color.ToString() != Color.ToString())
            return false;
        //
        if (m_bottleColor.Count + Count > COLOR_MAX)
            return false;
        //
        return true;
    }

    private void SetColorFill(Color Color, int Count)
    {
        int IndexStart = m_bottleColor.Count;
        for (int  i = 0; i < Count; i++)
        {
            m_bottleColor.Add(Color);
            ValueColor = (IndexStart + i, Color);
        }
    }

    private void SetColorFill(float CurveFillAmountValue)
    {
        ValueFillAmount = ValueFillAmount + CurveFillAmountValue;
        ValuePosition = transform.position;
    }

    private void SetColorFillUpdate()
    {
        m_bottleColorTop = BottleColorTop;
        m_bottleColorTopCount = BottleColorTopCount;
    }

    #endregion
}

#if UNITY_EDITOR

//[CustomEditor(typeof(WaterSortBottleController))]
public class BottleControllerEditor : Editor
{
}

#endif