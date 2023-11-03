using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AddressablesManager : MonoBehaviour
{
    public static AddressablesManager Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //
        Instance = this;
    }

    //Assets

    public AsyncOperationHandle<T> SetAssetsLoad<T>(AssetReference Asset)
    {
        return Asset.LoadAssetAsync<T>();
    }

    public AsyncOperationHandle<T> SetAssetsLoad<T>(string LabelOrPath)
    {
        return Addressables.LoadAssetAsync<T>(LabelOrPath);
    }

    public AsyncOperationHandle<IList<T>> SetAssetsLoadList<T>(string Label)
    {
        return Addressables.LoadAssetsAsync<T>(Label, null);
    }

    public void SetAssetsRelease<T>(AsyncOperationHandle<T> Asset)
    {
        Asset.Completed += (AsyncOperationHandle<T> Handle) => Addressables.Release(Asset);
    }

    //Scene

    public AsyncOperationHandle<SceneInstance> SetSceneLoad(AssetReference Asset, bool ActiveOnLoad = false)
    {
        return Asset.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Single, ActiveOnLoad);
    }

    public AsyncOperationHandle<SceneInstance> SetSceneLoad(string LabelOrPath, bool ActiveOnLoad = false)
    {
        return Addressables.LoadSceneAsync(LabelOrPath, UnityEngine.SceneManagement.LoadSceneMode.Single, ActiveOnLoad);
    }

    public void SetSceneActive(AsyncOperationHandle<SceneInstance> Scene)
    {
        Scene.Completed += (AsyncOperationHandle<SceneInstance> Handle) => Scene.Result.ActivateAsync();
    }

    //GameObject

    public AsyncOperationHandle<GameObject> SetPrefabInstantiate(AssetReference Asset)
    {
        return Asset.InstantiateAsync();
    }

    public AsyncOperationHandle<GameObject> SetPrefabInstantiate(string LabelOrPath)
    {
        return Addressables.InstantiateAsync(LabelOrPath);
    }

    public void SetPrefabRelease(AsyncOperationHandle<GameObject> Prefab)
    {
        Prefab.Completed += (AsyncOperationHandle<GameObject> Handle) => Addressables.ReleaseInstance(Prefab);
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