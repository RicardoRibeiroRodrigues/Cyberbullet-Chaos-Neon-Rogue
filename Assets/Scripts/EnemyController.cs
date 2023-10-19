using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
   	public int attackCooldown;
    private Rigidbody2D m_Rigidbody;
    private bool isMoving;
    private bool isDying;
	// Attack mechanic.
	private bool playerInRange;
	private bool canAttack;
    // Enemy stats
    public int health;
    public int damage;
    public float moveSpeed;
    
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

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        canAttack = true;
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        // Evita de o inimigo se mover e atacar depois de morrer.
        if (isDying)
            return;
        
        var targetPos = player.transform.position;
        var distance = Vector2.Distance(transform.position, targetPos);
        var direction = targetPos - transform.position;
        if (direction.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
        
        // Para o inimigo nao ficar tentanto entrar no player.
        if (distance >= 0.5f)
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
            animator.SetTrigger("Attack");
            StartCoroutine(AttackCooldown());
            player.GetComponent<PlayerController>().TakeDamage(damage);
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
        Destroy(gameObject);
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
