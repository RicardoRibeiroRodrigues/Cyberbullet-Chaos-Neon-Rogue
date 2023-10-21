using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool isMoving;
    public Vector2 Movement;
    private Animator animator;
    private Rigidbody2D m_Rigidbody;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject knifePrefab;
    public GameObject grenadeFirePrefab;
    public GameObject grenadeIcePrefab; 
    public GameObject grenadeFireExplosionPrefab;
    public GameObject grenadeIceExplosionPrefab;
    // Player stats
    public int health;
    public float moveSpeed;
    private Quaternion angle;
    private Vector2 direction;
    private int numKnifes;
    private GameObject fireGrenade;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {   
        numKnifes = 0;
        angle = Quaternion.Euler(0, 0, 0);
        direction = new Vector2(1, 0);
        InvokeRepeating(nameof(ActivateUpgrade), 1.0f, 5.0f);
        InvokeRepeating(nameof(ShootFireGrenade), 0.0f, 5.0f);
        // Para a bala
        InvokeRepeating(nameof(ShootBullet), 1.0f, 0.5f);
        // Para o laser
        // InvokeRepeating(nameof(ShootBullet), 0.0f, 0.25f);
    }
    
    void ShootFireGrenadeExplosion()
    {   
        if (fireGrenade != null) // Verifica se a granada ainda existe antes de destruí-la
        {
            Vector3 transform_position = fireGrenade.transform.position;
            Quaternion rotation_position = fireGrenade.transform.rotation;
            Vector3 transform_right = fireGrenade.transform.right;

            Destroy(fireGrenade);

            var explosion = Instantiate(grenadeFireExplosionPrefab, transform_position, rotation_position);
            explosion.GetComponent<ProjectileController>().direction = transform_right;
        }
        
    }
    
    void ShootFireGrenade()
    {
        fireGrenade = Instantiate(grenadeFirePrefab, firePoint.position, firePoint.rotation);
        fireGrenade.GetComponent<ProjectileController>().direction = firePoint.right;
        Invoke(nameof(ShootFireGrenadeExplosion), 1.0f);
    }
    void ShootKnifes()
    {
        var knife = Instantiate(knifePrefab, firePoint.position, angle);
        knife.GetComponent<ProjectileController>().direction = direction;
        angle *= Quaternion.Euler(0, 0, 45);
        float anguloZ = angle.eulerAngles.z;
        float angleRadians = Mathf.Deg2Rad * anguloZ;
        float direcaoX = Mathf.Cos(angleRadians);
        float direcaoY = Mathf.Sin(angleRadians);
        direction = new Vector2(direcaoX, direcaoY);
        numKnifes++;
        if (numKnifes == 8)
        {
            CancelInvoke(nameof(ShootKnifes));
            numKnifes = 0;
        }
    }
    void ActivateUpgrade()
    {
       InvokeRepeating(nameof(ShootKnifes), 0.0f, 0.1f);
    }
    void ShootBullet()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<ProjectileController>().direction = firePoint.right;
    }
    void FixedUpdate()
    {
        if (!isMoving)
        {
            if (Movement != Vector2.zero)
            {
                // Rotate the player to face the direction of movement.
                if (Movement.x < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                } else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                var targetPos = transform.position;
                targetPos.x += Movement.x;
                targetPos.y += Movement.y;
                StartCoroutine(Move(targetPos));
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        m_Rigidbody.MovePosition(Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime));
        yield return null;
        isMoving = false;
    }

    void OnMove(InputValue value)
    {
        Movement = value.Get<Vector2>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        // Trigger hurt animation
        animator.SetTrigger("isHurt");

    }

    void Die()
    {
        // Trigger death animation
        animator.SetTrigger("isDead");
        // Disable the player
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
