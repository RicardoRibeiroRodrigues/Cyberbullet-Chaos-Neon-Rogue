using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        offset = transform.position - player.transform.position ;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
