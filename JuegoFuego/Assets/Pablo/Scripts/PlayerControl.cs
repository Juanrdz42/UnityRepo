using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace Pablo{

public class PlayerController : MonoBehaviour
{
    [Header("Plantas")]
    public int plants = 0;
    public TextMeshProUGUI Plants_Text;

    [Header("Vidas")]
    public int lives = 3; 
    public Sprite spendLives;
    public Image[] livesImage;
    private bool isDead = false;

    [Header("Movimiento")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    
    [Header("Físicas y salto")]
    public float fallMultiplier = 3.5f; 
    public int maxSaltos = 2;           
    private int saltosRestantes;
    
    [Header("Detección de suelo")]
    public Transform groundCheck;       
    public float groundRadius = 0.2f;   
    public LayerMask groundLayer1;      
    public LayerMask groundLayer2;      
    private bool isGrounded;

    [Header("Ataque")]
    public GameObject balaPrefab; 
    public Transform puntoDisparo; 
    public float fireRate = 0.5f;  
    private float nextFireTime = 0f;

    public Rigidbody2D rig;
    private Animator animatorController;
    private SpriteRenderer sprite; 
    private float xInput;
    private bool jumpRequested;
    private float timerHit; 

    void Start() 
    {
        sprite = GetComponent<SpriteRenderer>();
        animatorController = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>(); 
        saltosRestantes = maxSaltos;
        if (Plants_Text != null) Plants_Text.text = "" + plants;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (isDead) return; //si muere, no hacer nada

        if (timerHit > 0) timerHit -= Time.deltaTime;

        bool onLayer1 = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer1);
        bool onLayer2 = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer2);
        isGrounded = onLayer1 || onLayer2;

        if (isGrounded && rig.linearVelocity.y <= 0.1f)
            saltosRestantes = maxSaltos;

        xInput = 0f;
        if(Keyboard.current.leftArrowKey.isPressed) xInput = -1f;
        else if(Keyboard.current.rightArrowKey.isPressed) xInput = 1f;

        if (xInput < 0)
        {
            sprite.flipX = true;
            puntoDisparo.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (xInput > 0)
        {
            sprite.flipX = false;
            puntoDisparo.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame && saltosRestantes > 0)
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxSalto);
            jumpRequested = true;
            saltosRestantes--;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && Time.time >= nextFireTime) 
        {
            Disparar();
            nextFireTime = Time.time + fireRate; 
        }

         if (timerHit <= 0) 
            {
                UpdatePlayerAnimation();
            }
    }

    public void FixedUpdate()
    {
        if (isDead) return;

        rig.linearVelocity = new Vector2(xInput * moveSpeed, rig.linearVelocity.y);

        if (jumpRequested)
        {
            rig.linearVelocity = new Vector2(rig.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }

        if (rig.linearVelocity.y < 0) 
            rig.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; //no recibir daño si esta muerto

        lives -= amount;

        animatorController.SetTrigger("Hit");
        timerHit = 0.4f; //bloquea las animaciones por 0.4 segundos
        
        //actualizar corazones UI
        for (int i = 0; i < livesImage.Length; i++)
        {
            if (i >= lives) livesImage[i].sprite = spendLives;
        }

        if (lives <= 0)
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxMuerteJugador);
            isDead = true;
            timerHit = 0f;

            rig.linearVelocity = Vector2.zero; 
            rig.bodyType = RigidbodyType2D.Static;
            animatorController.SetBool("isWalking", false);
            animatorController.SetBool("isJumping", false);

            animatorController.ResetTrigger("Hit");

            animatorController.SetTrigger("Death");
            Invoke("LlamarGameOver", 1.5f);
        }
        else
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxGolpeJugador);
            timerHit = 0.4f;
            animatorController.SetBool("isWalking", false);
            animatorController.SetBool("isJumping", false);
            animatorController.SetTrigger("Hit");
        }
    }

    void LlamarGameOver()
    {
        GameControl.Instance.CheckGameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trampa"))
        {
            TakeDamage(lives); 
        }
    }

    public void AddPlants(int amount)
    {
        plants += amount;
        if (Plants_Text != null) Plants_Text.text = "" + plants;
    }

    void Disparar() 
    {
        SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxDisparo);
        if(balaPrefab && puntoDisparo)
            Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
    }

    void UpdatePlayerAnimation() 
    {
        if (isDead) return;

        if (!isGrounded)
        {
            animatorController.SetBool("isWalking", false);
            animatorController.SetBool("isJumping", true);
        }
        else if (xInput != 0)
        {
            animatorController.SetBool("isWalking", true);
            animatorController.SetBool("isJumping", false);
        }
        else
        {
            animatorController.SetBool("isWalking", false);
            animatorController.SetBool("isJumping", false);
        }
    }
}
}