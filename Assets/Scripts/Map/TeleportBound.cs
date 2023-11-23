using UnityEngine;

public class TeleportBound : MonoBehaviour
{
    public int teleportRadius;
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss") || other.CompareTag("RangedEnemy"))
        {
            // Don't teleport if enemy is dying
            if (other.GetComponent<IEnemy>().isDying) return;
            // Teleport enemy to a random position closer to the player, this gameObject is centered in the player
            Vector2 spawnPosition = transform.position;
            spawnPosition += Random.insideUnitCircle.normalized * teleportRadius;
            
            other.transform.position = spawnPosition;
        }
    }
}
