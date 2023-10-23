using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameObject[] Avatars;
    public GameObject HealthSlider;
    public GameObject ExperienceSlider;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
