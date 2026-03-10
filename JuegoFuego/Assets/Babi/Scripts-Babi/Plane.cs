using UnityEngine;

public class Plane : MonoBehaviour
{
    public float flySpeed = 5f; // que tan rapido sube y baja (lo ajuste en unity)
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // jjaja para que no se caiga
    }

    void Update()
    {
        // ahora checa si le estoy picando arriba(1) o abajo(-1)
        float moveInput = Input.GetAxisRaw("Vertical"); 
        
        // le da la velocidad para que se mueva en y, y en x se queda en 0
        rb.linearVelocity = new Vector2(0, moveInput * flySpeed);
        // este es un tilt para que si voy paara arriba el aviion se incline 15 grados y para abajo -15
        float tilt = Input.GetAxisRaw("Vertical") * 15f;
        // la rotacion es con Quaternion que es la forma que unity usa para los angulos
        transform.rotation = Quaternion.Euler(0, 0, tilt);
    }
}