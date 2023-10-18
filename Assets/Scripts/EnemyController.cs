using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
   	public int attackCooldown;
    private Rigidbody2D m_Rigidbody;
    private bool isMoving;
	// Attack mechanic.
	private bool playerInRange;
	private bool canAttack;
    // Enemy stats
    public int health;
    public int damage;
    public float moveSpeed;
    public GameObject deathEffect;
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
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
		}
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

	IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
