using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private int lifeTime = 8;
    public Vector2 direction;
    public float speed = 10f;
    public Vector3 explosionScale;
    public GameObject explosionPrefab;
    public int damage;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        m_Rigidbody.velocity = direction * speed;
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("RangedEnemy") || other.CompareTag("Boss"))
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.GetComponent<ProjectileController>().direction = Vector2.zero;
            explosion.transform.localScale = explosionScale;
            explosion.GetComponent<ProjectileController>().damage = damage;
            Destroy(gameObject);
        }
    }
}
