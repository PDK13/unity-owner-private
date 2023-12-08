using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QPath
{
    #region ==================================== Path Get

    public enum PathType 
    { 
        None, 
        Persistent, 
        Assets, 
        Resources, 
        Document, 
        Picture, 
        Music, 
        Video, 
    }

    public static string GetPath(PathType PathType, params string[] PathChild)
    {
        string PathFinal = "";

        switch (PathType)
        {
            case PathType.Persistent:
                PathFinal = Application.persistentDataPath;
                break;
            case PathType.Assets:
                PathFinal = Application.dataPath;
                break;
            case PathType.Resources:
                PathFinal = Application.dataPath + @"/resources";
                break;
            case PathType.Document:
                PathFinal = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                break;
            case PathType.Picture:
                PathFinal = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                break;
            case PathType.Music:
                PathFinal = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                break;
            case PathType.Video:
                PathFinal = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                break;
        }
        //
        foreach (string PathChildAdd in PathChild)
            PathFinal = QEncypt.GetEncyptAdd('/', PathFinal, PathChildAdd);
        //
        return PathFinal;
    }

    #endregion

    #region ==================================== File

    //Name

    public static string GetFileName(string PathPrimary)
    {
        return Path.GetFileNameWithoutExtension(PathPrimary);
    }

    public static string GetFileName(PathType PathType, params string[] PathChild)
    {
        return GetFileName(GetPath(PathType, PathChild));
    }

    //Extension

    public static string GetFileExtension(string PathPrimary)
    {
        return Path.GetExtension(PathPrimary);
    }

    public static string GetFileExtension(PathType PathType, params string[] PathChild)
    {
        return GetFileExtension(GetPath(PathType, PathChild));
    }

    #endregion

    #region ==================================== Path Pannel

#if UNITY_EDITOR

    //Open

    ///<summary>
    ///Caution: Unity Editor only!
    ///</summary>
    public static (bool Result, string Path, string Name) GetPathFolderOpenPanel(string Title, string PathPrimary = "")
    {
        string Path = EditorUtility.OpenFolderPanel(Title, (PathPrimary == "") ? GetPath(PathType.Assets) : PathPrimary, "");
        List<string> PathDencypt = QEncypt.GetDencyptString('/', Path);
        return (Path != "", Path, (PathDencypt.Count > 0) ? PathDencypt[PathDencypt.Count - 1] : "");
    }

    ///<summary>
    ///Caution: Unity Editor only!
    ///</summary>
    public static (bool Result, string Path, string Name) GetPathFileOpenPanel(string Title, string Extension, string PathPrimary = "")
    {
        string Path = EditorUtility.OpenFilePanel(Title, (PathPrimary == "") ? GetPath(PathType.Assets) : PathPrimary, Extension);
        List<string> PathDencypt = QEncypt.GetDencyptString('/', Path);
        return (Path != "", Path, (PathDencypt.Count > 0) ? PathDencypt[PathDencypt.Count - 1] : "");
    }

    //Save

    ///<summary>
    ///Caution: Unity Editor only!
    ///</summary>
    public static (bool Result, string Path, string Name) GetPathFolderSavePanel(string Title, string PathPrimary = "")
    {
        string Path = EditorUtility.SaveFolderPanel(Title, (PathPrimary == "") ? GetPath(PathType.Assets) : PathPrimary, "");
        List<string> PathDencypt = QEncypt.GetDencyptString('/', Path);
        return (Path != "", Path, (PathDencypt.Count > 0) ? PathDencypt[PathDencypt.Count - 1] : "");
    }

    ///<summary>
    ///Caution: Unity Editor only!
    ///</summary>
    public static (bool Result, string Path, string Name) GetPathFileSavePanel(string Title, string Extension, string PathPrimary = "")
    {
        string Path = EditorUtility.SaveFilePanel(Title, (PathPrimary == "") ? GetPath(PathType.Assets) : PathPrimary, "", Extension);
        List<string> PathDencypt = QEncypt.GetDencyptString('/', Path);
        return (Path != "", Path, (PathDencypt.Count > 0) ? PathDencypt[PathDencypt.Count - 1] : "");
    }

#endif

    #endregion

    #region ==================================== Path Exist

    //File

    public static bool GetPathFileExist(string PathFile)
    {
        return File.Exists(PathFile);
    }

    public static bool GetPathFileExist(PathType PathType, params string[] PathChild)
    {
        return File.Exists(GetPath(PathType, PathChild));
    }

    //Folder

    public static bool GetPathFolderExist(string PathFolder)
    {
        return Directory.Exists(PathFolder);
    }

    public static bool GetPathFolderExist(PathType PathType, params string[] PathChild)
    {
        return Directory.Exists(GetPath(PathType, PathChild));
    }

    #endregion
}

public class QFileIO
{
    #region ==================================== File IO Write 

    public List<string> DataWriteQueue { private set; get; } = new List<string>();

    public string DataWriteText { private set; get; } = "";

    #region ------------------------------------ Write Start

    public void SetWriteStart(string Path)
    {
        SetWriteToFile(Path, DataWriteText);
    } //Call Last

