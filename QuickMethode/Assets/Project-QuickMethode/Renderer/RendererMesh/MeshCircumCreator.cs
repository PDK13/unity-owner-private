using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCircumCreator : MonoBehaviour
{
    [SerializeField] private MeshFilter m_meshFilter;

    [Space]
    [Min(3)] public int FilledPoints = 3;
    [Min(0)] public float FilledRadius = 2f;
    public float FilledDeg = 0f;

    [Space]
    [Min(0)] public float HollowRadius = 0f;

    [Space]
    public Vector3[] Points;
    public int[] Triangles;

    [Space]
    public QMeshCircum Circum;

    public void SetGenerate()
    {
        if (Application.isPlaying)
        {
            m_meshFilter.mesh.Clear();
            m_meshFilter.mesh.vertices = Points;
            m_meshFilter.mesh.triangles = Triangles;
            m_meshFilter.mesh.RecalculateNormals();
            m_meshFilter.mesh.RecalculateBounds();
        }
        else
        {
            Mesh Mesh = new Mesh();
            Mesh.vertices = Points;
            Mesh.triangles = Triangles;
            Mesh.RecalculateNormals();
            Mesh.RecalculateBounds();
            m_meshFilter.mesh = Mesh;
        }
    }

    public void SetGenerateFilled()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new QMeshCircum(m_meshFilter);
        //
        Circum.SetFilledGenerate(FilledPoints, FilledRadius, FilledDeg);
        //
        Points = Circum.Points;
        Triangles = Circum.Triangles;
    }

    public void SetGenerateHollow()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new QMeshCircum(m_meshFilter);
        //
        Circum.SetHollowGenerate(FilledPoints, FilledRadius, HollowRadius, FilledDeg);
        //
        Points = Circum.Points;
        Triangles = Circum.Triangles;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MeshCircumCreator))]
public class MeshCreatorEditor : Editor
{
    private MeshCircumCreator m_target;

    private SerializedProperty m_meshFilter;

    private SerializedProperty FilledPoints;
    private SerializedProperty FilledRadius;
    private SerializedProperty FilledDeg;

    private SerializedProperty HollowRadius;

    private SerializedProperty Points;
    private SerializedProperty Triangles;

    private void OnEnable()
    {
        m_target = target as MeshCircumCreator;
        //
        m_meshFilter = QEditorCustom.GetField(this, "m_meshFilter");
        //
        FilledPoints = QEditorCustom.GetField(this, "FilledPoints");
        FilledRadius = QEditorCustom.GetField(this, "FilledRadius");
        FilledDeg = QEditorCustom.GetField(this, "FilledDeg");
        //
        HollowRadius = QEditorCustom.GetField(this, "HollowRadius");
        //
        Points = QEditorCustom.GetField(this, "Points");
        Triangles = QEditorCustom.GetField(this, "Triangles");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditorCustom.SetField(m_meshFilter);
        //
        QEditorCustom.SetField(FilledPoints);
        QEditorCustom.SetField(FilledRadius);
        QEditorCustom.SetField(FilledDeg);
        //
        if (QEditor.SetButton("Generate Filled"))
            m_target.SetGenerateFilled();
        //
        QEditorCustom.SetField(HollowRadius);
        //
        if (QEditor.SetButton("Generate Hollow"))
            m_target.SetGenerateHollow();
        //
        QEditorCustom.SetField(Points);
        QEditorCustom.SetField(Triangles);
        //
        if (QEditor.SetButton("Generate"))
            m_target.SetGenerate();
        //
        QEditorCustom.SetApply(this);
    }
}

#endif