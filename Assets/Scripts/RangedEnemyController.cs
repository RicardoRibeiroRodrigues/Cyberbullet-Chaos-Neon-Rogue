using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
	public int attackCooldown;
    private Rigidbody2D m_Rigidbody;
    private bool isMoving;
	// Attack mechanic.
	private bool playerInRange;
	private bool canAttack;
    [SerializeField]
    private float attackRange;
    public Transform attackPoint;
    public GameObject projectilePrefab;
    // Enemy stats -> Damage in on enemys projectile
    public int health;
    private int max_health;
    public float moveSpeed;
    // Xp drop
    private bool isDying;
    public GameObject orbPrefab;

    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        canAttack = true;
        player = GameObject.Find("Player");
        max_health = health;
    }

    void FixedUpdate()
    {
        if (isDying)
            return;
        
        var targetPos = player.transform.position;
        // if is already in a certain dist, stop moving
        var distance = Vector2.Distance(transform.position, targetPos);
        var direction = targetPos - transform.position;
        if (direction.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
        
        if (distance >= attackRange)
        {
            // Ajusta a velocidade do inimigo
            direction.Normalize();
            m_Rigidbody.velocity = direction * moveSpeed;
            isMoving = true;
            playerInRange = false;
        } else {
            // Para de mover se estiver perto o suficiente do player
            m_Rigidbody.velocity = Vector3.zero;
            isMoving = false;
            playerInRange = true;
        }
        animator.SetBool("isMoving", isMoving);
        

		if (playerInRange && canAttack)
		{
            Attack(direction);
		}
    }

	IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Usado no evento da animacao de morrer.
    void FinishedDyingAnimation()
    {
        var orb = Instantiate(orbPrefab, transform.position, transform.rotation);
        // Scale xp with enemy health.
        orb.GetComponent<XpOrbController>().SetXp(max_health / 2);
        Destroy(gameObject);
    }

    void Attack(Vector3 direction)
    {
        var targetY = 0;
        if (direction.y > 0.5)
            targetY = 1;
        else if (direction.y < -0.5)
            targetY = -1;
        animator.SetInteger("AttackDir", targetY);
        animator.SetTrigger("Attack");
        StartCoroutine(AttackCooldown());
        direction.Normalize();
        var bullet = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
        bullet.GetComponent<ProjectileController>().direction = direction;
    }

    public void TakeDamage(int damage)
    {
        // Evita bug de morrer duas vezes.
        if (isDying)
            return;
        
        health -= damage;
        if (health <= 0)
        {
            Die();
        } else {
            // Trigger hurt animation
            animator.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        m_Rigidbody.velocity = Vector3.zero;
        isDying = true;
        // Trigger death animation
        animator.SetTrigger("Dying");
        // Disable the enemy
        GetComponent<Collider2D>().enabled = false;
    }
}
