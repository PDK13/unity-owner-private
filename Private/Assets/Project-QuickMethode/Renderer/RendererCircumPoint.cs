using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RendererCircumPoint : MonoBehaviour
{
    [SerializeField][Range(3, 60)] private int m_outerPoint = 3;
    float[] m_outerPointRatio = new float[3];
    //
    [SerializeField][Min(0)] private float m_outerRadius = 2f;
    [SerializeField] private float m_outerDeg = 0f;
    //
    private RendererCircumPointData m_data;

    public int OuterPoint
    {
        get => m_outerPoint;
        set
        {
            if (value < 3)
                value = 3;
            //
            if (value > 60)
                value = 60;
            //
            m_outerPoint = value;
            m_outerPointRatio = new float[value];
        }
    }

    public float[] OuterPointRatio
    {
        get => m_outerPointRatio;
        set
        {
            if (value.Length < 3)
                return;
            //
            for (int i = 0; i < value.Length; i++) if (value[i] < 0 && value[i] < -OuterRadius) value[i] = -OuterRadius;
            //
            m_outerPoint = value.Length;
            m_outerPointRatio = value;
        }
    }

    public float OuterRadius
    {
        get => m_outerRadius;
        set => m_outerRadius = value >= 0 ? value : m_outerRadius;
    }

    public float OuterDeg
    {
        get => m_outerDeg;
        set => m_outerDeg = value;
    }

    public RendererCircumPointData Data => m_data;

    public void SetGenerate()
    {
        m_data = new RendererCircumPointData(OuterPointRatio, OuterRadius, OuterDeg);
    }

    #region Sample

    public void SetSampleStarA()
    {
        OuterPoint = 10;
        //
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 2 != 0)
                continue;
            //
            OuterPointRatio[i] = OuterRadius * 0.6f;
        }
        //
        OuterDeg = 17.5f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    public void SetSampleStarB()
    {
        OuterPoint = 12;
        //
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 4 == 0)
                OuterPointRatio[i] = OuterRadius * 0.1f;
            else
            if (i % 2 != 0)
                OuterPointRatio[i] = OuterRadius * 0.6f;
        }
        //
        OuterDeg = 30f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    //

    public void SetSampleStarC()
    {
        OuterPoint = 15;
        //
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 != 0)
                continue;
            //
            OuterPointRatio[i] = OuterRadius * 0.5f;
        }
        //
        OuterDeg = 17.5f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    public void SetSampleStarD()
    {
        OuterPoint = 18;
        //
        int Draw = 1;
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 == 0)
                OuterPointRatio[i] = OuterRadius * 0.4f;
            else
            {
                if (Draw == 1 || Draw == 2)
                    OuterPointRatio[i] = OuterRadius * 0.1f;
                Draw++;
                if (Draw > 4)
                    Draw = 1;
            }
        }
        //
        OuterDeg = 0f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    //

    public void SetSampleStarE()
    {
        OuterPoint = 24;
        //
        int Draw = 1;
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 == 0)
                OuterPointRatio[i] = OuterRadius * 0.4f;
            else
            {
                if (Draw == 1 || Draw == 2)
                    OuterPointRatio[i] = OuterRadius * 0.2f;
                Draw++;
                if (Draw > 4)
                    Draw = 1;
            }
        }
        //
        OuterDeg = 22.5f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    public void SetSampleStarF()
    {
        OuterPoint = 36;
        //
        int Draw = 1;
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 == 0)
                OuterPointRatio[i] = OuterRadius * 0.6f;
            else
            {
                if (Draw == 1 || Draw == 2)
                    OuterPointRatio[i] = OuterRadius * 0.3f;
                Draw++;
                if (Draw > 4)
                    Draw = 1;
            }

        }
        //
        OuterDeg = 15f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    #endregion

    #region Editor

    public void SetEditorPointsRatioChange()
    {
        if (m_outerPointRatio.Length < 3)
        {
            m_outerPoint = 3;
            m_outerPointRatio = new float[3];
        }
        else
        if (m_outerPointRatio.Length != m_outerPoint)
        {
            m_outerPointRatio = new float[m_outerPoint];
        }
    }

    private void OnDrawGizmosSelected()
    {
        QGizmos.SetWireSphere(this.transform.position, OuterRadius, Color.gray);
        //
        SetGenerate();
        //
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
        //
        Vector3 PointA, PointB;
        for (int i = 0; i < Data.OuterPoint - 1; i++)
        {
            PointA = this.transform.position + Data.OuterPoints[i];
            PointB = this.transform.position + Data.OuterPoints[i + 1];
            QGizmos.SetLine(PointA, PointB, Color.green, 0.1f);
        }
        PointA = this.transform.position + Data.OuterPoints[Data.OuterPoints.Length - 1];
        PointB = this.transform.position + Data.OuterPoints[0];
        QGizmos.SetLine(PointA, PointB, Color.green, 0.1f);
    }

    #endregion
}

