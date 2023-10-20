using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TextCore.Text;

public class GameController : MonoBehaviour
{
    public state State = state.HomeScreen;
    public GameObject Camera;
    public float CameraSmoothTime = 1.0f;
    public Canvas canvas;
    public float CharacterSpeed = 1.0f;
    public GameObject Character1;
    public GameObject Character2;
    public GameObject Character3;

    private Vector3 CameraTarget = Vector3.zero;
    private Vector3 CameraVelocity = Vector3.zero;
    private SpriteRenderer CharacterRenderer;
    private Vector3 CharacterTarget = Vector3.zero;
    private Vector3 CharacterVelocity = Vector3.zero;
    private GameObject SelectedCharacter;

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
        Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, CameraTarget, ref CameraVelocity, CameraSmoothTime, CharacterSpeed);
        SelectedCharacter.transform.position = Vector3.SmoothDamp(SelectedCharacter.transform.position, CharacterTarget, ref CharacterVelocity, 0.0f, CharacterSpeed);
        CharacterRenderer.flipX = CharacterVelocity.x < 0.0;
    }

    public void SelectCharacter(int characterIndex)
    {
        switch (characterIndex)
        {
            case 0:
                SelectedCharacter = Character1;
                break;
            case 1:
                SelectedCharacter = Character2;
                break;
            case 2:
                SelectedCharacter = Character3;
                break;
        }

        CharacterRenderer = SelectedCharacter.GetComponent<SpriteRenderer>();
        CharacterTarget = SelectedCharacter.transform.position;
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
        CharacterTarget.x = 11;
    }

    void HandleWeaponScreen()
    {
        CameraTarget = new Vector3(32, 0, -10);
        CharacterTarget.x = 30;
    }
}
