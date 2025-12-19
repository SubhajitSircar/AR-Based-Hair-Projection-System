using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HairSelectorUI : MonoBehaviour
{
    public void SelectHair(int index)
    {
        // Finds the active face at runtime
        ARFace face = FindObjectOfType<ARFace>();
        if (face != null)
        {
            HairSwitcher switcher = face.GetComponent<HairSwitcher>();
            if (switcher != null)
            {
                switcher.ActivateHair(index);
            }
        }
    }
}