public class RendererCircumPointData
{
    public int OuterPoint { private set; get; } = 0;
    public float OuterRadius { private set; get; } = 0;
    public float OuterDeg { private set; get; } = 0;

    public Vector3[] OuterPoints { private set; get; } = new Vector3[0];
    public float[] OuterPointsRatio { private set; get; } = new float[0];

    public RendererCircumPointData()
    {
        //...
    }

    public RendererCircumPointData(int Point, float Radius, float Deg)
    {
        SetOuterGenerate(Point, Radius, Deg);
    }

    public RendererCircumPointData(float[] PointRatio, float Radius, float Deg)
    {
        SetOuterGenerate(PointRatio, Radius, Deg);
    }

    //

    public bool SetOuterGenerate(int Point, float Radius, float Deg)
    {
        if (Point < 3)
            //One shape must have 3 points at least!!
            return false;
        //
        this.OuterPoint = Point;
        this.OuterPointsRatio = new float[0];
        this.OuterRadius = Radius;
        this.OuterDeg = Deg;
        //
        this.OuterPoints = GetOuterterPoint();
        //
        return true;
    }

    public bool SetOuterGenerate(float[] PointRatio, float Radius, float Deg)
    {
        if (PointRatio.Length < 3)
            //One shape must have 3 points at least!!
            return false;
        //
        this.OuterPoint = PointRatio.Length;
        this.OuterPointsRatio = PointRatio;
        this.OuterRadius = Radius;
        this.OuterDeg = Deg;
        //
        this.OuterPoints = GetOuterterPoint();
        this.OuterPoints = GetOuterPointRatio();
        //
        return true;
    }

    private Vector3[] GetOuterterPoint()
    {
        if (OuterPoint < 3)
            //One shape must have 3 points at least!!
            return null;
        //
        List<Vector3> Points = new List<Vector3>();
        //
        float RadSpace = (360 / OuterPoint) * (Mathf.PI / 180);
        float RadStart = (OuterDeg) * (Mathf.PI / 180);
        float RadCur = RadStart;
        //
        Vector3 PointStart = new Vector3(Mathf.Cos(RadStart) * OuterRadius, Mathf.Sin(RadStart) * OuterRadius, 0f);
        Points.Add(PointStart);
        for (int i = 1; i < OuterPoint; i++)
        {
            RadCur += RadSpace;
            Vector3 NewPoint = new Vector3(Mathf.Cos(RadCur) * OuterRadius, Mathf.Sin(RadCur) * OuterRadius, 0f);
            Points.Add(NewPoint);
        }
        //
        return Points.ToArray();
    }

    private Vector3[] GetOuterPointRatio()
    {
        Vector3[] Points = this.OuterPoints;
        for (int i = 0; i < OuterPointsRatio.Length; i++)
            Points[i] = Points[i] + Points[i].normalized * OuterPointsRatio[i];
        return Points;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(RendererCircumPoint))]