    private void SetWriteToFile(string Path, string Data)
    {
        try
        {
            using (FileStream Stream = File.Create(Path))
            {
                byte[] Info = new UTF8Encoding(true).GetBytes(Data);
                Stream.Write(Info, 0, Info.Length);
            }
        }
        catch
        {
            Debug.LogErrorFormat("[Error] File Write Fail: {0}", Path);
        }
    }

    public void SetWriteClear()
    {
        DataWriteText = "";
        DataWriteQueue.Clear();
    }

    #endregion

    #region ------------------------------------ Write Add

    public void SetWriteAdd()
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += "";
        DataWriteQueue.Add("");
    }

    //Primary

    public void SetWriteAdd(string Value)
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += Value;
        DataWriteQueue.Add(Value);
    }

    public void SetWriteAdd(int Value)
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += Value;
        DataWriteQueue.Add(Value.ToString());
    }

    public void SetWriteAdd(float Value)
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += Value;
        DataWriteQueue.Add(Value.ToString());
    }

    public void SetWriteAdd(double Value)
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += Value;
        DataWriteQueue.Add(Value.ToString());
    }

    public void SetWriteAdd(bool Value)
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += Value.ToString().ToLower();
        DataWriteQueue.Add(Value.ToString().ToLower());
    }

    public void SetWriteAdd<T>(T Value) where T : Enum
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += QEnum.GetChoice(Value).ToString();
        DataWriteQueue.Add(QEnum.GetChoice(Value).ToString());
    }

    //Params & List

    public void SetWriteAdd(char Key, params string[] Value)
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += QEncypt.GetEncypt(Key, Value.ToList());
        DataWriteQueue.Add(QEncypt.GetEncypt(Key, Value.ToList()));
    }

    public void SetWriteAdd(char Key, params int[] Value)
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += QEncypt.GetEncypt(Key, Value.ToList());
        DataWriteQueue.Add(QEncypt.GetEncypt(Key, Value.ToList()));
    }

    public void SetWriteAdd(char Key, params float[] Value)
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += QEncypt.GetEncypt(Key, Value.ToList());
        DataWriteQueue.Add(QEncypt.GetEncypt(Key, Value.ToList()));
    }

    public void SetWriteAdd(char Key, params bool[] Value)
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += QEncypt.GetEncypt(Key, Value.ToList());
        DataWriteQueue.Add(QEncypt.GetEncypt(Key, Value.ToList()));
    }

    public void SetWriteAdd<T>(char Key, params T[] Value) where T : Enum
    {
        if (DataWriteText.Length != 0)
            DataWriteText += "\n";
        DataWriteText += QEncypt.GetEncypt(Key, Value.ToList());
        DataWriteQueue.Add(QEncypt.GetEncypt(Key, Value.ToList()));
    }

    //Vector

    public void SetWriteAdd(char Key, Vector2 Value)
    {
        SetWriteAdd(QEncypt.GetEncyptVector2(Key, Value));
    }

    public void SetWriteAdd(char Key, Vector2Int Value)
    {
        SetWriteAdd(QEncypt.GetEncyptVector2Int(Key, Value));
    }

    public void SetWriteAdd(char Key, Vector3 Value)
    {
        SetWriteAdd(QEncypt.GetEncyptVector3(Key, Value));
    }

    public void SetWriteAdd(char Key, Vector3Int Value)
    {
        SetWriteAdd(QEncypt.GetEncyptVector3Int(Key, Value));
    }

    #endregion

    #endregion

    #region ==================================== File IO Read

    public List<string> DataReadQueue { private set; get; } = new List<string>();

    public int RunRead { private set; get; } = -1;

    public bool RunReadEnd => RunRead >= DataReadQueue.Count - 1;

    #region ------------------------------------ Read Start

    public void SetReadStart(string Path)
    {
        DataReadQueue = GetReadFromFile(Path);
    } //Call First

    private List<string> GetReadFromFile(string Path)
    {
        try
        {
            List<string> TextRead = new List<string>();
            using (StreamReader Stream = File.OpenText(Path))
            {
                string ReadRun = "";
                while ((ReadRun = Stream.ReadLine()) != null)
                {
                    TextRead.Add(ReadRun);
                }
            }
            return TextRead;
        }
        catch
        {
            Debug.LogErrorFormat("[Error] File Read Fail: {0}", Path);
            return null;
        }
    }

    public void SetReadStart(TextAsset FileTest)
    {
        DataReadQueue = GetReadFromFile(FileTest);
    } //Call First

    private List<string> GetReadFromFile(TextAsset FileTest)
    {
        try
        {
            string ReadRun = FileTest.text.Replace("\r\n", "\n");
            List<string> TextRead = QEncypt.GetDencyptString('\n', ReadRun);
            return TextRead;
        }
        catch
        {
            Debug.LogErrorFormat("[Error] File Read Fail: {0}", FileTest.name);
            return null;
        }
    }

    public void SetReadClear()
    {
        DataReadQueue = new List<string>();
        RunRead = -1;
    }

    public void SetReadReset()
    {
        RunRead = -1;
    }

    #endregion

    #region ------------------------------------ Read Auto

    //Emty

    public void GetReadAuto()
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return;
        RunRead++;
    }

    //Primary

    public string GetReadAutoString()
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return "";
        RunRead++;
        return DataReadQueue[RunRead];
    }

    public int GetReadAutoInt()
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return 0;
        RunRead++;
        return int.Parse(DataReadQueue[RunRead]);
    }

    public float GetReadAutoFloat()
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return 0f;
        RunRead++;
        return float.Parse(DataReadQueue[RunRead]);
    }

    public double GetReadAutoDouble()
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return 0f;
        RunRead++;
        return double.Parse(DataReadQueue[RunRead]);
    }

    public bool GetReadAutoBool()
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return false;
        RunRead++;
        return DataReadQueue[RunRead].ToLower() == "true";
    }

    public T GetReadAutoEnum<T>() where T : Enum
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return QEnum.GetChoice<T>(0);
        RunRead++;
        return QEnum.GetChoice<T>(int.Parse(DataReadQueue[RunRead]));
    }

    //Params & List

    public List<string> GetReadAutoString(char Key)
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return new List<string>();
        RunRead++;
        return QEncypt.GetDencyptString(Key, DataReadQueue[RunRead]);
    }

    public List<int> GetReadAutoInt(char Key)
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return new List<int>();
        RunRead++;
        return QEncypt.GetDencyptInt(Key, DataReadQueue[RunRead]);
    }

    public List<float> GetReadAutoFloat(char Key)
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return new List<float>();
        RunRead++;
        return QEncypt.GetDencyptFloat(Key, DataReadQueue[RunRead]);
    }

    public List<bool> GetReadAutoBool(char Key)
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return new List<bool>();
        RunRead++;
        return QEncypt.GetDencyptBool(Key, DataReadQueue[RunRead]);
    }

    public List<T> GetReadAutoString<T>(char Key) where T : Enum
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return new List<T>();
        RunRead++;
        return QEncypt.GetDencyptEnum<T>(Key, DataReadQueue[RunRead]);
    }

    //Vector

    public Vector2 GetReadAutoVector2(char Key)
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return new Vector2();
        RunRead++;
        return QEncypt.GetDencyptVector2(Key, DataReadQueue[RunRead]);
    }

    public Vector2Int GetReadAutoVector2Int(char Key)
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return new Vector2Int();
        RunRead++;
        return QEncypt.GetDencyptVector2Int(Key, DataReadQueue[RunRead]);
    }

    public Vector3 GetReadAutoVector3(char Key)
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return new Vector3();
        RunRead++;
        return QEncypt.GetDencyptVector3(Key, DataReadQueue[RunRead]);
    }

    public Vector3Int GetReadAutoVector3Int(char Key)
    {
        if (RunRead >= DataReadQueue.Count - 1)
            return new Vector3Int();
        RunRead++;
        return QEncypt.GetDencyptVector3Int(Key, DataReadQueue[RunRead]);
    }

    #endregion

    #endregion
}

