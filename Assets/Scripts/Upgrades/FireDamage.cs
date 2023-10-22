using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{

    public int damage = 1;
    public float lifeTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void FireDamageApply(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy take dmg" + damage);
            collider.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }
        if (collider.gameObject.CompareTag("RangedEnemy"))
        {
            collider.gameObject.GetComponent<RangedEnemyController>().TakeDamage(damage);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("RangedEnemy"))
        {
            FireDamageApply(other);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("RangedEnemy"))
        {
            FireDamageApply(other);
        }
    }
}
