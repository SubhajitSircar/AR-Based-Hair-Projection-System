using UnityEngine;

public class ExitApp : MonoBehaviour
{
    public void ExitGame()
    {
        // Quit the application
        Application.Quit();

        // For editor testing (won't work in build)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
