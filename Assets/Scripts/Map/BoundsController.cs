using UnityEngine;

public class BoundsController : MonoBehaviour
{
    private GameObject player;

    void FixedUpdate()
    {
        if (player == null) {
            player = GameObject.Find("Player");
        } else {
            transform.position = player.transform.position;
        }
    }
}
