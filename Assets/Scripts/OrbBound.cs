using UnityEngine;

public class OrbBound : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("XpOrb"))
        {
            other.gameObject.transform.position = gameObject.transform.position;
        }
    }
}
