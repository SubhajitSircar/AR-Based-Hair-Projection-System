using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform rectTransform;
    public float padding = 0.01f; // Adjust this value to increase or decrease the padding
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        Rect safe = Screen.safeArea;

        Vector2 min = safe.position;
        Vector2 max = safe.position + safe.size;

        min.x /= Screen.width;
        min.y /= Screen.height;
        max.x /= Screen.width;
        max.y /= Screen.height;

        //adds padding 

        min.x += padding;
        min.y += padding;
        max.x -= padding;
        max.y -= padding;

        rectTransform.anchorMin = min;
        rectTransform.anchorMax = max;
    }
}