using System.Collections;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour, IEnemy
{
    private GameObject player;
    private Animator animator;
	public int attackCooldown;
    private Rigidbody2D m_Rigidbody;
    private bool isMoving;
	// Attack mechanic.
	private bool playerInRange;
	private bool canAttack;
    // Can take damage
    public bool canTakeDamage = true;
    [SerializeField]
    private float attackRange;
    public Transform attackPoint;
    public GameObject projectilePrefab;
    // Enemy stats -> Damage in on enemys projectile
    public int health;
    private int max_health;
    public float moveSpeed;
    private float normalSpeed;
    // Xp drop
    public bool isDying { get; set; }
    public GameObject orbPrefab;
    public GameObject ExtraLifePrefab;
    private bool isFreezing;
    public bool offScreen { get; set; } = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        canAttack = true;
        player = GameObject.Find("Player");
        // Set enemy stats with difficulty
        var difficulty = GameManager.Instance.difficulty;
        health = (int) (health * difficulty);
        moveSpeed += moveSpeed * ((1 - difficulty) / 2);
        max_health = health;
        normalSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        if (isDying || player == null)
            return;
        if (isFreezing) {
            // Locks the enemy in place x, y and z.
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        
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
            ChangeSpeed();
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
        if (!gameObject.activeInHierarchy)
            return;
        
        if (Random.Range(0, 100) <= 3)
        {
            // Spawn extra life
            Instantiate(ExtraLifePrefab, transform.position, transform.rotation);
        } else {
            var orb = Instantiate(orbPrefab, transform.position, transform.rotation);
            // Scale xp with enemy health.
            orb.GetComponent<XpOrbController>().SetXp(max_health / 2);
        }
        // Destroy(gameObject);
        gameObject.SetActive(false);
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
        var bulletController = bullet.GetComponent<ProjectileController>();
        bulletController.direction = direction;
        bulletController.damage = (int) (bulletController.damage * GameManager.Instance.difficulty);
    }

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

    void Die()
    {
        m_Rigidbody.velocity = Vector3.zero;
        isDying = true;
        // Trigger death animation
        animator.SetTrigger("Dying");
        // Disable the enemy
        GetComponent<Collider2D>().enabled = false;
    }

    public void Freeze(float freezeDuration)
    {
        isFreezing = true;
        GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 1);
        StartCoroutine(FreezeDuration(freezeDuration));
    }

    IEnumerator FreezeDuration(float freezeDuration)
    {
        yield return new WaitForSeconds(freezeDuration);
        isFreezing = false;
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    // Reset Enemy for pooling
    public void resetEnemy()
    {
        isDying = false;
        canTakeDamage = true;
        health = max_health;
        isFreezing = false;
        offScreen = true;
        GetComponent<Collider2D>().enabled = true;
    }

    public void ChangeSpeed()
    {
        if (offScreen)
            moveSpeed = 10;
        else
            moveSpeed = normalSpeed;
    }
}
