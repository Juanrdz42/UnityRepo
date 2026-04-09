using UnityEngine;

public class FollowingEnemy : MonoBehaviour
{    
    public Transform player;
    public int value;
    public float speed;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        float direction = player.position.x - transform.position.x;
        float xInput = 0f;

        if (direction < -0.1f)
            xInput = -1f;
        else if (direction > 0.1f)
            xInput = 1f;

        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);

        // rotar
        if (xInput > 0)
            sprite.flipX = true;
        else if (xInput < 0)
            sprite.flipX = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameControl.Instance.TakeDamage(value); 
            GameControl.Instance.sfxManager.BadSound();
            }
    }
}