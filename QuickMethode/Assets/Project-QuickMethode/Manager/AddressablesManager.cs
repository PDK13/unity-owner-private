using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AddressablesManager : MonoBehaviour
{
    public static AddressablesManager Instance;

    //Varible: Scene

    private AsyncOperationHandle<SceneInstance> m_sceneLoaded;
    private bool m_sceneReady = true;

    public Action onSceneLoad;
    public Action onSceneActive;

    //Varible: GameObject

    private List<GameObject> m_gameObject = new List<GameObject>();
    private List<AsyncOperationHandle<GameObject>> m_gameObjectAsync = new List<AsyncOperationHandle<GameObject>>();

    public Action<GameObject> onGameObjectCreate;
    public Action<GameObject> onGameObjectDestroy;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //
        Instance = this;
    }

    public void SetLoad<T>(AssetReference m_asset, T Result)
    {
        AsyncOperationHandle<T> AsyncHandle = m_asset.LoadAssetAsync<T>();
        //
        AsyncHandle.Completed += ((AsyncOperationHandle<T> Handle) =>
        {
            Result = Handle.Result;
        });
    }

    //Scene

    public void SetSceneLoad(string TagOrPath, bool ActiveOnLoad = true)
    {
        if (!m_sceneReady)
            return;
        m_sceneReady = false;
        //
        //TAG: If Scene aldready got it Tag, then just called it with that Tag
        //PATH: If Scene have not Tag, then called it by Path start from "Assets\.."
        m_sceneLoaded = Addressables.LoadSceneAsync(TagOrPath, UnityEngine.SceneManagement.LoadSceneMode.Single, false);
        onSceneLoad?.Invoke();
        //
        if (ActiveOnLoad)
            SetSceneActive();
    }

    public void SetSceneActive()
    {
        m_sceneLoaded.Completed += ((AsyncOperationHandle<SceneInstance> Handle) =>
        {
            m_sceneReady = true;
            //
            m_sceneLoaded.Result.ActivateAsync();
            onSceneActive?.Invoke();
        });
    }

    //GameObject

    public AsyncOperationHandle<GameObject> SetGameObjectCreate(AssetReference Prefab)
    {
        AsyncOperationHandle<GameObject> AsyncHandle = Prefab.InstantiateAsync();
        //
        AsyncHandle.Completed += ((AsyncOperationHandle<GameObject> Handle) => 
        {
            m_gameObject.Add(AsyncHandle.Result);
            m_gameObjectAsync.Add(AsyncHandle);
            onGameObjectCreate?.Invoke(AsyncHandle.Result);
        });
        //
        return AsyncHandle;
    }

    public void SetGameObjectDestroy(AsyncOperationHandle<GameObject> GameObject)
    {
        onGameObjectDestroy?.Invoke(GameObject.Result);
        m_gameObject.RemoveAt(m_gameObjectAsync.IndexOf(GameObject));
        m_gameObjectAsync.Remove(GameObject);
        //
        Addressables.Release(GameObject);
    }

    public bool SetGameObjectDestroy(GameObject GameObject)
    {
        int Index = m_gameObject.IndexOf(GameObject);
        //
        if (Index < 0)
            return false;
        //
        Addressables.Release(m_gameObjectAsync[Index]);
        //
        m_gameObjectAsync.RemoveAt(Index);
        m_gameObject.Remove(GameObject);
        onGameObjectDestroy?.Invoke(GameObject);
        //
        return true;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(AddressablesManager))]
public class AddressablesManagerEditor : Editor
{
    private AddressablesManager m_target;

    //private SerializedProperty m_sceneReady;
    //private SerializedProperty m_gameObject;

    private void OnEnable()
    {
        m_target = target as AddressablesManager;
        //
        //m_sceneReady = serializedObject.FindProperty("m_sceneReady");
        //m_gameObject = serializedObject.FindProperty("m_gameObject");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        //serializedObject.Update();
        //
        //EditorGUILayout.PropertyField(m_sceneReady);
        //EditorGUILayout.PropertyField(m_gameObject);
        //
        //serializedObject.ApplyModifiedProperties();
    }
}

#endif