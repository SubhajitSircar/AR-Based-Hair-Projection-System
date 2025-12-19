using UnityEngine;
using System.Collections;
using System.IO;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class ScreenshotHandler : MonoBehaviour
{
    public void TakeScreenshot()
    {
        StartCoroutine(CaptureAndSaveScreenshot());
    }

    private IEnumerator CaptureAndSaveScreenshot()
    {
        yield return new WaitForEndOfFrame();

        // Capture screen into Texture2D
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        // Encode to PNG
        byte[] imageBytes = screenImage.EncodeToPNG();
        Destroy(screenImage);

        // Build file path
        string filename = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

#if UNITY_ANDROID
        // Use public Pictures folder
        string folderPath = "/storage/emulated/0/Pictures/UnityScreenshots";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fullPath = Path.Combine(folderPath, filename);
#else
        // For editor testing or other platforms
        string fullPath = Path.Combine(Application.persistentDataPath, filename);
#endif

        // Save file
        File.WriteAllBytes(fullPath, imageBytes);
        Debug.Log("Screenshot saved to: " + fullPath);

#if UNITY_ANDROID
        // Trigger media scan so it's visible in Gallery
        using (AndroidJavaClass mediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection"))
        using (AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                   .GetStatic<AndroidJavaObject>("currentActivity"))
        {
            mediaScanner.CallStatic("scanFile", context, new string[] { fullPath }, null, null);
        }
#endif
    }
}
