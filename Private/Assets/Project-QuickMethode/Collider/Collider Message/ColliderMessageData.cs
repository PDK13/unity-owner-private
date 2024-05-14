using UnityEngine;

public class ColliderMessageData
{
    public string Tag;
    public GameObject Target;

    public ColliderMessageData(string Tag, GameObject target)
    {
        this.Tag = Tag;
        this.Target = target;
    }
}