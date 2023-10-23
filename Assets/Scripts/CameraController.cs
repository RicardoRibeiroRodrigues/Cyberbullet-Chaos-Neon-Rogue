using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;
    public float CameraSmoothTime = 0.5f;
    private Vector3 CameraTarget = Vector3.zero;
    private Vector3 CameraVelocity = Vector3.zero;

    void FixedUpdate()
    {
        if (player != null){
            CameraTarget = player.transform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, CameraTarget, ref CameraVelocity, CameraSmoothTime);
        }
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        offset = transform.position - player.transform.position;
    }
}
