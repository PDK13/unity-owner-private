using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SimpleAddressables : MonoBehaviour
{
    [SerializeField] private GameObject m_loadPrefab;
    [SerializeField] private Sprite m_loadSprite;
    [SerializeField] private GameObject m_instantiatePrefab;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        //
        Debug.Log("[Debug] Start Loading...");
        //
        var PrefabLoad = AddressablesManager.Instance.SetAssetsLoad<GameObject>("myPrefab");
        yield return PrefabLoad;
        m_loadPrefab = PrefabLoad.Result;
        if (m_loadPrefab != null)
            Debug.Log("[Debug] Load Prefab Complete..."); //Will get here!!
        else
            Debug.Log("[Debug] Load Prefab Un-Complete...");
        //
        yield return new WaitForEndOfFrame();
        //
        AddressablesManager.Instance.SetAssetsLoad<Sprite>("mySprite").Completed += (AsyncOperationHandle<Sprite> Handle) => m_loadSprite = Handle.Result;
        if (m_loadSprite != null)
            Debug.Log("[Debug] Load Sprite Complete...");
        else
            Debug.Log("[Debug] Load Sprite Un-Complete..."); //Will get here!!
        //
        yield return new WaitForEndOfFrame();
        //
        Debug.Log("[Debug] End Loading...");
        //
        yield return new WaitForEndOfFrame();
        //
        Debug.Log("[Debug] Start Instantiate...");
        //
        var PrefabInstantiate = AddressablesManager.Instance.SetPrefabInstantiate("myPrefab");
        yield return PrefabInstantiate;
        m_instantiatePrefab = PrefabInstantiate.Result.gameObject;
        if (m_instantiatePrefab != null)
            Debug.Log("[Debug] Instantiate Prefab Complete..."); //Will get here!!
        else
            Debug.Log("[Debug] Instantiate Prefab Un-Complete...");
        //
        yield return new WaitForSeconds(3);
        //
        AddressablesManager.Instance.SetPrefabRelease(PrefabInstantiate);
        if (m_instantiatePrefab == null)
            Debug.Log("[Debug] Release Prefab Complete..."); 
        else
            Debug.Log("[Debug] Release Prefab Un-Complete..."); //Will get here!!
        //
        Debug.Log("[Debug] End Instantiate...");
        //
    }
}