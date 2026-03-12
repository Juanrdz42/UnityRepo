using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Vector2 input;
    private Animator animator;
    private SpriteRenderer sprite;

    [Header("Footsteps")]
    public float footstepInterval = 0.4f;
    private float footstepTimer;

    [Header("Movement Lock")]
    public bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector2 moveInput = canMove ? input : Vector2.zero;

        if (rb != null)
            rb.linearVelocity = moveInput * speed;

        bool isWalking = moveInput.magnitude > 0;

        if (animator != null)
            animator.SetBool("IsWalking", isWalking);

        if (sprite != null)
        {
            if (moveInput.x > 0)
                sprite.flipX = false;
            else if (moveInput.x < 0)
                sprite.flipX = true;
        }

        HandleFootsteps(isWalking);
    }

    void HandleFootsteps(bool isWalking)
    {
        if (!isWalking)
        {
            footstepTimer = 0f;
            return;
        }

        footstepTimer -= Time.fixedDeltaTime;

        if (footstepTimer <= 0f)
        {
            SFXManager_JuanRdz.Play("Walking");
            footstepTimer = footstepInterval;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            input = Vector2.zero;
            return;
        }

        input = context.ReadValue<Vector2>();
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
        input = Vector2.zero;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetBool("IsWalking", false);
    }
}