using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;





#if UNITY_ANDROID
using UnityEngine.Android;
#endif



public class ScreenshotHandler : MonoBehaviour
{
    public Image i;
    string screenshotPath = "";
    public string URL = "http://187.127.140.189:8000/api/hair_classify";
    public GameObject loadingScreen;
    public GameObject genderSelectScreen;
    public TMP_Text t;
    public TMP_Text t2;

    public GameObject retakeText;
    public GameObject continueText;


    // api requests and responses
    public void makeAPIRequest()
    {
        StartCoroutine(APIRequest());
    }

    class UploadResponse
    {
        public bool face_detected;
        public string hair_length;
    }
    public IEnumerator APIRequest()
    {
        Debug.Log("Starting API request with image: " + screenshotPath);
        if (screenshotPath != null && screenshotPath != "")
        {
            loadingScreen.SetActive(true);

            byte[] imageBytes = System.IO.File.ReadAllBytes(screenshotPath);
            WWWForm form = new WWWForm();
            form.AddBinaryData("image", imageBytes);

            //t2.SetText("Uploading image...") ;

            using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
            {
                yield return www.SendWebRequest();
                t.gameObject.SetActive(true);
                loadingScreen.SetActive(false);
                genderSelectScreen.SetActive(true);

                //t2.text = "Processing response...";

                if (www.result != UnityWebRequest.Result.Success)
                {
                    //loadingScreen.SetActive(false);
                    //genderSelectScreen.SetActive(true);
                    t.text = www.error.ToString();
                }
                else
                {
                    loadingScreen.SetActive(false);
                    genderSelectScreen.SetActive(true);
                    string json = www.downloadHandler.text;
                    UploadResponse response = JsonUtility.FromJson<UploadResponse>(json);
                    t.text = response.hair_length;
                }
            }
        }

    }


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
        screenshotPath = fullPath;
#else
        // For editor testing or other platforms
        string fullPath = Path.Combine(Application.persistentDataPath, filename);
#endif

        // Save file
        File.WriteAllBytes(fullPath, imageBytes);
        Debug.Log("Screenshot saved to: " + fullPath);
        screenshotPath = fullPath;


#if UNITY_ANDROID
        // Trigger media scan so it's visible in Gallery
        using (AndroidJavaClass mediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection"))
        using (AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                   .GetStatic<AndroidJavaObject>("currentActivity"))
        {
            mediaScanner.CallStatic("scanFile", context, new string[] { fullPath }, null, null);
        }
#endif
        // if there is a viewer set set the image to the latest screenshot
        if (i != null)
        {
            Texture2D tex = new Texture2D(9, 16);
            tex.LoadImage(imageBytes);
            Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            i.sprite = s;
            i.gameObject.SetActive(true);
            i.enabled = true;
        }
        retakeText.SetActive(true);
        continueText.SetActive(true);

    }
}