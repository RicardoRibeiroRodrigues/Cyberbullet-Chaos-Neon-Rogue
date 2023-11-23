using UnityEngine;

public class BossSpecial : MonoBehaviour
{
    public int damage = 100;
    // on trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
