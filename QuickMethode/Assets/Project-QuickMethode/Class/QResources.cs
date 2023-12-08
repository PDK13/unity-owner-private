using System.Collections.Generic;
using UnityEngine;

public class QResources
{
    //NOTE:
    //Folder(s) "Resources" can be created everywhere from root "Assests/*", that can be access by Unity or Application

    //BEWARD:
    //All content(s) in folder(s) "Resources" will be builded to Application, even they ightn't be used in Build-Game Application

    #region ==================================== Prefab

    public static List<GameObject> GetPrefab(params string[] PathChildInResources)
    {
        string PathInResources = QPath.GetPath(QPath.PathType.None, PathChildInResources);
        GameObject[] LoadArray = Resources.LoadAll<GameObject>(PathInResources);
        List<GameObject> LoadList = new List<GameObject>();
        LoadList.AddRange(LoadArray);
        return LoadList;
    }

    #endregion

    #region ==================================== Sprite

    public static List<Sprite> GetSprite(params string[] PathChildInResources)
    {
        string PathInResources = QPath.GetPath(QPath.PathType.None, PathChildInResources);
        Sprite[] LoadArray = Resources.LoadAll<Sprite>(PathInResources);
        List<Sprite> LoadList = new List<Sprite>();
        LoadList.AddRange(LoadArray);
        return LoadList;
    }

    #endregion

    #region ==================================== Text Asset

    public static List<TextAsset> GetTextAsset(params string[] PathChildInResources)
    {
        string PathInResources = QPath.GetPath(QPath.PathType.None, PathChildInResources);
        TextAsset[] LoadArray = Resources.LoadAll<TextAsset>(PathInResources);
        List<TextAsset> LoadList = new List<TextAsset>();
        LoadList.AddRange(LoadArray);
        return LoadList;
    }

    #endregion
}