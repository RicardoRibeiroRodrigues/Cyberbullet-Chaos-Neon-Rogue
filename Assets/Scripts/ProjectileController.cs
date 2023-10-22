using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage;
    private Rigidbody2D m_Rigidbody;
    public Vector2 direction;
    private bool isDamaging = true;
    private int enemiesHit = 0;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
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
            if (other.CompareTag("RangedEnemy"))
            {
                other.GetComponent<RangedEnemyController>().TakeDamage(damage);
                Destroy(gameObject);
            }
        } else if (proj_tag == "Explosion" && isDamaging){
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyController>().TakeDamage(damage);
                isDamaging = false;
                StartCoroutine(WaitToDamage());
            }
            if (other.CompareTag("RangedEnemy"))
            {
                other.GetComponent<RangedEnemyController>().TakeDamage(damage);
                isDamaging = false;
                StartCoroutine(WaitToDamage());
            }
        } else if (proj_tag == "Knife"){
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyController>().TakeDamage(damage);
            }
            if (other.CompareTag("RangedEnemy"))
            {
                other.GetComponent<RangedEnemyController>().TakeDamage(damage);
            }
            if (other.CompareTag("Enemy") || other.CompareTag("RangedEnemy")){
                enemiesHit++;
                if (enemiesHit == 2 && KnifeUpgrade.level == 5){
                    Destroy(gameObject);
                }else if (KnifeUpgrade.level < 5){
                    Destroy(gameObject);
                }
            }
        }   
    }
    IEnumerator WaitToDamage()
    {
        yield return new WaitForSeconds(0.8f);
        isDamaging = true;
    }
}
