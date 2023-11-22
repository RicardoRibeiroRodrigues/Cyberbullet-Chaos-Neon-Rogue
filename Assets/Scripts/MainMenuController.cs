using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public state State = state.HomeScreen;
    public GameObject Camera;
    public float CameraSmoothTime = 1.0f;
    public GameObject Character1;
    public GameObject Character1Weapons;
    public GameObject Character2;
    public GameObject Character2Weapons;
    public GameObject Character3;
    public GameObject Character3Weapons;
    public GameObject CharacterCursor;
    private int Money;
    public GameObject MoneyIndicator;
    public GameObject WeaponCursor;
    public int WeaponPrice = 0;
    public GameObject WeaponPurchaseButton;

    private Vector3 CameraTarget = Vector3.zero;
    private Vector3 CameraVelocity = Vector3.zero;
    private MainMenuCharacterController ActiveCharacterController;

    // Menu Sounds
    private AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip errorSound;
    public AudioClip primaryButtonSound;
    private int ActiveCharacterIndex = 0;
    public GameObject StatsIndicator;
    public GameObject LevelIndicator;
    public GameObject LevelUpCostText;

    public enum state
    {
        HomeScreen,
        CharacterScreen,
        WeaponScreen,
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Money = GameManager.Instance.GetPlayerCoins();
        UpdateMoneyIndicator();
        SelectCharacter(0);
        UpdateState(0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, CameraTarget, ref CameraVelocity, CameraSmoothTime);
    }

    public void playClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void startGame()
    {
        audioSource.PlayOneShot(primaryButtonSound);
        GameManager.Instance.StartGame();
    }

    public void SelectCharacter(int characterIndex)
    {
        float cursorPositionY = 0.0f;

        Character1Weapons.SetActive(false);
        Character2Weapons.SetActive(false);
        Character3Weapons.SetActive(false);
        SelectWeapon(0);
        GameManager.Instance.SelectPlayer(characterIndex);


        switch (characterIndex)
        {
            case 0:
                ActiveCharacterController = Character1.GetComponent<MainMenuCharacterController>();
                cursorPositionY = 3.0f;
                Character1Weapons.SetActive(true);
                WeaponPurchaseButton.SetActive(true);
                WeaponPrice = 100;
                break;
            case 1:
                ActiveCharacterController = Character2.GetComponent<MainMenuCharacterController>();
                cursorPositionY = -0.0f;
                Character2Weapons.SetActive(true);
                WeaponPurchaseButton.SetActive(true);
                WeaponPrice = 150;
                break;
            case 2:
                ActiveCharacterController = Character3.GetComponent<MainMenuCharacterController>();
                cursorPositionY = -3.0f;
                Character3Weapons.SetActive(true);
                WeaponPurchaseButton.SetActive(true);
                WeaponPrice = 200;
                break;
        }
        ActiveCharacterIndex = characterIndex;

        CharacterCursor.transform.position = new Vector3(CharacterCursor.transform.position.x, cursorPositionY, CharacterCursor.transform.position.z);
        WeaponPurchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = "$ " + WeaponPrice.ToString();
        WeaponPurchaseButton.SetActive(!GameManager.Instance.EnabledPlayersWeapons[characterIndex]);
        UpdateLevelIndicatorValue(GameManager.Instance.GetUpgradeLevel());
        UpdateLevelButtonValue(GameManager.Instance.GetUpgradePrice());
    }

    public void SelectWeapon(int weaponIndex)
    {
        float cursorPositionY = 0.0f;

        switch (weaponIndex)
        {
            case 0:
                cursorPositionY = 3.0f;
                break;
            case 1:
                cursorPositionY = -0.0f;
                break;
            case 2:
                cursorPositionY = -3.0f;
                break;
        }

        WeaponCursor.transform.position = new Vector3(WeaponCursor.transform.position.x, cursorPositionY, WeaponCursor.transform.position.z);
        GameManager.Instance.SelectWeapon(weaponIndex);
    }

    public void PurchaseWeapon2()
    {

        if (!GameManager.Instance.SpendCoins(WeaponPrice))
        {
            audioSource.PlayOneShot(errorSound);
            return;
        }
        GameManager.Instance.EnabledPlayersWeapons[ActiveCharacterIndex] = true;
        audioSource.PlayOneShot(clickSound);
        Money -= WeaponPrice;
        WeaponPurchaseButton.SetActive(false);
        UpdateMoneyIndicator();
        SelectWeapon(1);
    }
    
    public void PurchaseLevelUp()
    {
        if (!GameManager.Instance.UpgradePlayer())
        {
            audioSource.PlayOneShot(errorSound);
            return;
        }

        audioSource.PlayOneShot(clickSound);
        Money = GameManager.Instance.GetPlayerCoins();
        UpdateMoneyIndicator();
        UpdateLevelIndicatorValue(GameManager.Instance.GetUpgradeLevel());
        UpdateLevelButtonValue(GameManager.Instance.GetUpgradePrice());
        UpdatePlayerStatsBoardText();
    }

    public void UpdateLevelIndicatorValue(int value)
    {
        LevelIndicator.GetComponentInChildren<TextMeshProUGUI>().text = "Nível " + value.ToString();
    }

    public void UpdatePlayerStatsBoardText()
    {
        GameObject player = GameManager.Instance.players[ActiveCharacterIndex];
        PlayerController playerController = player.GetComponent<PlayerController>();
        string health = "Vida: " + playerController.health.ToString();
        string damage = "Dano: " + playerController.damage.ToString();
        string speed = "Velocidade: " + playerController.moveSpeed.ToString("F1") + "x";
        string fireRate = "Fire Rate: " + (1 / playerController.fireRate).ToString("F1") + "/s";
        string luck = "Sorte: " + playerController.luck.ToString("F1") + "x";

        StatsIndicator.GetComponent<TextMeshProUGUI>().text = health + "\n\n" + damage + "\n\n" + speed + "\n\n" + fireRate + "\n\n" + luck;
    }

    public void UpdateLevelButtonValue(int value)
    {
        LevelUpCostText.GetComponent<TextMeshProUGUI>().text = "$ " + value;
    }

    public void UpdateMoneyIndicator()
    {
        MoneyIndicator.GetComponentInChildren<TextMeshProUGUI>().text = Money.ToString();
    }

    public void UpdateState(int stateIndex)
    {
        switch (stateIndex)
        {
            case 0:
                State = state.HomeScreen;
                HandleHomeScreen();
                break;
            case 1:
                audioSource.PlayOneShot(primaryButtonSound);
                State = state.CharacterScreen;
                HandleCharacterScreen();
                break;
            case 2:
                audioSource.PlayOneShot(primaryButtonSound);
                State = state.WeaponScreen;
                HandleWeaponScreen();
                break;
        }
    }

    void HandleHomeScreen()
    {
        CameraTarget = new Vector3(0, 0, -10);
    }

    void HandleCharacterScreen()
    {
        CameraTarget = new Vector3(21, 0, -10);
        ActiveCharacterController.Target.x = 14;
    }

    void HandleWeaponScreen()
    {
        CameraTarget = new Vector3(42, 0, -10);
        ActiveCharacterController.Target.x = 43;

        UpdatePlayerStatsBoardText();
    }
}
