using System.Collections;
using UnityEngine;
using UnityEditor.PackageManager;
using System.Linq;
using System.Collections.Generic;
using System;

public class PackageManager : MonoBehaviour
{
    //NOTE: To get Package Name, go to Window\Package Manager to see them!!
    [SerializeField] private List<string> m_package = new List<string>() { "com.unity.ide.vscode", };

    public static Action onPackageSucess;
    public static Action onPackageFail;

    private IEnumerator Start()
    {
        var pack = Client.List();
        //
        while (!pack.IsCompleted) yield return null;
        //
        bool Missed = false;
        foreach(string Package in m_package)
        {
            var haveProgrids = pack.Result.FirstOrDefault(q => q.name == Package);
            if (haveProgrids == null)
            {
                Missed = true;
                Debug.LogFormat("[PackageManager] Missing {0}", Package);
            }
        }
        //
        if (Missed)
            onPackageFail?.Invoke();
        else
            onPackageSucess?.Invoke();
    }
}