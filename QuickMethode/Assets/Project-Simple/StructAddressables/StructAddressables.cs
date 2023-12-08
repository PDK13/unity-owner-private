using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StructAddressables : MonoBehaviour
{
    [SerializeField] private GameObject m_loadPrefab;
    [SerializeField] private Sprite m_loadSprite;

    [Space]
    [SerializeField] private AssetLabelReference m_labelReferencePrefab;
    [SerializeField] private GameObject m_instantiatePrefab;

    [Space]
    [SerializeField] private AssetLabelReference m_labelReferenceSame;
    [SerializeField] private List<Sprite> m_loadSpriteSame;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        //
        Debug.Log("[Debug] Start Loading...");
        //
        var PrefabLoad = QAddressables.SetAssetsLoad<GameObject>("myPrefab");
        yield return PrefabLoad;
        m_loadPrefab = PrefabLoad.Result;
        if (m_loadPrefab != null)
            Debug.Log("[Debug] Load Prefab Complete..."); //Will get here!!
        else
            Debug.Log("[Debug] Load Prefab Un-Complete...");
        //
        Debug.Log("------------------------");
        yield return new WaitForSeconds(3f); //Rest a bit before new run testing!!
        //
        QAddressables.SetAssetsLoad<Sprite>("mySprite").Completed += (Handle) => m_loadSprite = Handle.Result;
        if (m_loadSprite != null)
            Debug.Log("[Debug] Load Sprite Complete...");
        else
            Debug.Log("[Debug] Load Sprite Un-Complete..."); //Will get here!!
        //
        yield return new WaitForEndOfFrame();
        //
        Debug.Log("[Debug] End Loading...");
        //
        Debug.Log("------------------------");
        yield return new WaitForSeconds(3f); //Rest a bit before new run testing!!
        //
        Debug.Log("[Debug] Start Instantiate...");
        //
        var PrefabInstantiate = QAddressables.SetPrefabInstantiate(m_labelReferencePrefab.labelString);
        yield return PrefabInstantiate;
        m_instantiatePrefab = PrefabInstantiate.Result.gameObject;
        if (m_instantiatePrefab != null)
            Debug.Log("[Debug] Instantiate Prefab Complete..."); //Will get here!!
        else
            Debug.Log("[Debug] Instantiate Prefab Un-Complete...");
        //
        Debug.Log("[Debug] End Instantiate...");
        //
        Debug.Log("------------------------");
        yield return new WaitForSeconds(3f); //Rest a bit before new run testing!!
        //
        Debug.Log("[Debug] Start Release...");
        //
        QAddressables.SetPrefabRelease(PrefabInstantiate);
        if (m_instantiatePrefab == null)
            Debug.Log("[Debug] Release Prefab Complete...");
        else
            Debug.Log("[Debug] Release Prefab Un-Complete..."); //Will get here!!
        //
        Debug.Log("[Debug] End Release...");
        //
        Debug.Log("------------------------");
        yield return new WaitForSeconds(3f); //Rest a bit before new run testing!!
        //
        Debug.Log("[Debug] Start Load Same...");
        //
        var SpriteSame = QAddressables.SetAssetsLoadList<Sprite>(m_labelReferenceSame.labelString);
        yield return SpriteSame;
        m_loadSpriteSame = SpriteSame.Result.ToList();
        //
        Debug.Log("[Debug] End Load Same...");
    }
}