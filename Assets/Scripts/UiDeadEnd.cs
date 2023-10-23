using UnityEngine;
using TMPro;

public class UiDeadEnd : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clickSound;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void setUi(bool hasWin, int coinsEarned)
    {
        var title = transform.Find("Title");
        if (hasWin)
        {
            // Title
            title.GetComponent<TextMeshProUGUI>().text = "Você venceu!";
        }
        else {
            // Title
            title.GetComponent<TextMeshProUGUI>().text = "Você morreu, fique mais forte e tente novamente!";
        }
        // Coins
        var coins = transform.Find("CoinsDisplay").transform.Find("CoinsText");
        coins.GetComponent<TextMeshProUGUI>().text = "+" + coinsEarned.ToString();
    }

    public void RestartGame()
    {
        audioSource.PlayOneShot(clickSound);
        GameManager.Instance.StartGame();
    } 

    public void MainMenu()
    {
        audioSource.PlayOneShot(clickSound);
        GameManager.Instance.GoToMainMenu();
    }
}
