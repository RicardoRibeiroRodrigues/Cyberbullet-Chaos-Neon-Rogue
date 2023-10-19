using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    
    private GameObject player;
    private Animator animator;
	public int attackCooldown;
    private Rigidbody2D m_Rigidbody;
    private bool isMoving;
	// Attack mechanic.
	private bool playerInRange;
	private bool canAttack;
    private bool canUseSpecialAttack;
    [SerializeField]
    private float attackRange;
    public Transform attackPoint;
    public GameObject projectilePrefab;
    // Enemy stats -> Damage in on enemys projectile
    public int health;
    public float moveSpeed;
    public float specialAttackCooldown;

    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        canAttack = true;
        canUseSpecialAttack = true;
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
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
		} else if (playerInRange && canUseSpecialAttack)
        {
            SpecialAttack();
        }
    }

	IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator SpecialAttackCooldown()
    {
        canUseSpecialAttack = false;
        yield return new WaitForSeconds(specialAttackCooldown);
        canUseSpecialAttack = true;
    }

    void Attack(Vector3 direction)
    {
        animator.SetTrigger("Attack");
        StartCoroutine(AttackCooldown());
        direction.Normalize();
        var bullet = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
        bullet.GetComponent<ProjectileController>().direction = direction;
    }

    void SpecialAttack()
    {
        animator.SetTrigger("Special");
        StartCoroutine(SpecialAttackCooldown());
    }
}
