using UnityEngine;
namespace Pablo{
public class EnemyController : MonoBehaviour
{
    [Header("Estadísticas")]
    public int vida = 3;
    public float velocidad = 2f;
    public float distanciaPatrulla = 3f;
    
    [Header("Detección de suelo")]
    public Transform detecSuelo;
    public float distanciaRayo = 1f;
    public LayerMask capaSuelo;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 posicionInicial;
    private int direccion = 1; 
    private SpriteRenderer sprite;

    private bool estaMuerto = false;

    public GameObject plantaPrefab;

    void Start() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        posicionInicial = transform.position;
    }

    void Update() {
        Patrullar();
    }

    void Patrullar() {
        //rayo hacia abajo desde la posición del detecSuelo
        RaycastHit2D infoSuelo = Physics2D.Raycast(detecSuelo.position, Vector2.down, distanciaRayo, capaSuelo);

        //si el rayo no toca nada
        float distanciaActual = transform.position.x - posicionInicial.x;
        
        if (infoSuelo.collider == false || Mathf.Abs(distanciaActual) >= distanciaPatrulla) {
            Girar();
        }

        rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);
        anim.SetBool("isWalking", true);
    }

    void Girar() {
        direccion *= -1;
        posicionInicial.x = transform.position.x;

        //voltear el sensor de suelo
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void TomarDaño(int cantidad) {
        if (estaMuerto) return;
        vida -= cantidad;
        if (vida > 0) anim.SetTrigger("Hit");
        else Morir();
    }

    void Morir() {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxMuerteEnemigo);
        if (estaMuerto) return;
        estaMuerto = true;

        if (plantaPrefab != null)
        {
            Instantiate(plantaPrefab, transform.position, Quaternion.identity);
        }

        this.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; 
        anim.SetBool("isWalking", false);
        anim.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1.5f);
    }

    //dibuja el laser en el editor para poder verlo
    private void OnDrawGizmos() {
        if (detecSuelo != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(detecSuelo.position, Vector2.down * distanciaRayo);
        }
    }
}
}