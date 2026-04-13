using UnityEngine;
namespace Pablo{

public class BossController : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMax = 20;
    private int vidaActual;

    [Header("Movimiento")]
    public float velocidad = 3f;
    public float distanciaDetencion = 4f;
    public float rangoDeteccion = 15f;
    public bool activado = false;

    [Header("Salto")]
    public float fuerzaSalto = 10f;
    public Transform sueloCheck;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform jugador;
    private bool estaEnSuelo;

    private bool estaMuerto = false;
    private float timerHit;

    void Start()
    {
        vidaActual = vidaMax;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) jugador = p.transform;
    }

    void Update()
    {
        if (!activado || jugador == null || estaMuerto) return;
        if (timerHit > 0) timerHit -= Time.deltaTime;

        //detección de suelo para animaciones Air
        estaEnSuelo = Physics2D.OverlapCircle(sueloCheck.position, radioSuelo, capaSuelo);
        if (timerHit <= 0)
        {
            anim.SetBool("isGrounded", estaEnSuelo);
        }

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        //IA de Movimiento
        if (distanciaAlJugador < rangoDeteccion)
        {
            LogicaPersecucion(distanciaAlJugador);
        }
        else
        {
            //si el jugador se sale del rango, el jefe se queda quieto
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("isWalking", false);
        }
        
    }

    void LogicaPersecucion(float distancia)
    {
        float direccion = Mathf.Sign(jugador.position.x - transform.position.x);

        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direccion, transform.localScale.y, 1);

        if (distancia > distanciaDetencion)
        {
            rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);
            anim.SetBool("isWalking", estaEnSuelo);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("isWalking", false);
        }

        if (estaEnSuelo && jugador.position.y > transform.position.y + 2f)
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);  
            SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxSalto);
        }
    }

    public void ActivarJefe()
    {
        activado = true;
        SFXManager.Instance.PlayMusic(SFXManager.Instance.musicJefe);
        Debug.Log("El Jefe ha despertado");
    }

    public void TomarDaño(int cantidad)
    {
        if (estaMuerto) return;
        vidaActual -= cantidad;
        timerHit = 0.4f;
        
        if (vidaActual > 0)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isGrounded", true); 
            anim.SetTrigger("Hit");
        }
        else
        {
            Morir();
        }
    }

    void Morir()
    {
        estaMuerto = true;

        //desactivar script y físicas
        this.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;

        anim.SetBool("isWalking", false);
        anim.SetBool("isGrounded", true);
        anim.SetTrigger("Death");
        SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxMuerteJefe);
        Invoke("CargarFinal", 3f); 

        Debug.Log("Jefe derrotado.");
        Destroy(gameObject, 4f);
    }

    void CargarFinal()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndScene");
        }

    private void OnDrawGizmosSelected()
    {
        if (sueloCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(sueloCheck.position, radioSuelo);
        }
    }
}
}