public class RendererCircumPointEditor : Editor
{
    private RendererCircumPoint m_target;

    private SerializedProperty m_outerPoint;
    private SerializedProperty m_outerRadius;
    private SerializedProperty m_outerDeg;

    private Vector2 m_scrollOuterPointRatio;

    private void OnEnable()
    {
        m_target = target as RendererCircumPoint;
        //
        m_outerPoint = QUnityEditorCustom.GetField(this, "m_outerPoint");
        m_outerRadius = QUnityEditorCustom.GetField(this, "m_outerRadius");
        m_outerDeg = QUnityEditorCustom.GetField(this, "m_outerDeg");
    }

    public override void OnInspectorGUI()
    {
        QUnityEditorCustom.SetUpdate(this);
        //
        QUnityEditor.SetLabel("SETTING", QUnityEditor.GetGUIStyleLabel(FontStyle.Bold));
        //
        QUnityEditorCustom.SetField(m_outerPoint);
        //
        if (m_target.OuterPointRatio.Length != m_target.OuterPoint)
            m_target.OuterPointRatio = new float[m_target.OuterPoint];
        //
        int i = 0;
        m_scrollOuterPointRatio = QUnityEditor.SetScrollViewBegin(m_scrollOuterPointRatio, QUnityEditor.GetGUILayoutHeight(105));
        while (i < m_target.OuterPoint)
        {
            if (m_target.OuterPointRatio[i] < 0 && m_target.OuterPointRatio[i] < -m_target.OuterRadius)
                m_target.OuterPointRatio[i] = -m_target.OuterRadius;
            //
            //VIEW:
            QUnityEditor.SetHorizontalBegin();
            QUnityEditor.SetLabel(string.Format("{0}", i), QUnityEditor.GetGUIStyleLabel(), QUnityEditor.GetGUILayoutWidth(25));
            m_target.OuterPointRatio[i] = QUnityEditor.SetField(m_target.OuterPointRatio[i], null, QUnityEditor.GetGUILayoutWidth(50));
            //
            if (m_target.Data != null)
                if (m_target.Data.OuterPoints != null)
                    if (i < m_target.Data.OuterPoints.Length)
                        QUnityEditor.SetLabel(((Vector2)m_target.Data.OuterPoints[i]).ToString(), QUnityEditor.GetGUIStyleLabel());
            //
            QUnityEditor.SetHorizontalEnd();
            //VIEW:
            //
            i++;
        }
        QUnityEditor.SetScrollViewEnd();
        //
        QUnityEditorCustom.SetField(m_outerRadius);
        QUnityEditorCustom.SetField(m_outerDeg);
        //
        QUnityEditor.SetSpace(10);
        //
        QUnityEditor.SetLabel("SAMPLE", QUnityEditor.GetGUIStyleLabel(FontStyle.Bold));
        //
        QUnityEditor.SetHorizontalBegin();
        if (QUnityEditor.SetButton("Star A"))
            m_target.SetSampleStarA();
        if (QUnityEditor.SetButton("Star B"))
            m_target.SetSampleStarB();
        QUnityEditor.SetHorizontalEnd();
        //
        QUnityEditor.SetHorizontalBegin();
        if (QUnityEditor.SetButton("Star C"))
            m_target.SetSampleStarC();
        if (QUnityEditor.SetButton("Star D"))
            m_target.SetSampleStarD();
        QUnityEditor.SetHorizontalEnd();
        //
        QUnityEditor.SetHorizontalBegin();
        if (QUnityEditor.SetButton("Star E"))
            m_target.SetSampleStarE();
        if (QUnityEditor.SetButton("Star F"))
            m_target.SetSampleStarF();
        QUnityEditor.SetHorizontalEnd();
        //
        QUnityEditorCustom.SetApply(this);
    }
}

#endif