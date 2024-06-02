using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

///<summary>Excute command(s) of Window and Unity Editor on runtime</summary>
///<remarks><b>UnityEditorOnly</b></remarks>
public class QUnityAssets
{
    //Folder "Assets" is the main root of all assets in project, that can find any assets from it.

    #region ==================================== AssetsDatabase : Create Folder

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static string SetCreateFolder(params string[] PathChildInAssets)
    {
        Debug.LogFormat("[UnityAssets] Create Folder with path {0}", QPath.GetPath(QPath.PathType.None, PathChildInAssets));

        List<string> Path = new List<string>();
        for (int i = 0; i < PathChildInAssets.Length; i++)
        {
            Path.Add(PathChildInAssets[i]);
            SetCreateFolderExist(Path.ToArray());
        }

        return QPath.GetPath(QPath.PathType.None, PathChildInAssets);
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    private static string SetCreateFolderExist(params string[] PathChildInAssets)
    {
        string PathInAssets = "Assets";

        //If Root Folder not Exist, then can't Create new Folder from that Root Folder

        for (int i = 0; i < PathChildInAssets.Length - 1; i++)
            PathInAssets = QString.GetSplitAdd('/', PathInAssets, PathChildInAssets[i]);

        string FolderName = PathChildInAssets[PathChildInAssets.Length - 1];

        if (QPath.GetPathFolderExist(QPath.PathType.None, PathChildInAssets))
            //Exist Folder with path
            return "";

        try
        {
            string GUID = AssetDatabase.CreateFolder(PathInAssets, FolderName);
            SetRefresh();
            return AssetDatabase.GUIDToAssetPath(GUID); //GUID not emty is success!!
        }
        catch
        {
            //Something went wrong when created folder?!
            return "";
        }
    }

    #endregion

    #region ==================================== Delete

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static void SetDelete(params string[] PathChildInAssets)
    {
        FileUtil.DeleteFileOrDirectory(QPath.GetPath(QPath.PathType.None, PathChildInAssets) + ".meta");
        FileUtil.DeleteFileOrDirectory(QPath.GetPath(QPath.PathType.None, PathChildInAssets));

        SetRefresh();
    }

    #endregion

    #region ==================================== AssetsDatabase : Refresh

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static void SetRefresh()
    {
        AssetDatabase.Refresh();
    }

    #endregion

    #region ==================================== AssetsDatabase : Get Assets

    //Primary

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<GameObject> GetPrefab(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<GameObject> ObjectsFound = new List<GameObject>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Prefab"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Prefab"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            GameObject ObjectFound = AssetDatabase.LoadAssetAtPath<GameObject>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<AnimationClip> GetAnimationClip(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<AnimationClip> ObjectsFound = new List<AnimationClip>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:AnimationClip"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:AnimationClip"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            AnimationClip ObjectFound = AssetDatabase.LoadAssetAtPath<AnimationClip>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<AudioClip> GetAudioClip(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<AudioClip> ObjectsFound = new List<AudioClip>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:AudioClip"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:AudioClip"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            AudioClip ObjectFound = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<Font> GetFont(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<Font> ObjectsFound = new List<Font>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Font"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Font"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            Font ObjectFound = AssetDatabase.LoadAssetAtPath<Font>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<Material> GetMaterial(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<Material> ObjectsFound = new List<Material>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Material"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Material"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            Material ObjectFound = AssetDatabase.LoadAssetAtPath<Material>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<PhysicMaterial> GetPhysicMaterial(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<PhysicMaterial> ObjectsFound = new List<PhysicMaterial>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:PhysicMaterial"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:PhysicMaterial"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            PhysicMaterial ObjectFound = AssetDatabase.LoadAssetAtPath<PhysicMaterial>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<Texture> GetTexture(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<Texture> ObjectsFound = new List<Texture>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Texture"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Texture"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            Texture ObjectFound = AssetDatabase.LoadAssetAtPath<Texture>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<Texture2D> GetTexture2D(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<Texture2D> ObjectsFound = new List<Texture2D>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Texture2D"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Texture2D"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            Texture2D ObjectFound = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<VideoClip> GetVideoClip(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<VideoClip> ObjectsFound = new List<VideoClip>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:VideoClip"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:VideoClip"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            VideoClip ObjectFound = AssetDatabase.LoadAssetAtPath<VideoClip>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<Sprite> GetSprite(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<Sprite> ObjectsFound = new List<Sprite>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Sprite"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Sprite"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            Sprite ObjectFound = AssetDatabase.LoadAssetAtPath<Sprite>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<RuntimeAnimatorController> GetAnimatorController(string NameSpecial, bool NameCheck, params string[] PathChildInAssets)
    {
        List<RuntimeAnimatorController> ObjectsFound = new List<RuntimeAnimatorController>();
        //
        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:AnimatorController"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:AnimatorController"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            RuntimeAnimatorController ObjectFound = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<TextAsset> GetTextAsset(string NameSpecial, bool NameCheck, QPath.ExtensionType FileExtension, params string[] PathChildInAssets)
    {
        List<TextAsset> ObjectsFound = new List<TextAsset>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:TextAsset"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:TextAsset"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);

            if (!AssetsSinglePath.Contains(QPath.GetFileName("", FileExtension)))
                continue;

            TextAsset ObjectFound = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetsSinglePath);

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound;
    }

    //Custom

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<T> GetPrefab<T>(string NameSpecial, bool NameCheck, params string[] PathChildInAssets) where T : Component
    {
        List<T> ObjectsFound = new List<T>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Prefab"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:Prefab"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            GameObject ObjectFound = AssetDatabase.LoadAssetAtPath<GameObject>(AssetsSinglePath);

            if (ObjectFound.GetComponent<T>() == null)
                continue;

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound.GetComponent<T>());
        }

        return ObjectsFound;
    }

    ///<remarks><b>UnityEditorOnly</b></remarks>
    public static List<T> GetScriptableObject<T>(string NameSpecial, bool NameCheck, params string[] PathChildInAssets) where T : ScriptableObject
    {
        List<T> ObjectsFound = new List<T>();

        string[] GUIDPathUnityFound;
        if (PathChildInAssets.Length == 0 || (PathChildInAssets.Length == 1 && PathChildInAssets[0] == ""))
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:ScriptableObject"));
        else
            GUIDPathUnityFound = AssetDatabase.FindAssets(string.Format("{0} {1}", NameSpecial, "t:ScriptableObject"), PathChildInAssets);

        foreach (string GUIDPath in GUIDPathUnityFound)
        {
            string AssetsSinglePath = AssetDatabase.GUIDToAssetPath(GUIDPath);
            T ObjectFound = AssetDatabase.LoadAssetAtPath<T>(AssetsSinglePath);

            if (ObjectFound == null)
                continue;

            if (!ObjectFound.GetType().Equals(typeof(T)))
                continue;

            if (NameCheck && ObjectFound.name != NameSpecial)
                continue;

            ObjectsFound.Add(ObjectFound);
        }

        return ObjectsFound as List<T>;
    }

    #endregion
}

#endif