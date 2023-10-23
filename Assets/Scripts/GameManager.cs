using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] players;
    private int playerIndex = 0;
    private int selectedWeaponIndex = 0;
    // World Prefabs
    public GameObject World;
    public GameObject Spawner;
    // Music
    private AudioSource audioSource;
    public AudioClip mainMenuMusic;
    public AudioClip mainGameMusic;
    public AudioClip tenseMusic;
    // Item select ui
    public GameObject itemSelectUiPrefab;
    public GameObject EndGameUiPrefab;
    private int selectedUpgradeIndex;
    // Player coins
    private int coins = 0;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        resumeGame();
        StartCoroutine(LoadMainGame());
    }

    IEnumerator LoadMainGame()
    {
        SceneManager.LoadScene("MainGame");
        // Play music
        audioSource.clip = mainGameMusic;
        audioSource.Play();
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "MainGame");
        // Spawn world
        Instantiate(World, new Vector3(0, 0, 0), Quaternion.identity);
        // Spawn spawner
        Instantiate(Spawner, new Vector3(0, 0, 0), Quaternion.identity);
        // Spawn player
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        var player = Instantiate(players[playerIndex], new Vector3(0, 0, 0), Quaternion.identity);
        player.name = "Player";
        player.GetComponent<PlayerController>().selectedWeaponIndex = selectedWeaponIndex;

        if (selectedUpgradeIndex == 1)
        {
            var controller = player.GetComponent<PlayerController>();
            controller.setNShots(3);

            if (playerIndex == 0)
            {
                controller.setSpread(true);
            } else {
                controller.SetFireRate(controller.fireRate - 0.1f > 0.1f ? controller.fireRate - 0.1f : 0.1f);
            }
        }
        // Set camera to follow player
        Camera.main.GetComponent<CameraController>().SetPlayer(player);
    }

    public void GoToMainMenu()
    {
        resumeGame();
        SceneManager.LoadScene("MainMenuScene");
        // Play music
        audioSource.clip = mainMenuMusic;
        audioSource.Play();
    }

    public void SelectPlayer(int index)
    {
        playerIndex = index;
    }

    public void SelectWeapon(int index)
    {
        selectedWeaponIndex = index;
    }

    public int GetPlayerCoins()
    {
        return coins;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            return true;
        }
        return false;
    }

    public void putTenseMusic()
    {
        audioSource.clip = tenseMusic;
        audioSource.volume = (float) 0.75 * audioSource.volume;
        audioSource.Play();
        Invoke(nameof(removeTenseMusic), 30f);
    }

    private void removeTenseMusic()
    {
        audioSource.clip = mainGameMusic;
        audioSource.volume = (float) (audioSource.volume / 0.75);
        audioSource.Play();
    }

    public IEnumerator SelectItem(List<UpgradeData> upgrades, PlayerController player)
    {
        selectedUpgradeIndex = -1;
        pauseGame();
        var canvas = GameObject.Find("Canvas");
        var itemSelectUi = Instantiate(itemSelectUiPrefab, canvas.transform);
        itemSelectUi.transform.SetParent(canvas.transform);
        itemSelectUi.GetComponent<UpgradeSelectUi>().selectUpgrade(upgrades);
        yield return new WaitUntil(() => selectedUpgradeIndex != -1);
        Destroy(itemSelectUi);
        resumeGame();
        player.setSelectedUpgrade(selectedUpgradeIndex);
    }

    public void setSelectedIndex(int index)
    {
        selectedUpgradeIndex = index;
    }

    public void endGame(bool won, int coins)
    {
        pauseGame();
        var canvas = GameObject.Find("Canvas");
        var endGameUi = Instantiate(EndGameUiPrefab, canvas.transform);
        endGameUi.transform.parent = canvas.transform;
        endGameUi.GetComponent<UiDeadEnd>().setUi(won, coins);
    }
}
