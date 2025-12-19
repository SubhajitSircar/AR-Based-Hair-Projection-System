using UnityEngine;
using UnityEngine.EventSystems;

public class ARScale : MonoBehaviour
{
    public float pinchScaleSpeed = 0.005f;
    public float minScale = 0.3f;
    public float maxScale = 2.5f;

    private float prevDistance;
    private bool isPinching;

    void Update()
    {
        if (Input.touchCount < 2)
        {
            isPinching = false;
            return;
        }

        if (EventSystem.current != null &&
            (EventSystem.current.IsPointerOverGameObject(0) ||
             EventSystem.current.IsPointerOverGameObject(1)))
            return;

        HandlePinch();
    }

    void HandlePinch()
    {
        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        if (!isPinching)
        {
            prevDistance = Vector2.Distance(t0.position, t1.position);
            isPinching = true;
            return;
        }

        float currentDistance = Vector2.Distance(t0.position, t1.position);
        float delta = currentDistance - prevDistance;
        prevDistance = currentDistance;

        float scaleFactor = 1 + (delta * pinchScaleSpeed);
        Vector3 newScale = transform.localScale * scaleFactor;

        float clamped = Mathf.Clamp(newScale.x, minScale, maxScale);
        transform.localScale = new Vector3(clamped, clamped, clamped);
    }
}
