using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public state State = state.HomeScreen;
    public GameObject Camera;
    public float CameraSmoothTime = 1.0f;
    public Canvas canvas;
    public GameObject Character1;
    public GameObject Character1Weapons;
    public GameObject Character2;
    public GameObject Character2Weapons;
    public GameObject Character3;
    public GameObject Character3Weapons;
    public GameObject CharacterCursor;
    public GameObject MoneyIndicator;
    public GameObject WeaponCursor;
    public int WeaponPrice = 0;
    public GameObject WeaponPurchaseButton;
    public GameObject LevelIndicator;

    private Vector3 CameraTarget = Vector3.zero;
    private Vector3 CameraVelocity = Vector3.zero;
    private int ActiveCharacterIndex = 0;
    private MainMenuCharacterController ActiveCharacterController;
    private int Money;

    // Menu Sounds
    private AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip errorSound;
    public AudioClip primaryButtonSound;

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

    public void SelectCharacter(int characterIndex)
    {
        float cursorPositionY = 0.0f;

        Character1Weapons.SetActive(false);
        Character2Weapons.SetActive(false);
        Character3Weapons.SetActive(false);
        GameManager.Instance.SelectPlayer(characterIndex);
        SelectWeapon(0);

        switch (characterIndex)
        {
            case 0:
                ActiveCharacterController = Character1.GetComponent<MainMenuCharacterController>();
                cursorPositionY = 2.98f;
                Character1Weapons.SetActive(true);
                WeaponPrice = 100;
                break;
            case 1:
                ActiveCharacterController = Character2.GetComponent<MainMenuCharacterController>();
                cursorPositionY = -0.08f;
                Character2Weapons.SetActive(true);
                WeaponPrice = 100;
                break;
            case 2:
                ActiveCharacterController = Character3.GetComponent<MainMenuCharacterController>();
                cursorPositionY = -3.08f;
                Character3Weapons.SetActive(true);
                WeaponPrice = 100;
                break;
        }

        ActiveCharacterIndex = characterIndex;
        CharacterCursor.transform.position = new Vector3(CharacterCursor.transform.position.x, cursorPositionY, CharacterCursor.transform.position.z);
        WeaponPurchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = "$ " + WeaponPrice.ToString();

        WeaponPurchaseButton.SetActive(!GameManager.Instance.EnabledPlayersWeapons[characterIndex]);
    }

    public void SelectWeapon(int weaponIndex)
    {
        float cursorPositionY = 0.0f;

        switch (weaponIndex)
        {
            case 0:
                cursorPositionY = 1.985f;
                break;
            case 1:
                cursorPositionY = -2.015f;
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

        Money -= WeaponPrice;
        GameManager.Instance.EnabledPlayersWeapons[ActiveCharacterIndex] = true;

        WeaponPurchaseButton.SetActive(false);
        audioSource.PlayOneShot(clickSound);
        UpdateMoneyIndicator();
        SelectWeapon(1);
    }

    public void PurchaceLevelUp()
    {
        audioSource.PlayOneShot(errorSound);
        UpdateLevelIndicatorValue(2);
        UpdateLevelButtonValue(500);
    }

    public void UpdateLevelIndicatorValue(int value)
    {
        LevelIndicator.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + value.ToString();
    }

    public void UpdateLevelButtonValue(int value)
    {
        LevelIndicator.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "$ " + value;
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
        CameraTarget = new Vector3(16, 0, -10);
        ActiveCharacterController.Target.x = 11;
    }

    void HandleWeaponScreen()
    {
        CameraTarget = new Vector3(32, 0, -10);
        ActiveCharacterController.Target.x = 30;
    }
}
