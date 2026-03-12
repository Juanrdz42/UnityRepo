using UnityEngine;
using System.Collections; 
using UnityEngine.UI;
using TMPro;

public class PlayerBabi : MonoBehaviour
{
    public int plants = 0;
    public TextMeshProUGUI Plants_Text;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int health = 100;
    

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f; // que tan grande es el circulo para sentir el piso
    public LayerMask groundLayer;
    

    public int extraJumpsValue = 1; // cuantos saltos extra puede hacer en el aire
    private int extraJumps; // el contador

    
    private Rigidbody2D rb; 
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    public Image healthImage;
    private AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip hurtClip;


    public float coyoteTime = 0.2f; // tiempo extra para saltar
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.15f; // buffer para saltar antes de tocar el ground
    private float jumpBufferCounter;



    void Start()
{
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    spriteRenderer = GetComponent<SpriteRenderer>(); 
    audioSource = GetComponent<AudioSource>();

    extraJumps = extraJumpsValue;

    if (GameData.regresarDeNubes) // si viene de las nubes
    {
        // se regresa a donde estaba y se pone las plantas que ya tenia
        // saca la info del script GameData
        transform.position = GameData.posicionRetorno;
        this.plants = GameData.plantasGuardadas; 
        GameData.regresarDeNubes = false;
    }
    // si es la primera vez que empieza el juego o se muere, solo se acuerda de las plants
    else 
    {
        this.plants = GameData.plantasGuardadas; 
    }

    // pone el numero de plantas en el UI
    if (Plants_Text != null) 
    {
        Plants_Text.text = this.plants.ToString(); 
    }
}

    void Update()
    {
        // checa si le estoy picando izq o derecha
        float moveInput = Input.GetAxis("Horizontal");  

        if (isGrounded) // si esta en el piso
        {
            // reset el tiempo de coyote y los extra jumps
            coyoteTimeCounter = coyoteTime;
            extraJumps = extraJumpsValue;
        }
        else // si esta en el aire
        {
            coyoteTimeCounter -= Time.deltaTime; // el tiempo se acaba
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) // aqui si le pica a la up key se guarda que quiere saltar
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

    
        if (jumpBufferCounter > 0f) // y luego aqui checa, si queria saltar
        {
            if (coyoteTimeCounter > 0f) // checa si todavia se puede (se puede si estas en el piso o en el coyotetime)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                PlaySFX(jumpClip);
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
            else if (extraJumps > 0) // si no, checa si tienes saltos extras
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
                PlaySFX(jumpClip);
                jumpBufferCounter = 0f;
            }
        }

        // voltea al character dependiendo del lado
        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        SetAnimation(moveInput); // aqui le dice que animation poner
        healthImage.fillAmount = health / 100f; // actualiza el health

        if (transform.position.y < -27) // si se cae, cuando pasa el y=-27 se muere
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        // checa si los pies estan en el piso
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveInput = Input.GetAxisRaw("Horizontal"); // se supone que con Raw es más seco el frenado

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            // se le estoy picando para que se mueba le aplic fuerza
            rb.AddForce(new Vector2(moveInput * moveSpeed * 50, 0f), ForceMode2D.Force);
            // pero aqui lo limita a que no vaya más rapido que lo que le estamos diciendo en el movespeed
            rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -moveSpeed, moveSpeed), rb.linearVelocity.y);
        }
        else if (isGrounded)
        {
            // si no le estoy picando a nada y está en el piso solito se va frenando
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.95f, rb.linearVelocity.y); 
        }
    }

    private void SetAnimation(float moveInput)
    {
        string anim;
        if (isGrounded)
        {
            // si se queda sill es idle, si se mueve walk
            anim = (moveInput == 0) ? "Player_Idle" : "Player_Walk";
        }
        else
        {
            // si salta jump si se va para abajo fall
            anim = (rb.linearVelocity.y > 0) ? "Player_Jump" : "Player_Fall";
        }

       // aqui le dice que si la anmacion que queremos no es la que se esta poniendo la cambia 
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
        {
            animator.Play(anim);
        }
    }

    // cuando choca de frente más que nada
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage")) // si toca los piquitos
        {
            PlaySFX(hurtClip);
            health -= 25;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // pequeño rebote
            StartCoroutine(BlinkRed()); // lo pone rojo cierto time

            if (health <= 0) Die(); // si ya no hay vida se muere
        }
        else if (collision.gameObject.CompareTag("BouncePad")) // si toca el bounce pad
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 2); // salta alto *2
            Animator padAnim = collision.gameObject.GetComponentInChildren<Animator>();

            if (padAnim != null)
            {
                padAnim.Play("Bounce",0,0f);
            }
        }
       // hize otro bouncepad que saltara más alto y en unity nadamas le cambio el tag dependiendo que tanto quiero que salte
        else if (collision.gameObject.CompareTag("BouncePad2"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 3); 
            Animator padAnim = collision.gameObject.GetComponentInChildren<Animator>();

            if (padAnim != null)
            {
                padAnim.Play("Bounce",0,0f);
            }
        }
        
    }

    // más para cosas qye se "colleccionan"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Energy")
        {
            health += 10;
            if (health > 100) // no se puede tener más de 100
            {
                health = 100;
            }
            Destroy(collision.gameObject);
        }
    }

    // para que cambie de color cuando pierde vida
    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
{
    if (GameData.tieneCheckpoint) // si se muere checa si ya habia pasado un checkpoint
    {
        // regresa al último checkpoint
        transform.position = GameData.ultimoCheckpointPos;
        health = 100; 
        rb.linearVelocity = Vector2.zero; // se queda quieto cuando vuelve a apareceer
        FallingPlatform[] plataformas = Object.FindObjectsByType<FallingPlatform>(FindObjectsSortMode.None);
        foreach (FallingPlatform p in plataformas) {
        p.ResetPlatform();
    }
        
    }
    else
    {
        // si no toco ninguna bandera, entonces desde el principio
        UnityEngine.SceneManagement.SceneManager.LoadScene("Mini1");
    }
}

    public void PlaySFX(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}