using UnityEngine;
using TMPro;

public class UiDeadEnd : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clickSound;
    public int coinsvar = 0;
    public static bool  doubleCoins = false;
    public static bool revivead = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void setUi(bool hasWin, int coinsEarned)
    {
        var title = transform.Find("Title");
        if (hasWin)
        {
            revivead = true;
            // Title
            title.GetComponent<TextMeshProUGUI>().text = "Você venceu!";
            if (!doubleCoins){
                transform.Find("ReviveAd").gameObject.SetActive(false);
                transform.Find("RewardedAd").gameObject.SetActive(true);
            }else{
                transform.Find("ReviveAd").gameObject.SetActive(false);
                transform.Find("RewardedAd").gameObject.SetActive(false);
            }
        }
        else {
            // Title
            title.GetComponent<TextMeshProUGUI>().text = "Você morreu, fique mais forte e tente novamente!";
            if (!revivead){
                transform.Find("ReviveAd").gameObject.SetActive(true);
                transform.Find("RewardedAd").gameObject.SetActive(false);
            }else if (!doubleCoins){
                transform.Find("ReviveAd").gameObject.SetActive(false);
                transform.Find("RewardedAd").gameObject.SetActive(true);
            }else{
                transform.Find("ReviveAd").gameObject.SetActive(false);
                transform.Find("RewardedAd").gameObject.SetActive(false);
            }    
        }
        // Coins
        coinsvar = coinsEarned;
        var coins = transform.Find("CoinsDisplay").transform.Find("CoinsText");
        coins.GetComponent<TextMeshProUGUI>().text = "+" + coinsEarned.ToString();
    }

    public void ReviveCharacter()
    {
        if (!revivead) 
        {
            audioSource.PlayOneShot(clickSound);
            // Chama o método de respawn
            GameManager.Instance.RespawnPlayer();
            revivead = true;
            Debug.Log("Revive ad");
            // Desativa o botão de reviver após a primeira revivida
        }
    }

    public void DoubleCoins()
    {
        if (!doubleCoins)
        {
            audioSource.PlayOneShot(clickSound);
            coinsvar = coinsvar * 2;
            var coins = transform.Find("CoinsDisplay").transform.Find("CoinsText");
            coins.GetComponent<TextMeshProUGUI>().text = "+" + coinsvar.ToString();
            doubleCoins = true;
            transform.Find("RewardedAd").gameObject.SetActive(false);
        }
    }

    public void RestartGame()
    {
        revivead = false;
        doubleCoins = false;
        GameManager.Instance.AddCoins(coinsvar);
        audioSource.PlayOneShot(clickSound);
        GameManager.Instance.StartGame();
    } 

    public void MainMenu()
    {
        revivead = false;
        doubleCoins = false;
        GameManager.Instance.AddCoins(coinsvar);
        audioSource.PlayOneShot(clickSound);
        GameManager.Instance.GoToMainMenu();
    }
}
