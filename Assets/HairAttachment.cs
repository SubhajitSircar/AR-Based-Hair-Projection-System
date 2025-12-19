using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFace))]
public class HairAttachment : MonoBehaviour
{
    public GameObject hairPrefab;
    [Range(0f, 0.3f)] public float baseYOffset = 0.15f;   // manual lift above head
    [Range(0f, 1f)] public float smooth = 0.2f;           // lower is smoother

    private GameObject hairInstance;
    private Vector3 targetLocalPos;
    private Vector3 vel;

    void Start()
    {
        if (hairPrefab == null) return;
        hairInstance = Instantiate(hairPrefab, transform);
        targetLocalPos = new Vector3(0, baseYOffset, 0);
        hairInstance.transform.localPosition = targetLocalPos;
        hairInstance.transform.localRotation = Quaternion.identity;
    }

    // Called by OpenCV script to nudge Y dynamically
    public void UpdateForeheadOffset01(float normY)
    {
        // normY is 0..1 image space (0=top). Map it to a small Y lift range.
        float y = Mathf.Lerp(0.12f, 0.19f, 1f - normY);
        targetLocalPos = new Vector3(0, y, 0);
    }

    void LateUpdate()
    {
        if (hairInstance == null) return;

        // Blend OpenCV offset with base offset
        var desired = new Vector3(0, Mathf.Max(targetLocalPos.y, baseYOffset), 0);
        hairInstance.transform.localPosition =
            Vector3.SmoothDamp(hairInstance.transform.localPosition, desired, ref vel, smooth);
        hairInstance.transform.localRotation = Quaternion.identity; // keep upright
    }
}
