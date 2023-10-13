using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    private Animator animator;
    private Rigidbody2D m_Rigidbody;
    private bool isMoving;
    public float moveSpeed;
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var targetPos = player.transform.position;
        StartCoroutine(Move(targetPos));
        animator.SetBool("isMoving", isMoving);
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        m_Rigidbody.MovePosition(Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime));
        yield return null;
        isMoving = false;
    }
}
