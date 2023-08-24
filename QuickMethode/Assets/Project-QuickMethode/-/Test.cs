using UnityEngine;

public class Test : MonoBehaviour
{
    public class MyClass
    {
        public int a = 0;
        public int b = 0;
    }

    private void Start()
    {
        MyClass Data = new MyClass();

        //QJSON.SetData(Data, QPath.GetPath(QPath.PathType.Assets, "data.json"));

        Data = QJSON.GetDataPath<MyClass>(QPath.GetPath(QPath.PathType.Assets, "data.json"));
    }
}