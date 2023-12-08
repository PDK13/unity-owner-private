using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextAssets : MonoBehaviour
{
    [ContextMenu("aa")]
    private void aaaa()
    {
        QUnityAssets.SetCreateFolder("aa", "bb", "cc", "dd");
        QUnityAssets.SetCreateFolder("aa", "bb", "dd");
    }
}
