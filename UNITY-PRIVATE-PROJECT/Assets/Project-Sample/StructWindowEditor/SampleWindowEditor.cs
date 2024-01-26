#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class SampleWindowEditor : EditorWindow
{
    public static void Init()
    {
        GetWindow<SampleWindowEditor>("SampleWindowEditor");
    }

    private void OnGUI()
    {
        SetGUIMain();

        GUILayout.Space(10);

        SetGUIHorizontal();

        GUILayout.Space(10);

        SetGUIOption();

        GUILayout.Space(10);

        SetGUIBrowser();

        GUILayout.Space(10);

        SetGUIColor();
    }

    #region GUI Main

    private string m_Input;

    private void SetGUIMain()
    {
        GUIStyle m_Style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
        };

        GUILayout.Label("Label", m_Style, GUILayout.Width(10), GUILayout.Height(10));

        m_Input = EditorGUILayout.TextField("", m_Input);
    }

    #endregion

    #region GUI Horizontal (And Button)

    private void SetGUIHorizontal()
    {
        GUILayout.BeginHorizontal();

        for (int i = 0; i < 3; i++)
        {
            if (GUILayout.Button("Button " + i.ToString(), GUILayout.Width(position.width / 3)))
            {
                Debug.LogFormat("{0}: Button {1} Pressed!", name, i);
            }
        }

        GUILayout.EndHorizontal();
    }

    #endregion

    #region GUI Option

    private enum m_OptionList { Option1, Option2, Option3, };

    private m_OptionList m_Option;

    private void SetGUIOption()
    {
        m_Option = (m_OptionList)EditorGUILayout.EnumPopup("Option", m_Option);
    }

    #endregion

    #region GUI Folder / File Browser

    private string m_Path = "";

    private void SetGUIBrowser()
    {
        if (GUILayout.Button("Browser Folder"))
        {
            m_Path = EditorUtility.OpenFolderPanel("Blocks Folder", "", "");
        }

        if (GUILayout.Button("Browser File"))
        {
            m_Path = EditorUtility.OpenFilePanel("Blocks Folder", "", "");
        }
    }

    #endregion

    #region GUI Color

    private void SetGUIColor()
    {
        GUI.backgroundColor = Color.cyan;
        GUILayout.Button("Chance Color all render UI...");
        GUILayout.Button("Chance Color all render UI...");

        GUI.backgroundColor = Color.white;
        GUILayout.Button("Reset Color all render UI...");
    }

    #endregion
}

#endif