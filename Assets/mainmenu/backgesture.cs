using UnityEngine;

public class MobileBackHandler : MonoBehaviour
{
    public GameObject currentCanvas;
    public GameObject previousCanvas;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
            currentCanvas.SetActive(false);
            previousCanvas.SetActive(true);
        }
    }
}
