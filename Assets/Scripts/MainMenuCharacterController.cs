using UnityEngine;

public class MainMenuCharacterController : MonoBehaviour
{
    public float Speed = 1.0f;
    public Vector3 Target = Vector3.zero;

    private SpriteRenderer Renderer;
    private Animator animator;
    private Vector3 Velocity = Vector3.zero;

    public enum state
    {
        HomeScreen,
        CharacterScreen,
        WeaponScreen,
    }

    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Target = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Target, ref Velocity, 0.0f, Speed);
        Renderer.flipX = Velocity.x < 0.0f;

        animator.SetBool("isMoving", Velocity.x != 0.0f);
    }
}
