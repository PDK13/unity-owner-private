using UnityEngine;

public class ColliderMessageData
{
    public string Tag;
    public GameObject Target;

    public ColliderMessageData(string Tag, GameObject Target)
    {
        this.Tag = Tag;
        this.Target = Target;
    }
}