//IMPORTANCE: This JSON class still can't handle with big-size data!!
public class QJSON
{
    //NOTE:
    //Type "TextAsset" is a "Text Document" File or "*.txt" File

    //SAMPLE:
    //ClassData Data = ClassFileIO.GetDatafromJson<ClassData>(JsonDataTextDocument);

    #region ==================================== Path

    public static void SetDataPath(object Data, string Path)
    {
        string JsonData = JsonUtility.ToJson(Data, true);
        //
        QFileIO FileIO = new QFileIO();
        FileIO.SetWriteAdd(JsonData);
        FileIO.SetWriteStart(Path);
    }

    public static void SetDataPath<ClassData>(ClassData Data, string Path)
    {
        string JsonData = JsonUtility.ToJson(Data, true);
        //
        QFileIO FileIO = new QFileIO();
        FileIO.SetWriteAdd(JsonData);
        FileIO.SetWriteStart(Path);
    }

    public static ClassData GetDataPath<ClassData>(string Path)
    {
        QFileIO FileIO = new QFileIO();
        FileIO.SetReadStart(Path);
        List<string> JSonRead = FileIO.DataReadQueue;
        //
        string JsonData = "";
        for (int i = 0; i < JSonRead.Count; i++)
        {
            JsonData += (FileIO.GetReadAutoString() + "\n");
        }
        //
        return JsonUtility.FromJson<ClassData>(JsonData);
    }

    #endregion

    #region ==================================== Primary

    public static string GetDataConvertJson(object JsonDataClass)
    {
        return JsonUtility.ToJson(JsonDataClass);
    }

    public static ClassData GetDataConvertClass<ClassData>(TextAsset JsonDataTextDocument)
    {
        return GetDataConvertClass<ClassData>(JsonDataTextDocument.text);
    }

    public static ClassData GetDataConvertClass<ClassData>(string JsonData)
    {
        return JsonUtility.FromJson<ClassData>(JsonData);
    }

    #endregion
}