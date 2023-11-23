using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage;
    private Rigidbody2D m_Rigidbody;
    public Vector2 direction;
    private int enemiesHit = 0;
    public float freezeDuration;
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
            if (other.CompareTag("Enemy") || other.CompareTag("RangedEnemy") || other.CompareTag("Boss"))
            {
                // TakeDamage on IEnemy interface
                other.GetComponent<IEnemy>().TakeDamage(damage);
                Destroy(gameObject);
            }
        } else if (proj_tag == "Knife"){
            if (other.CompareTag("Enemy") || other.CompareTag("RangedEnemy") || other.CompareTag("Boss"))
            {
                other.GetComponent<IEnemy>().TakeDamage(damage);
            }
            if (other.CompareTag("Enemy") || other.CompareTag("RangedEnemy") || other.CompareTag("Boss")){
                enemiesHit++;
                if (enemiesHit == 2 && KnifeUpgrade.level == 5){
                    Destroy(gameObject);
                } else if (KnifeUpgrade.level < 5){
                    Destroy(gameObject);
                }
            }
        } else if (proj_tag == "IceExplosion")  
        {
            if (other.CompareTag("Enemy") || other.CompareTag("RangedEnemy") || other.CompareTag("Boss"))
            {
                other.GetComponent<IEnemy>().Freeze(freezeDuration);
                if (IceGrenadeUpgrade.level >= 3){
                    other.GetComponent<IEnemy>().TakeDamage(damage);
                }
            }
        } else if (proj_tag == "Explosion") {
            if (other.CompareTag("Enemy") || other.CompareTag("RangedEnemy") || other.CompareTag("Boss"))
            {
                other.GetComponent<IEnemy>().TakeDamage(damage);
            }
        }
    }
}
