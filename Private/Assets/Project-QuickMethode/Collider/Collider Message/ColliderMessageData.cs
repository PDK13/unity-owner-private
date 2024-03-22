using UnityEngine;

public class ColliderMessageData
{
    public string Code;
    public GameObject Target;

    public ColliderMessageData(string Code, GameObject target)
    {
        this.Code = Code;
        this.Target = target;
    }
}