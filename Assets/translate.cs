using UnityEngine;
using UnityEngine.EventSystems;

public class ARDragTranslate : MonoBehaviour
{
    private Camera mainCam;

    [Tooltip("1 = normal speed, <1 = slower, >1 = faster")]
    public float dragSpeed = 0.5f;

    private bool isDragging = false;
    private Vector2 dragStartTouchPos;
    private Vector3 dragStartObjPos;
    private float dragDistanceFromCamera;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        // We only handle ONE finger for drag
        if (Input.touchCount != 1)
        {
            isDragging = false;
            return;
        }

        Touch t = Input.GetTouch(0);

        // Ignore if over UI
        if (IsTouchOverUI(t.fingerId))
            return;

        if (t.phase == TouchPhase.Began)
        {
            isDragging = true;
            dragStartTouchPos = t.position;
            dragStartObjPos = transform.position;

            // Distance from camera to object (for consistent movement)
            dragDistanceFromCamera = Vector3.Distance(mainCam.transform.position, transform.position);
        }
        else if (t.phase == TouchPhase.Moved && isDragging)
        {
            Vector2 currentTouchPos = t.position;

            // Convert the start & current touch positions to world points
            Vector3 worldStart = mainCam.ScreenToWorldPoint(
                new Vector3(dragStartTouchPos.x, dragStartTouchPos.y, dragDistanceFromCamera));

            Vector3 worldCurrent = mainCam.ScreenToWorldPoint(
                new Vector3(currentTouchPos.x, currentTouchPos.y, dragDistanceFromCamera));

            // World-space delta = how much finger moved on screen, mapped to world
            Vector3 worldDelta = (worldCurrent - worldStart) * dragSpeed;

            // New position = original position + delta
            transform.position = dragStartObjPos + worldDelta;
        }
        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
        {
            isDragging = false;
        }
    }

    bool IsTouchOverUI(int fingerId)
    {
        if (EventSystem.current == null)
            return false;

        return EventSystem.current.IsPointerOverGameObject(fingerId);
    }
}
