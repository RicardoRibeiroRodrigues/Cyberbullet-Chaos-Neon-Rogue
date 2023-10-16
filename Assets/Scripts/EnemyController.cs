using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    private Animator animator;
	public int attackCooldown;
    private Rigidbody2D m_Rigidbody;
    private bool isMoving;
    public float moveSpeed;
	// Attack mechanic.
	private bool playerInRange;
	private bool canAttack;
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        canAttack = true;
    }

    void FixedUpdate()
    {
        var targetPos = player.transform.position;
        StartCoroutine(Move(targetPos));
        animator.SetBool("isMoving", isMoving);

		if (playerInRange && canAttack)
		{
            Debug.Log("Ja pode ataca");
            animator.SetTrigger("isAttacking");
            StartCoroutine(AttackCooldown());
		}
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        m_Rigidbody.MovePosition(Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime));
        yield return null;
        isMoving = false;
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
}
