using UnityEngine;

public class HairSwitcher : MonoBehaviour
{
    public GameObject[] hairs;

    public void ActivateHair(int index)
    {
        for (int i = 0; i < hairs.Length; i++)
        {
            hairs[i].SetActive(i == index);
        }
    }
}
