using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARFoundationFaceTracker : MonoBehaviour
{
    private ARFaceManager faceManager;
    private ARFace face;

    private HairAttachment hair;

    // Forehead vertex index (ARKit face mesh index)
    // This is a commonly-used approximate forehead index.
    private const int ForeheadVertexIndex = 10;

    void Awake()
    {
        faceManager = FindObjectOfType<ARFaceManager>();
        faceManager.facesChanged += OnFacesChanged;
    }

    void OnDestroy()
    {
        if (faceManager != null)
            faceManager.facesChanged -= OnFacesChanged;
    }

    private void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        // Use first detected face
        if (args.added.Count > 0)
        {
            face = args.added[0];
        }
    }

    void Update()
    {
        if (face == null || face.vertices == null || face.vertices.Length == 0)
            return;

        // Get vertex position of forehead
        if (ForeheadVertexIndex >= face.vertices.Length)
            return;

        Vector3 foreheadLocal = face.vertices[ForeheadVertexIndex];
        Vector3 foreheadWorld = face.transform.TransformPoint(foreheadLocal);

        // Convert face world point → screen position (0..1)
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(foreheadWorld);
        float normY = screenPoint.y / Screen.height;

        // Send to HairAttachment
        if (hair == null)
            hair = FindObjectOfType<HairAttachment>();

        if (hair != null)
            hair.UpdateForeheadOffset01(normY);
    }
}
