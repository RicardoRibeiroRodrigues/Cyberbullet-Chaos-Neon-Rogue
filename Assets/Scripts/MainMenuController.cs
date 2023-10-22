using UnityEngine;
using TMPro;

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
    private int Money;
    public GameObject MoneyIndicator;
    public GameObject WeaponCursor;
    public int WeaponPrice = 0;
    public GameObject WeaponPurchaseButton;

    private Vector3 CameraTarget = Vector3.zero;
    private Vector3 CameraVelocity = Vector3.zero;
    private MainMenuCharacterController SelectedCharacterController;

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
                SelectedCharacterController = Character1.GetComponent<MainMenuCharacterController>();
                cursorPositionY = 2.98f;
                Character1Weapons.SetActive(true);
                WeaponPurchaseButton.SetActive(true);
                WeaponPrice = 100;
                break;
            case 1:
                SelectedCharacterController = Character2.GetComponent<MainMenuCharacterController>();
                cursorPositionY = -0.08f;
                Character2Weapons.SetActive(true);
                WeaponPurchaseButton.SetActive(true);
                WeaponPrice = 150;
                break;
            case 2:
                SelectedCharacterController = Character3.GetComponent<MainMenuCharacterController>();
                cursorPositionY = -3.08f;
                Character3Weapons.SetActive(true);
                WeaponPurchaseButton.SetActive(true);
                WeaponPrice = 200;
                break;
        }

        CharacterCursor.transform.position = new Vector3(CharacterCursor.transform.position.x, cursorPositionY, CharacterCursor.transform.position.z);
        WeaponPurchaseButton.GetComponentInChildren<TextMeshProUGUI>().text = "$ " + WeaponPrice.ToString();
    }

    public void SelectWeapon(int weaponIndex)
    {
        float cursorPositionY = 0.0f;
        GameManager.Instance.SelectWeapon(weaponIndex);

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
    }

    public void PurchaseWeapon2()
    {

        if (GameManager.Instance.SpendCoins(WeaponPrice))
        {
            audioSource.PlayOneShot(clickSound);
            Money -= WeaponPrice;
            WeaponPurchaseButton.SetActive(false);
            UpdateMoneyIndicator();
            SelectWeapon(1);
        } else {
            audioSource.PlayOneShot(errorSound);
        }
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
        SelectedCharacterController.Target.x = 11;
    }

    void HandleWeaponScreen()
    {
        CameraTarget = new Vector3(32, 0, -10);
        SelectedCharacterController.Target.x = 30;
    }
}
