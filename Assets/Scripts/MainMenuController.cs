using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public state State = state.HomeScreen;
    public GameObject Camera;
    public float CameraSmoothTime = 0.15f;

    private Vector3 cameraTarget = Vector3.zero;
    private Vector3 CameraVelocity = Vector3.zero;

    public enum state
    {
        HomeScreen,
        CharacterScreen,
        WeaponScreen,
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateState(0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, cameraTarget, ref CameraVelocity, CameraSmoothTime);
    }

    public void UpdateState(int newState)
    {
        switch (newState)
        {
            case 0:
                State = state.HomeScreen;
                break;
            case 1:
                State = state.CharacterScreen;
                break;
            case 2:
                State = state.WeaponScreen;
                break;
        }

        Console.WriteLine("Hello World!");

        switch (State)
        {
            case state.HomeScreen:
                cameraTarget = new Vector3(0, 0, -10);
                break;
            case state.CharacterScreen:
                cameraTarget = new Vector3(16, 0, -10);
                break;
            case state.WeaponScreen:
                cameraTarget = new Vector3(32, 0, -10);
                break;
        }
    }
}
