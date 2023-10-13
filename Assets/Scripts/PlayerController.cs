using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;
    private Vector2 Movement;
    private Animator animator;
    private Rigidbody2D m_Rigidbody;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

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
}
