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
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_Rigidbody.velocity = direction * speed;
    }


}
