using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;

[RequireComponent(typeof(ARFace))]
public class HeadOcclusion : MonoBehaviour
{
    public GameObject headMaskPrefab; // assign sphere or head mesh with DepthMask shader
    private GameObject headMaskInstance;
    private ARFace arFace;

    void Start()
    {
        arFace = GetComponent<ARFace>();

        if (headMaskPrefab != null)
        {
            headMaskInstance = Instantiate(headMaskPrefab, transform);
            headMaskInstance.transform.localPosition = Vector3.zero;
            headMaskInstance.transform.localRotation = Quaternion.identity;
        }
    }

    void Update()
    {
        if (headMaskInstance != null && arFace.vertices.IsCreated && arFace.vertices.Length > 0)
        {
            // Example: scale head mask based on detected face mesh width
            float faceWidth = 0.12f; // fallback default
            headMaskInstance.transform.localScale = new Vector3(faceWidth, faceWidth, faceWidth);
        }
    }
}
