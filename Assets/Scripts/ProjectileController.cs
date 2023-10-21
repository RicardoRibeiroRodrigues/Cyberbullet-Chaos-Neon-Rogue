using System;
using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage;
    private Rigidbody2D m_Rigidbody;
    public Vector2 direction;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        if (!gameObject.CompareTag("FireGrenade")){
            Destroy(gameObject, lifeTime);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_Rigidbody.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var proj_tag = gameObject.tag;
        if (proj_tag == "EnemyBullet")
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().TakeDamage(damage);
                Destroy(gameObject);
            }
        } else if (proj_tag == "PlayerBullet")
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyController>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

}
