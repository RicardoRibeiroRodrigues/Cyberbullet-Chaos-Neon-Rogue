using UnityEngine;

public class BossSpecial : MonoBehaviour
{
    public int damage = 100;
    // on trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Boss special hit");
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
