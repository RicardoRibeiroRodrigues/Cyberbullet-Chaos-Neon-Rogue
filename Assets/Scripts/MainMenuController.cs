using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

public class MainMenuController : MonoBehaviour
{
    public state State = state.HomeScreen;
    public GameObject Camera;
    public float CameraSmoothTime = 1.0f;
    public Canvas canvas;
    public GameObject Character1;
    public GameObject Character2;
    public GameObject Character3;
    public GameObject CharacterCursor;
    public GameObject WeaponCursor;

    private Vector3 CameraTarget = Vector3.zero;
    private Vector3 CameraVelocity = Vector3.zero;
    private MainMenuCharacterController SelectedCharacterController;

    public enum state
    {
        HomeScreen,
        CharacterScreen,
        WeaponScreen,
    }

    // Start is called before the first frame update
    void Start()
    {
        SelectCharacter(0);
        UpdateState(0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, CameraTarget, ref CameraVelocity, CameraSmoothTime);
    }

    public void SelectCharacter(int characterIndex)
    {
        float cursorPositionY = 0.0f;

        switch (characterIndex)
        {
            case 0:
                SelectedCharacterController = Character1.GetComponent<MainMenuCharacterController>();
                cursorPositionY = 2.98f;
                break;
            case 1:
                SelectedCharacterController = Character2.GetComponent<MainMenuCharacterController>();
                cursorPositionY = -0.08f;
                break;
            case 2:
                SelectedCharacterController = Character3.GetComponent<MainMenuCharacterController>();
                cursorPositionY = -3.08f;
                break;
        }

        CharacterCursor.transform.position = new Vector3(CharacterCursor.transform.position.x, cursorPositionY, CharacterCursor.transform.position.z);
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
                State = state.CharacterScreen;
                HandleCharacterScreen();
                break;
            case 2:
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
