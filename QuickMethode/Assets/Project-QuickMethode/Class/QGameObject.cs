using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QGameObject
{
    #region ==================================== Create

    //Primary

    public static GameObject SetCreate(GameObject Prepab, Transform Parent = null, bool WorldStay = true)
    {
        if (Prepab == null)
        {
            Debug.LogWarning("[GameObject] Can't create an GameObject with null!");
            return null;
        }
        //
        GameObject GameObject = MonoBehaviour.Instantiate(Prepab);
        //
        if (Parent != null)
            GameObject.transform.SetParent(Parent, WorldStay);
        //
        GameObject.transform.position = Prepab.transform.position;
        GameObject.transform.localScale = Prepab.transform.localScale;
        //
        return GameObject;
    }

    public static GameObject SetCreate(string Name, Transform Parent = null, bool WorldStay = true)
    {
        GameObject GameObject = new GameObject(Name);
        //
        if (Parent != null)
            GameObject.transform.SetParent(Parent, WorldStay);
        //
        return GameObject;
    }

    //Component

    public static T SetCreateComponent<T>(GameObject Prepab, Transform Parent = null, bool WorldStay = true) where T : Component
    {
        GameObject GameObject = SetCreate(Prepab, Parent, WorldStay);
        //
        if (GameObject == null)
            return default(T);
        //
        if (GameObject.GetComponent<T>() == null)
            return GameObject.AddComponent<T>();
        else
            return GameObject.GetComponent<T>();
    }

    public static T SetCreateComponent<T>(string Name, Transform Parent = null, bool WorldStay = true) where T : Component
    {
        return SetCreate(Name, Parent, WorldStay).AddComponent<T>();
    }

    #endregion

    #region ==================================== Destroy

    /// <summary>
    /// Destroy GameObject, Component or any Object if avaible in Scene or Unity
    /// </summary>
    /// <param name="From"></param>
    public static void SetDestroy(Object From)
    {
        //Remove GameObject from Scene or Component from GameObject!!
        //
        if (From == null)
            return;
        //
        if (Application.isEditor)
        {
            if (Application.isPlaying)
                MonoBehaviour.Destroy(From);
            else
                MonoBehaviour.DestroyImmediate(From);
        }
        else
            MonoBehaviour.Destroy(From);
    }

    #endregion

    #region ==================================== Transform

    public static void SetIndex(Transform From, int Index)
    {
        if (From.parent != null)
        {
            if (Index < 0 || Index > From.parent.childCount - 1)
            {
                return;
            }
        }

        From.SetSiblingIndex(Index);
    }

    #endregion

    #region ==================================== Message

    public static void SetMessage(GameObject From, string MethodeName, SendMessageOptions Option = SendMessageOptions.DontRequireReceiver)
    {
        From.SendMessage(MethodeName, Option);
    }

    #endregion

    #region ==================================== GameObject

    public static string GetNameReplaceClone(string Name)
    {
        return Name.Replace("(Clone)", "");
    }

#if UNITY_EDITOR

    //Prefab

    /// <summary>
    /// Caution: Unity Editor only!
    /// </summary>
    /// <param name="From"></param>
    /// <returns></returns>
    public static bool GetCheckPrefab(GameObject From)
    {
        //Check if GameObject is a Prefab?!
#if UNITY_2018_3_OR_NEWER
        return PrefabUtility.IsPartOfAnyPrefab(From);
#else
        return PrefabUtility.GetPrefabType(go) != PrefabType.None;
#endif
    }

    //Focus

    ///<summary>
    ///Caution: Unity Editor only!
    ///</summary>
    public static GameObject SetFocus(GameObject From)
    {
        return Selection.activeGameObject = From;
    }

    ///<summary>
    ///Caution: Unity Editor only!
    ///</summary>
    public static GameObject GetFocus()
    {
        return Selection.activeGameObject;
    }

#endif

    #endregion
}

public class QComponent
{
    #region ==================================== Primary

    public static T GetComponent<T>(GameObject From) where T : Component
    {
        //Get Component from GameObject. If null, Add Component to GameObject.
        //
        if (From == null)
            return default(T);
        //
        if (From.GetComponent<T>() == null)
            return From.AddComponent<T>();
        else
            return From.GetComponent<T>();
    }

    public static T GetComponent<T>(Transform From) where T : Component
    {
        //Get Component from GameObject. If null, Add Component to GameObject.
        //
        if (From == null)
            return default(T);
        //
        if (From.GetComponent<T>() == null)
            return From.gameObject.AddComponent<T>();
        else
            return From.gameObject.GetComponent<T>();
    }

    public static T GetComponent<T>(Component From) where T : Component
    {
        //Get Component from GameObject. If null, Add Component to GameObject.
        //
        if (From == null)
            return default(T);
        //
        if (From.GetComponent<T>() == null)
            return From.gameObject.AddComponent<T>();
        else
            return From.gameObject.GetComponent<T>();
    }

    #endregion

    #region ==================================== Button

    public static void SetButton(Button From, UnityAction Action)
    {
        //Add an void methode to Action!!

        //Caution: Some version of Unity might not run this after build app!!

        From.onClick.AddListener(Action);
    }

    #endregion
}