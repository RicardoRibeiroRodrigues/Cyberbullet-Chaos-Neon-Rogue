using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
   	public float attackCooldown;
    private Rigidbody2D m_Rigidbody;
    private bool isMoving;
    private bool isDying;
    // Can take damage
    public bool canTakeDamage = true;
	// Attack mechanic.
	private bool playerInRange;
	private bool canAttack;
    // Enemy stats
    private int max_health;
    public int health;
    public int damage;
    public float moveSpeed;
    // Drop orb
    public GameObject orbPrefab;
    public GameObject weaponUpgradeBoxPrefab;
    public GameObject ExtraLifePrefab;
    private bool isFreezing;
    public void TakeDamage(int damage)
    {
        // Evita bug de morrer duas vezes.
        if (isDying || !canTakeDamage)
            return;
        
        health -= damage;
        if (health <= 0)
        {
            Die();
        } else {
            // Trigger hurt animation
            animator.SetTrigger("Hurt");
        }
        StartCoroutine(TakeDamageCooldown());
    }
    IEnumerator TakeDamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }

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
        // Evita de o inimigo se mover e atacar depois de morrer.
        if (isDying || player == null)
            return;
        
        if (isFreezing) {
            // Locks the enemy in place x, y and z.
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        
        var targetPos = player.transform.position;
        var distance = Vector2.Distance(transform.position, targetPos);
        var direction = targetPos - transform.position;
        if (direction.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
        
        // Para o inimigo nao ficar tentanto entrar no player.
        if (distance >= 0.5f && !isFreezing)
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

		if (playerInRange && canAttack && !isFreezing)
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
        // Se o nome for MiniBoss
        if (gameObject.name == "MiniBoss")
        {
            // Spawn weapon upgrade box
            Instantiate(weaponUpgradeBoxPrefab, transform.position, transform.rotation);
        } else if (Random.Range(0, 100) <= 3)
        {
            // Spawn extra life
            Instantiate(ExtraLifePrefab, transform.position, transform.rotation);
        } else {
            var orb = Instantiate(orbPrefab, transform.position, transform.rotation);
            // Scale xp with enemy health.
            orb.GetComponent<XpOrbController>().SetXp(max_health / 2);
        }
        
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
        Invoke(nameof(FinishedDyingAnimation), 3f);
    }

    public void Freeze(float freezeDuration)
    {
        isFreezing = true;
        // Make the enemy blue
        GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 1);
        StartCoroutine(FreezeDuration(freezeDuration));
    }

    IEnumerator FreezeDuration(float freezeDuration)
    {
        yield return new WaitForSeconds(freezeDuration);
        isFreezing = false;
        // Return the enemy to normal color
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
}
