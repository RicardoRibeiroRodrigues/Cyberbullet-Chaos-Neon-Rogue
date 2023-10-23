using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    public int damage = 100;
    // on trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }
        if (other.gameObject.CompareTag("RangedEnemy"))
        {
            other.gameObject.GetComponent<RangedEnemyController>().TakeDamage(damage);
        }
    }
}
