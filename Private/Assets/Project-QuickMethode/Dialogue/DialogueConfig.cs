using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "dialogue-config", menuName = "QConfig/Dialogue Main", order = 1)]
public class DialogueConfig : ScriptableObject
{
    public List<DialogueDataAuthor> Author = new List<DialogueDataAuthor>();

    public DialogueDataTextDelay DelayDefault;

    //

    public List<string> AuthorName
    {
        get
        {
            List<string> NameFound = new List<string>();
            //
            if (Author == null)
                return NameFound;
            //
            if (Author.Count == 0)
                return NameFound;
            //
            foreach (DialogueDataAuthor AuthorItem in Author)
                NameFound.Add(AuthorItem.Name);
            //
            return NameFound;
        }
    }

    public List<Sprite> AuthorAvatar
    {
        get
        {
            if (Author == null)
                return null;
            //
            if (Author.Count == 0)
                return null;
            //
            List<Sprite> NameFound = new List<Sprite>();
            //
            foreach (DialogueDataAuthor AuthorItem in Author)
                NameFound.Add(AuthorItem.Avatar);
            //
            return NameFound;
        }
    }

    //

    public DialogueDataAuthor GetAuthor(int AuthorIndex)
    {
        return Author[AuthorIndex];
    }

    public DialogueDataAuthor GetAuthor(string Name)
    {
        return Author.Find(t => t.Name == Name);
    }
}

//Editor



#if UNITY_EDITOR

[CustomEditor(typeof(DialogueConfig))]
public class DialogueConfigEditor : Editor
{
    private DialogueConfig m_target;

    private SerializedProperty Author;
    private SerializedProperty DelayDefault;

    private void OnEnable()
    {
        m_target = target as DialogueConfig;
        //
        Author = QUnityEditorCustom.GetField(this, "Author");
        DelayDefault = QUnityEditorCustom.GetField(this, "DelayDefault");
        //
        SetConfigAuthorFixed();
    }

    private void OnDisable()
    {
        SetConfigAuthorFixed();
    }

    private void OnDestroy()
    {
        SetConfigAuthorFixed();
    }

    public override void OnInspectorGUI()
    {
        QUnityEditorCustom.SetUpdate(this);
        //
        QUnityEditorCustom.SetField(Author);
        QUnityEditorCustom.SetField(DelayDefault);
        //
        QUnityEditorCustom.SetApply(this);
    }

    //

    private void SetConfigAuthorFixed()
    {
        bool RemoveEmty = false;
        int Index = 0;
        while (Index < m_target.Author.Count)
        {
            if (m_target.Author[Index].Name == "")
            {
                RemoveEmty = true;
                m_target.Author.RemoveAt(Index);
            }
            else
                Index++;
        }
        QUnityEditor.SetDirty(m_target);
        //
        if (RemoveEmty)
            Debug.Log("[Dialogue] Author(s) emty have been remove from list");
    }
}

#endif