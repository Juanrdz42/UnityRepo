using UnityEngine;

public class Plane : MonoBehaviour
{
    public float flySpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void Update()
    {
        // Detectamos flechas o W/S
        float moveInput = Input.GetAxisRaw("Vertical"); 
        
        // Aplicamos la velocidad
        rb.linearVelocity = new Vector2(0, moveInput * flySpeed);
        // Añade esto al Update del avión
        float tilt = Input.GetAxisRaw("Vertical") * 20f; // 20 grados de inclinación
        transform.rotation = Quaternion.Euler(0, 0, tilt);
    }
}