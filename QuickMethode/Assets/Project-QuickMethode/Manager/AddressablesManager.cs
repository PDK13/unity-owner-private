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

    public AsyncOperationHandle<T> SetAssetsLoad<T>(string TagOrPath)
    {
        return Addressables.LoadAssetAsync<T>(TagOrPath);
    }

    //Scene

    public AsyncOperationHandle<SceneInstance> SetSceneLoad(AssetReference Asset, bool ActiveOnLoad = false)
    {
        return Asset.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Single, ActiveOnLoad);
    }

    public AsyncOperationHandle<SceneInstance> SetSceneLoad(string TagOrPath, bool ActiveOnLoad = false)
    {
        return Addressables.LoadSceneAsync(TagOrPath, UnityEngine.SceneManagement.LoadSceneMode.Single, ActiveOnLoad);
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

    public AsyncOperationHandle<GameObject> SetPrefabInstantiate(string TagOrPath)
    {
        return Addressables.InstantiateAsync(TagOrPath);
    }

    public void SetPrefabRelease(AsyncOperationHandle<GameObject> Prefab)
    {
        Prefab.Completed += (AsyncOperationHandle<GameObject> Handle) => Addressables.Release(Prefab);
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