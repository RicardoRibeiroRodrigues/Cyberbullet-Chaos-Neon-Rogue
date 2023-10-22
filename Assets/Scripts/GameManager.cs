using System.Collections;
using System.Collections.Generic;
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
        var player = Instantiate(players[playerIndex], new Vector3(0, 0, 0), Quaternion.identity);
        player.name = "Player";
        player.GetComponent<PlayerController>().selectedWeaponIndex = selectedWeaponIndex;
        // Set camera to follow player
        Camera.main.GetComponent<CameraController>().SetPlayer(player);
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
        audioSource.volume = (int) 0.75 * audioSource.volume;
        audioSource.Play();
        Invoke(nameof(removeTenseMusic), 45f);
    }

    private void removeTenseMusic()
    {
        audioSource.clip = mainGameMusic;
        audioSource.volume = (int) (audioSource.volume / 0.75);
        audioSource.Play();
    }
}
