using UnityEngine;

public class CameraBound : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss") || other.CompareTag("RangedEnemy"))
        {
            if (other.GetComponent<IEnemy>().isDying) return;
            // Deactivate Animator
            other.GetComponent<Animator>().enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss") || other.CompareTag("RangedEnemy"))
        {
            // Activate Animator
            other.GetComponent<Animator>().enabled = true;
        }
    }
}
