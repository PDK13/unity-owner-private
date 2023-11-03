using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private void Start()
    {
        var Character = new Dictionary<string, GameObject>();

        var PrefabLoad = AddressablesManager.Instance.SetAssetsLoad<GameObject>("myPrefab");
    }
}