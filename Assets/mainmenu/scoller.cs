using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    public RawImage image;
    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.1f;

    void Update()
    {
        Rect uvRect = image.uvRect;
        uvRect.x += scrollSpeedX * Time.deltaTime;
        uvRect.y += scrollSpeedY * Time.deltaTime;
        image.uvRect = uvRect;
    }
}
