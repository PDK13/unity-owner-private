using QuickMethode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class UniqueID : MonoBehaviour
{
    [SerializeField] private string m_idAuto = "";
    [SerializeField] private bool m_idKeep = true;

    public string ID_AUTO => m_idAuto;

#if UNITY_EDITOR

    private void Awake()
    {
        if (Application.isPlaying)
            return;

        if (m_idAuto == "" || m_idAuto == "0")
        {
            if (!QGameObject.GetCheckPrefab(this.gameObject))
                SetUpdateIdentfier(true);
            else
                SetUpdateNormal(true);
        }
        else
        {
            SetOnDuplicate();
        }
    }

    private void Update()
    {
        if (Application.isPlaying)
            return;

        if (m_idAuto == "" || m_idAuto == "0")
        {
            if (!QGameObject.GetCheckPrefab(this.gameObject))
                SetUpdateIdentfier(true);
            else
                SetUpdateNormal(true);
        }
        else
        {
            SetUpdateIdentfier(false);
        }
    }

    private void SetOnDuplicate()
    {
        Event Event = Event.current;

        if (Event != null)
        {
            //If Prefab(s) got muti Duplicate, or got another event than Duplicate, need Refresh ID!!

            if (Event.type == EventType.ValidateCommand)
            {
                if (Event.commandName == "Paste" || Event.commandName == "Duplicate")
                {
                    if (!QGameObject.GetCheckPrefab(this.gameObject))
                        SetUpdateIdentfier(true);
                    else
                        SetUpdateNormal(true);
                }
                Event.Use();
            }

            if (Event.type == EventType.ExecuteCommand)
            {
                if (Event.commandName == "Paste" || Event.commandName == "Duplicate")
                {
                    if (!QGameObject.GetCheckPrefab(this.gameObject))
                        SetUpdateIdentfier(true);
                    else
                        SetUpdateNormal(true);
                }
            }
        }
        else
        if (QGameObject.GetFocus())
        {
            if (!QGameObject.GetCheckPrefab(this.gameObject))
                SetUpdateIdentfier(true);
            else
                SetUpdateNormal(true);
        }
    }

    private void SetUpdateNormal(bool Force)
    {
        if (!Force)
        {
            if (m_idKeep)
                return;
        }

        m_idAuto = QDateTime.GetFormat(QDateTime.Now, "yyMMddHHMMss") + ":" + transform.GetInstanceID().ToString().Replace("-", "");

        m_idKeep = true;
    }

    private void SetUpdateIdentfier(bool Force)
    {
        if (!Force)
        {
            if (QGameObject.GetCheckPrefab(this.gameObject))
                return;

            if (m_idKeep)
                return;
        }

        //Beward when unpack an Prefab tp GameObject, because it will change saved ID!!

        PropertyInfo inspectorModeInfo = typeof(UnityEditor.SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

        UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this.transform);
        inspectorModeInfo.SetValue(serializedObject, UnityEditor.InspectorMode.Debug, null);

        UnityEditor.SerializedProperty localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile");

        if (localIdProp.intValue.ToString() == "0")
            m_idAuto = localIdProp.intValue.ToString();
        else
            m_idAuto = QDateTime.GetFormat(QDateTime.Now, "yyMMddHHMMss") + ":" + localIdProp.intValue.ToString();

        m_idKeep = true;
    }

    public void SetUpdateRefresh()
    {
        SetUpdateNormal(true);
    }

    //private void OnValidate()
    //{
    //    //When value on GameObject or Component(s) changed!!
    //    if (m_idAuto == "")
    //    {
    //        if (!QGameObject.GetCheckPrefab(this.gameObject))
    //            SetUpdateIdentfier();
    //        else
    //            SetUpdateNormal(false);
    //    }
    //}

#endif
}