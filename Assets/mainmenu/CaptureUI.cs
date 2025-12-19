using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaptureUI : MonoBehaviour
{
    public TMP_Text instructionText;

    public Button frontButton;
    public Button sideButton;
    public Button doneButton;

    public RawImage frontPreview;
    public RawImage sidePreview;

    private Texture2D frontTex;
    private Texture2D sideTex;

    void Start()
    {
        frontButton.onClick.AddListener(CaptureFront);
        sideButton.onClick.AddListener(CaptureSide);
        doneButton.onClick.AddListener(OnDone);

        instructionText.text = "Please capture FRONT and SIDE photos.";
    }

    void CaptureFront()
    {
        instructionText.text = "Opening camera for FRONT photo...";

        NativeCamera.TakePicture((path) =>
        {
            if (path == null)
            {
                instructionText.text = "Front capture cancelled.";
                return;
            }

            Texture2D tex = NativeCamera.LoadImageAtPath(path, 1024);
            if (tex == null)
            {
                instructionText.text = "Failed to load front image.";
                return;
            }

            frontTex = tex;
            frontPreview.texture = tex;
            frontPreview.color = Color.white;

            instructionText.text = "Front photo captured! Now capture SIDE photo.";
        });
    }

    void CaptureSide()
    {
        instructionText.text = "Opening camera for SIDE photo...";

        NativeCamera.TakePicture((path) =>
        {
            if (path == null)
            {
                instructionText.text = "Side capture cancelled.";
                return;
            }

            Texture2D tex = NativeCamera.LoadImageAtPath(path, 1024);
            if (tex == null)
            {
                instructionText.text = "Failed to load side image.";
                return;
            }

            sideTex = tex;
            sidePreview.texture = tex;
            sidePreview.color = Color.white;

            instructionText.text = "Side photo captured! Press DONE.";
        });
    }

    void OnDone()
    {
        if (frontTex == null || sideTex == null)
        {
            instructionText.text = "Please capture BOTH photos first!";
            return;
        }

        instructionText.text = "Thank you! Both photos captured.";
    }
}
