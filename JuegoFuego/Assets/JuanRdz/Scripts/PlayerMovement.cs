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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * speed;

        bool isWalking = input.magnitude > 0;
        animator.SetBool("IsWalking", isWalking);

        if (input.x > 0)
        {
            sprite.flipX = false;
        }
        else if (input.x < 0)
        {
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
        input = context.ReadValue<Vector2>();
    }
}