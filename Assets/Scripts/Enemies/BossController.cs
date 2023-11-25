using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IEnemy
{
    
    private GameObject player;
    private Animator animator;
	public float attackCooldown;
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
    private float normalSpeed;
    public float specialAttackCooldown;
    private bool isFreezing;
    public int damage;
    public int specialAttackDamage;
    public bool isDying { get; set; }
    // Special attack
    public GameObject specialAttackPrefab;
    public GameObject specialAttackExplosionPrefab;
    private GameObject specialAttack;

    public bool offScreen { get; set; } = true;

    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        canAttack = true;
        canUseSpecialAttack = true;
        player = GameObject.Find("Player");
        normalSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        if (player == null || isFreezing || isDying)
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
        bullet.GetComponent<ProjectileController>().damage = damage;
    }

    void SpecialAttack()
    {
        animator.SetTrigger("Special");
        StartCoroutine(SpecialAttackCooldown());
        Vector2 spawnPos = player.transform.position;
        spawnPos += Random.insideUnitCircle.normalized * 0.35f;
        // Shoot at a random position in the screen
        specialAttack = Instantiate(specialAttackPrefab, spawnPos, Quaternion.identity);
        specialAttack.transform.localScale = new Vector3(10f, 10f, 10f) * 2;
        specialAttack.GetComponent<BossSpecial>().damage = specialAttackDamage;

        Invoke(nameof(ShootSpecialExplosion), 1f);
    }

    void ShootSpecialExplosion()
    {
        Vector3 transform_position = specialAttack.transform.position;
        Quaternion rotation_position = specialAttack.transform.rotation;
        Vector3 transform_right = specialAttack.transform.right;

        // Actite collider
        specialAttack.GetComponent<CircleCollider2D>().enabled = true;

        var explosion = Instantiate(specialAttackExplosionPrefab, transform_position, rotation_position);
        explosion.transform.localScale = new Vector3(1f, 1f, 1f);
        // Damage, is given in the controller
        explosion.GetComponent<ProjectileController>().damage = 0;

        Destroy(specialAttack, 0.6f);
    }

    void FinishedDyingAnimation()
    {
        var playerController = player.GetComponent<PlayerController>();
        GameManager.Instance.endGame(true, playerController.level * 25 + 200);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (isDying)
            return;
        health -= damage;
        animator.SetTrigger("Hurt");
        if (health <= 0)
        {
            GetComponent<AudioSource>().Play();
            isDying = true;
            animator.SetTrigger("Dying");
            Invoke(nameof(FinishedDyingAnimation), 1.5f);
        }
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

    public void resetEnemy()
    {
    }

    public void ChangeSpeed()
    {
        if (offScreen)
            moveSpeed = 10;
        else
            moveSpeed = normalSpeed;
    }
}
