using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] players;
    private int playerIndex = 0;
    private int selectedWeaponIndex = 0;
    public bool[] EnabledPlayersWeapons = { false, false, false, false, false, false };
    public int[] UpgradeLevels = { 1, 1, 1 };

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
    public GameObject ScreenBlockPrefab;
    public GameObject UpgradeUiPrefab;
    private int selectedUpgradeIndex;
    private GameObject endGameUi;
    private GameObject ScreenBlock;
    private GameObject player;
    // Player coins
    private PlayerData playerData;    
    // Game currency
    private int coins = 0;
    private int bossKills = 0;
    public float difficulty { get; private set; } = 1f;


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
            return;
        }
        LoadData();
        Debug.Log("Difficulty: " + difficulty.ToString());
    }

    void OnApplicationQuit()
    {   
        SaveData();
    }

    void SaveData()
    {
        Debug.Log("Saving data");
        playerData.coins = coins;
        playerData.bossKills = bossKills;
        playerData.upgradeLevels = UpgradeLevels;
        playerData.enabledWeapon = EnabledPlayersWeapons;
        playerData.difficulty = difficulty;
        int i = 0;
        var characterStats = new CharacterStats[players.Length];
        foreach (var player in players)
        {
            var charStats = new CharacterStats();
            var playerController = player.GetComponent<PlayerController>();
            // Get player stats
            charStats.health = playerController.health;
            charStats.luck = playerController.luck;
            charStats.damage = playerController.damage;
            charStats.fireRate = playerController.fireRate;
            charStats.moveSpeed = playerController.moveSpeed;
            // Save player stats
            characterStats[i] = charStats;
            i++;
        }
        playerData.characterStats = characterStats;
        DataManager.Save(playerData);
    }

    void LoadData()
    {
        Debug.Log("Loading data");
        var res = DataManager.Load();
        if (res != null)
        {
            playerData = res;
            difficulty = playerData.difficulty;
            coins = playerData.coins;
            bossKills = playerData.bossKills;
            UpgradeLevels = playerData.upgradeLevels;
            EnabledPlayersWeapons = playerData.enabledWeapon;
            for (int i = 0; i < players.Length; i++)
            {
                var player = players[i];
                var playerController = player.GetComponent<PlayerController>();
                var charStats = playerData.characterStats[i];
                // Set player stats
                playerController.health = charStats.health;
                playerController.luck = charStats.luck;
                playerController.damage = charStats.damage;
                playerController.fireRate = charStats.fireRate;
                playerController.moveSpeed = charStats.moveSpeed;
            }
            // EnabledPlayersWeapons = playerData.weaponLevels;
        } else {
            playerData = new PlayerData();
        }
    }

     void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveData();
        } else {
            LoadData();
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
        SceneManager.LoadScene("MainGameJoystickFullScreen");
        // Play music
        audioSource.clip = mainGameMusic;
        audioSource.Play();
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "MainGameJoystickFullScreen");

        // Spawn world
        Instantiate(World, new Vector3(0, 0, 0), Quaternion.identity);
        // Spawn spawner
        Instantiate(Spawner, new Vector3(0, 0, 0), Quaternion.identity);
        // Spawn player
        SpawnPlayer();
        // Canvas
        var canvasController = GameObject.Find("Canvas").GetComponent<HUDController>();
        canvasController.SetActiveAvatar(playerIndex);
    }

    public void SpawnPlayer()
    {
        player = Instantiate(players[playerIndex], new Vector3(0, 0, 0), Quaternion.identity);
        player.name = "Player";
        player.GetComponent<PlayerController>().selectedWeaponIndex = selectedWeaponIndex;

        if (selectedWeaponIndex == 1)
        {
            var controller = player.GetComponent<PlayerController>();
            controller.setNShots(3, true);

            if (playerIndex == 0)
            {
                controller.setSpread(true);
            } else {
                controller.SetFireRate(controller.fireRate - 0.1f > 0.1f ? controller.fireRate - 0.1f : 0.1f, true);
            }
        } else if (selectedWeaponIndex == 2)
        {
            var controller = player.GetComponent<PlayerController>();
            controller.setNShots(3, true);
            controller.SetFireRate(controller.fireRate - 0.2f > 0.1f ? controller.fireRate - 0.2f : 0.1f, true);
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

    public int GetPlayerBossKills() {
        return bossKills;
    }

    public void AddBossKills(int amount = 1)
    {
        bossKills += amount;
    }

    public bool SpendBossKills(int amount)
    {
        if (bossKills >= amount)
        {
            bossKills -= amount;
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
        if (won)
        {
            difficulty += 0.1f;
            AddBossKills();
        }
        Debug.Log("End game");
        var canvas = GameObject.Find("Canvas");
        ScreenBlock = Instantiate(ScreenBlockPrefab, canvas.transform);
        ScreenBlock.transform.SetParent(canvas.transform);
        endGameUi = Instantiate(EndGameUiPrefab, ScreenBlock.transform);
        endGameUi.transform.SetParent(ScreenBlock.transform);
        endGameUi.GetComponent<UiDeadEnd>().setUi(won, coins);
        pauseGame();
    }

    public int GetUpgradePrice()
    {
        return (int)(250 * Mathf.Pow(1.2f, UpgradeLevels[playerIndex]-1));
    }

    public int GetUpgradeLevel()
    {
        return UpgradeLevels[playerIndex];
    }

    public bool UpgradePlayer()
    {
        var upgradePrice = GetUpgradePrice();
        if (this.coins >= upgradePrice)
        {
            this.coins -= upgradePrice;
            UpgradeLevels[playerIndex]++;
            var player = players[playerIndex];
            var playerController = player.GetComponent<PlayerController>();
            playerController.health += 50;
            playerController.luck += 0.1f;
            playerController.damage += 15;
            if (playerController.fireRate - 0.05f > 0.1f)
                playerController.fireRate -= 0.05f;
            playerController.moveSpeed += 0.5f;
            return true;
        }
        return false;
    }


    public void SpawnPowerUpText(string Text)
    {
        var canvas = GameObject.Find("Canvas");
        var upgradeUi = Instantiate(UpgradeUiPrefab, canvas.transform);
        upgradeUi.transform.parent = canvas.transform;
        upgradeUi.GetComponent<TextMeshProUGUI>().text = Text;
        Destroy(upgradeUi, 2f);
    }

    public void RespawnPlayer()
    {
        resumeGame();
        Debug.Log("Player respawn");
        // EnemyManager.Instance.DisableAllEnemies();
        Destroy(endGameUi);
        Destroy(ScreenBlock);
        player.GetComponent<PlayerController>().revivePlayer();
        // EnemySpawnerController.Instance.RestartWave();
    }
}
