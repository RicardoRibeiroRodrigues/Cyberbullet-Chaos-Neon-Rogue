using UnityEngine;
using TMPro;

public class UiDeadEnd : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clickSound;
    public int coinsvar = 0;
    public bool doubleCoins = false;

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
        coinsvar = coinsEarned;
        var coins = transform.Find("CoinsDisplay").transform.Find("CoinsText");
        coins.GetComponent<TextMeshProUGUI>().text = "+" + coinsEarned.ToString();
    }

    public void DoubleCoins()
    {
        if (!doubleCoins)
        {
            coinsvar = coinsvar * 2;
            var coins = transform.Find("CoinsDisplay").transform.Find("CoinsText");
            coins.GetComponent<TextMeshProUGUI>().text = "+" + coinsvar.ToString();
            doubleCoins = true;
            transform.Find("RewardedAd").gameObject.SetActive(false);
        }
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
