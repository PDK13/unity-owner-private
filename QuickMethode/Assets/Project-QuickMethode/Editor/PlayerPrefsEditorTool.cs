#if UNITY_EDITOR

using QuickMethode;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsEditorTool : EditorWindow
{
    #region Varible: Primary

    private readonly string[] m_valueType = { "String", "Int", "Float" };
    private string m_name = "";
    private int m_valueTypeIndex = 0;
    private string m_value = "";

    #endregion

    #region Varible: List

    private List<(string Name, int Choice)> m_list = new List<(string Name, int Choice)>();
    private Vector2 m_listScrollView;

    #endregion

    [MenuItem("Tools/Project Script/Player Prefs")]
    public static void Init()
    {
        GetWindow<PlayerPrefsEditorTool>("PLAYER-PREFS");
    }

    private void OnGUI()
    {
        SetGUIRef();

        SetGUIOption();

        SetGUIList();
    }

    #region Ref

    private void SetGUIRef()
    {
        GUIStyle m_StyleLabel = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
        };

        GUIStyle m_Style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
        };

        GUILayout.Label("REF", m_StyleLabel);

        //Type

        GUILayout.BeginHorizontal();

        GUILayout.Label("Type", m_Style, GUILayout.Width(position.width / 5));

        m_valueTypeIndex = EditorGUILayout.Popup("", m_valueTypeIndex, m_valueType);

        GUILayout.EndHorizontal();

        //Name

        GUILayout.BeginHorizontal();

        GUILayout.Label("Name", m_Style, GUILayout.Width(position.width / 5));

        m_name = EditorGUILayout.TextField("", m_name);

        GUILayout.EndHorizontal();

        //Value

        GUILayout.BeginHorizontal();

        GUILayout.Label("Value", m_Style, GUILayout.Width(position.width / 5));

        m_value = EditorGUILayout.TextField("", m_value);

        GUILayout.EndHorizontal();

        GUILayout.Space(10f);
    }

    #endregion

    #region Option


    private const int m_ButtonMainHorizontalCount = 4;

    private void SetGUIOption()
    {
        GUIStyle m_StyleLabel = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
        };

        GUIStyle m_Style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
        };

        GUILayout.Label("OPPTION", m_StyleLabel);

        GUILayout.BeginHorizontal();

        SetGUIButtonSet();

        SetGUIButtonGet();

        SetGUIButtonClear();

        SetGUIButtonClearAll();

        GUILayout.EndHorizontal();

        GUILayout.Space(10f);
    }

    private void SetGUIButtonSet()
    {
        if (GUILayout.Button("Set", GUILayout.Width(position.width / m_ButtonMainHorizontalCount)))
        {
            if (!m_name.Equals("") && !m_value.Equals(""))
            {
                if (m_valueType[m_valueTypeIndex] == m_valueType[0])
                    //String
                    QPlayerPrefs.SetValue(m_name, m_value);
                else
                if (m_valueType[m_valueTypeIndex] == m_valueType[1])
                    //Int
                    QPlayerPrefs.SetValue(m_name, int.Parse(m_value));
                else
                if (m_valueType[m_valueTypeIndex] == m_valueType[2])
                    //Float
                    QPlayerPrefs.SetValue(m_name, float.Parse(m_value));
            }
        }
    }

    private void SetGUIButtonGet()
    {
        if (GUILayout.Button("Get"))
        {
            if (!m_name.Equals(""))
            {
                if (m_valueType[m_valueTypeIndex] == m_valueType[0])
                    //String
                    m_value = QPlayerPrefs.GetValueString(m_name);
                else
                if (m_valueType[m_valueTypeIndex] == m_valueType[1])
                    //Int
                    m_value = QPlayerPrefs.GetValueInt(m_name).ToString();
                else
                if (m_valueType[m_valueTypeIndex] == m_valueType[2])
                    //Float
                    m_value = QPlayerPrefs.GetValueFloat(m_name).ToString();
            }
        }
    }

    private void SetGUIButtonClear()
    {
        if (GUILayout.Button("Clear", GUILayout.Width(position.width / m_ButtonMainHorizontalCount)))
            QPlayerPrefs.SetValueClear(m_name);
    }

    private void SetGUIButtonClearAll()
    {
        if (GUILayout.Button("Clear All"))
            QPlayerPrefs.SetValueClearAll();
    }

    #endregion

    #region List

    private string GetGUIListPath()
    {
        return QPath.GetPath(QPath.PathType.Assets, @"..", "Ref-Data.txt");
    }

    private void SetGUIListDataRead()
    {
        m_list = new List<(string Name, int Choice)>();

        string m_FileDataPath = GetGUIListPath();

        if (QPath.GetPathFileExist(m_FileDataPath))
        {
            QFileIO m_FileIO = new QFileIO();
            m_FileIO.SetReadStart(m_FileDataPath);
            int Count = m_FileIO.GetReadAutoInt();
            for (int i = 0; i < Count; i++)
            {
                m_list.Add((m_FileIO.GetReadAutoString(), m_FileIO.GetReadAutoInt()));
            }
        }
        else
        {
            QFileIO m_FileIO = new QFileIO();
            m_FileIO.SetWriteStart(m_FileDataPath);
        }
    }

    private void SetGUIListDataSave()
    {
        string m_FileDataPath = GetGUIListPath();

        QFileIO m_FileIO = new QFileIO();
        m_FileIO.SetWriteAdd(m_list.Count);
        for (int i = 0; i < m_list.Count; i++)
        {
            m_FileIO.SetWriteAdd(m_list[i].Name);
            m_FileIO.SetWriteAdd(m_list[i].Choice);
        }

        m_FileIO.SetWriteStart(m_FileDataPath);
    }

    private void SetGUIList()
    {
        QEditor.SetLabel("LIST", QEditor.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter), QEditorWindow.GetGUILayoutWidth(this));

        if (QEditor.SetButton("Refresh", QEditor.GetGUIButton(FontStyle.Normal, TextAnchor.MiddleCenter), QEditorWindow.GetGUILayoutWidth(this))) 
            SetGUIListDataRead();

        GUILayout.Space(10f);

        SetGUIListMemory();

        GUILayout.Space(10f);
    }

    private void SetGUIListMemory()
    {
        QEditor.SetScrollViewBegin(m_listScrollView);

        for (int i = 0; i < m_list.Count + 1; i++)
        {
            GUILayout.BeginHorizontal();

            if (i < m_list.Count)
            {
                if (QEditor.SetButton(m_list[i].Name, QEditor.GetGUIButton(FontStyle.Normal, TextAnchor.MiddleCenter), QEditorWindow.GetGUILayoutWidth(this, 0.7f)))
                {
                    m_name = m_list[i].Name;
                    m_valueTypeIndex = m_list[i].Choice;
                }

                QEditor.SetLabel(m_valueType[m_list[i].Choice], QEditor.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter), QEditorWindow.GetGUILayoutWidth(this, 0.2f));

                if (QEditor.SetButton("Del", QEditor.GetGUIButton(FontStyle.Normal, TextAnchor.MiddleCenter), QEditorWindow.GetGUILayoutWidth(this, 0.1f)))
                {
                    m_list.RemoveAt(i);
                    SetGUIListDataSave();
                }
            }
            else
            {
                if (QEditor.SetButton("[New]", QEditor.GetGUIButton(FontStyle.Normal, TextAnchor.MiddleCenter), QEditorWindow.GetGUILayoutWidth(this)))
                {
                    if (m_name != "")
                    {
                        m_list.Add((m_name, m_valueTypeIndex));
                        SetGUIListDataSave();
                    }
                }
            }

            GUILayout.EndHorizontal();
        }

        QEditor.SetScrollViewEnd();
    }

    #endregion
}

#endif