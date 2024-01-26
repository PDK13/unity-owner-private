using UnityEngine;

public class CanvasWorldPoint : MonoBehaviour
{
    private void aaa(Vector3? WorldPos)
    {
        Camera cam = transform.root.GetComponent<Canvas>().rootCanvas.worldCamera;
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(WorldPos.Value);
        Vector2 pos = cam.ViewportToWorldPoint(viewportPoint);
    }
}
