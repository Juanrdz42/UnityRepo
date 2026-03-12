using UnityEngine;

public class BirdBehaviour_JuanRdz : MonoBehaviour
{
    public float leftLimit = -1.5f;
    public float rightLimit = 1.5f;
    public float speed = 2f;

    private bool movingRight = true;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 localPos = transform.localPosition;

        if (movingRight)
        {
            localPos.x += speed * Time.deltaTime;

            if (localPos.x >= rightLimit)
            {
                localPos.x = rightLimit;
                movingRight = false;
            }
        }
        else
        {
            localPos.x -= speed * Time.deltaTime;

            if (localPos.x <= leftLimit)
            {
                localPos.x = leftLimit;
                movingRight = true;
            }
        }

        transform.localPosition = localPos;

        // Si por defecto el pájaro mira a la izquierda
        if (movingRight)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }
}