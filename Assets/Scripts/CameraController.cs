using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;

    void FixedUpdate()
    {
        if (player != null)
            transform.position = player.transform.position + offset;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        offset = transform.position - player.transform.position;
    }
}
