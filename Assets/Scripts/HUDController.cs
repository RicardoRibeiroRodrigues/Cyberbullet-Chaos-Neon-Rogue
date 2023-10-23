using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameObject[] Avatars;
    public GameObject HealthSlider;
    public GameObject ExperienceSlider;
    public GameObject[] Inventory;

    public void SetActiveAvatar(int avatarIndex)
    {
        foreach (GameObject avatar in Avatars)
        {
            avatar.SetActive(false);
        }
        Avatars[avatarIndex].SetActive(true);
    }

    public void updateHealthValue(int value)
    {
        HealthSlider.GetComponent<Slider>().value = value;
    }

    public void updateExperienceProgress(float value)
    {
        ExperienceSlider.GetComponent<Slider>().value = value;
    }

    public void addItem(Sprite image)
    {
        foreach (GameObject Image in Inventory)
        {
            // If item is not active, set it active and set the image.
            if (!Image.activeSelf)
            {
                Image.GetComponent<Image>().sprite = image;
                Image.SetActive(true);
                break;
            }
        }
    }
}