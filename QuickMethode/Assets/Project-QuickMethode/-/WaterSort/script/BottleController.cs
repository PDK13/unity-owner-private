using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BottleController : MonoBehaviour
{
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
    [SerializeField] private AnimationCurve m_curveFillAmount;
    [SerializeField] private AnimationCurve m_curveScaleAndRotation;
    [SerializeField] private AnimationCurve m_curveRotationSpeed;
    [SerializeField] private List<float> m_limitRotation = new List<float>();
    [SerializeField] private List<float> m_limitFillAmount = new List<float>();

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

    private float LimitRotationValue => m_limitRotation[LimitRotationIndex];

    private float m_limitRotationValue;

    private bool m_rotateActive = false;

    [Space]
    [SerializeField] private SpriteRenderer m_bottleMark;

    [SerializeField] private BottleController m_bottleFillIn;

    private Material m_bottleMaterial => !Application.isPlaying ? m_bottleMark.sharedMaterial : m_bottleMark.material;

    private void Start()
    {
        m_bottleColorTop = BottleColorTop;
        m_bottleColorTopCount = BottleColorTopCount;
        //
        m_bottleMaterial.SetFloat("_FillAmount", m_limitFillAmount[m_bottleColor.Count]);
        //
        SetUpdateColorStart();
    }

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

    private void SetUpdateColorStart()
    {
        for (int i = 0; i < m_bottleColor.Count; i++)
            m_bottleMaterial.SetColor(string.Format("_Color{0}", i), m_bottleColor[i]);
    }

    private void SetUpdateColorController()
    {

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
            if (m_limitFillAmount[m_bottleColor.Count] > m_curveFillAmount.Evaluate(AngleValue))
            {
                m_bottleMaterial.SetFloat("_FillAmount", m_curveFillAmount.Evaluate(AngleValue));
                m_bottleFillIn.SetColorFill(m_curveFillAmount.Evaluate(AngleValueLast) - m_curveFillAmount.Evaluate(AngleValue));
            }
            m_bottleMaterial.SetFloat("_ScaleAndRotate", m_curveScaleAndRotation.Evaluate(AngleValue));
            //
            m_timeRotateCurrent += Time.deltaTime * m_curveRotationSpeed.Evaluate(AngleValue);
            //
            yield return new WaitForEndOfFrame();
        }
        //
        AngleValue = m_limitRotationValue;
        transform.eulerAngles = Vector3.forward * AngleValue;
        m_bottleMaterial.SetFloat("_FillAmount", m_curveFillAmount.Evaluate(AngleValue));
        m_bottleMaterial.SetFloat("_ScaleAndRotate", m_curveScaleAndRotation.Evaluate(AngleValue));
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
            m_bottleMaterial.SetFloat("_ScaleAndRotate", m_curveScaleAndRotation.Evaluate(AngleValue));
            //
            m_timeRotateCurrent += Time.deltaTime;
            //
            yield return new WaitForEndOfFrame();
        }
        //
        AngleValue = 0f;
        transform.eulerAngles = Vector3.forward * AngleValue;
        m_bottleMaterial.SetFloat("_ScaleAndRotate", m_curveScaleAndRotation.Evaluate(AngleValue));
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
            m_bottleMaterial.SetColor(string.Format("_Color{0}", IndexStart + i), Color);
        }
    }

    private void SetColorFill(float CurveFillAmountValue)
    {
        m_bottleMaterial.SetFloat("_FillAmount", m_bottleMaterial.GetFloat("_FillAmount") + CurveFillAmountValue);
    }

    private void SetColorFillUpdate()
    {
        m_bottleColorTop = BottleColorTop;
        m_bottleColorTopCount = BottleColorTopCount;
    }

    #endregion
}

#if UNITY_EDITOR

[CustomEditor(typeof(BottleController))]
public class BottleControllerEditor : Editor
{
    private BottleController m_target;

    private SerializedProperty m_timeRotate;
    //
    private SerializedProperty m_curveFillAmount;
    private SerializedProperty m_curveScaleAndRotation;
    private SerializedProperty m_curveData;

    private void OnEnable()
    {
        m_target = target as BottleController;
        //
        m_timeRotate = QEditorCustom.GetField(this, "m_timeRotate");
        //
        m_curveFillAmount = QEditorCustom.GetField(this, "m_curveFillAmount");
        m_curveScaleAndRotation = QEditorCustom.GetField(this, "m_curveScaleAndRotation");
        m_curveData = QEditorCustom.GetField(this, "m_curveData");
    }

    //public override void OnInspectorGUI()
    //{
    //    QEditorCustom.SetUpdate(this);
    //    //
    //    QEditorCustom.SetLabel("SETTING", QEditorCustom.GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
    //    QEditorCustom.SetField(m_timeRotate);
    //    //
    //    QEditorCustom.SetSpace(10);
    //    //
    //    QEditorCustom.SetLabel("COLOR", QEditorCustom.GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
    //    for (int i = 0; i < m_target.BottleColor.Count; i++)
    //    {
    //        QEditorCustom.SetHorizontalBegin();
    //        QEditorCustom.SetLabel(i.ToString(), QEditorCustom.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter), QEditorCustom.GetGUIWidth(25));
    //        m_target.BottleColor[i] = QEditorCustom.SetField(m_target.BottleColor[i], QEditorCustom.GetGUIWidth(100));
    //        QEditorCustom.SetHorizontalEnd();
    //    }
    //    //
    //    QEditorCustom.SetSpace(10);
    //    //
    //    QEditorCustom.SetLabel("ANIMATION", QEditorCustom.GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
    //    QEditorCustom.SetField(m_curveFillAmount);
    //    QEditorCustom.SetField(m_curveScaleAndRotation);
    //    QEditorCustom.SetField(m_curveData);
    //    //
    //    QEditorCustom.SetApply(this);
    //}
}

#endif