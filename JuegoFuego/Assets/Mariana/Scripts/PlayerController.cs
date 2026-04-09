using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; 
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static bool firstTime = false;
    public int plants = 0;
    public int water = 0;
    public TextMeshProUGUI plantsText;
    public TextMeshProUGUI waterText;
    public int health = 100;
    public float moveSpeed;
    public float jumpForce;
    public int extraJumpsValue = 2; // cuantos saltos extra puede hacer en el aire
    private int extraJumps; // el contador

    public Rigidbody2D rig;

    private float xInput;
    private bool jumpRequested;

    Animator animatorController;
    private bool isGrounded = true;
    private SpriteRenderer sprite; 
    public Image healthImage;

    void Start() // inicializa los componentes
    {
        sprite = GetComponent<SpriteRenderer>();
        animatorController = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>(); 
        extraJumps = extraJumpsValue;
        UpdateHealthUI(); // inicializa la barra
        plantsText.text = plants.ToString();
        waterText.text = water.ToString();
    }

    public enum PlayerAnimation
    {
        Idle, Run, Jump, Hit  // estados
    }

    void UpdateAnimation(PlayerAnimation nameAnimation) // cambia los parámetros 
    {
        switch (nameAnimation)
        {
            case PlayerAnimation.Idle:
                animatorController.SetBool("isWalking", false);
                animatorController.SetBool("isJumping", false);
                break;
            case PlayerAnimation.Run:
                animatorController.SetBool("isWalking", true);
                animatorController.SetBool("isJumping", false);
                break;
            case PlayerAnimation.Jump:
                animatorController.SetBool("isWalking", false);
                animatorController.SetBool("isJumping", true);
                break;
            case PlayerAnimation.Hit:
                animatorController.SetBool("isWalking", false);
                animatorController.SetBool("isJumping", false);
                animatorController.SetBool("isHit", true);
                break;
        }
    }
    void UpdatePlayerAnimation() // cuál es la siguiente animación
    {
        if (animatorController.GetBool("isHit")) return; // no interrumpe hit

        if (!isGrounded)
            UpdateAnimation(PlayerAnimation.Jump);
        else if (xInput != 0)
            UpdateAnimation(PlayerAnimation.Run);
        else
            UpdateAnimation(PlayerAnimation.Idle);
    }
    void Update()
    {
        if (isGrounded) // si esta en el piso
        {
            extraJumps = extraJumpsValue;
        }

        xInput = 0f;
        if(Keyboard.current.leftArrowKey.isPressed)
            xInput = -1f;
        else if(Keyboard.current.rightArrowKey.isPressed)
            xInput = 1f;

        // rotar
        if (xInput < 0)
            sprite.flipX = true;
        else if (xInput > 0)
            sprite.flipX = false;

        
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            if (isGrounded)
            {
                jumpRequested = true;
                isGrounded = false;
            }
            else if (extraJumps > 0) // doble salto
            {
                jumpRequested = true;
                extraJumps--;
            }
        }
        // actualiza las animaciones 
        UpdatePlayerAnimation();
    }
    public void FixedUpdate()
    {
        rig.linearVelocity = new Vector2(xInput * moveSpeed, rig.linearVelocity.y); // velocidad horizontal
        
        if(jumpRequested)
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequested = false;
        }

        if (Mathf.Abs(rig.linearVelocity.y) < 0.01f) // detecta el suelo
        {
            isGrounded = true;
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -20f, 20f); // limita el movimiento horizontal
        transform.position = pos;
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, 100);
        UpdateHealthUI();
        PlayerPrefs.SetInt("Lives", health);
        StartCoroutine(HitAnimation());

        if (health <= 0)
            SceneManager.LoadScene("endScene");
    }

    IEnumerator HitAnimation()
    {
        animatorController.SetBool("isHit", true);
        yield return new WaitForSeconds(0.4f); // ajusta según duración del clip de Hit
        animatorController.SetBool("isHit", false);
    }

    public void AddPlants(int amount)
    {
        plants += amount;
        plantsText.text = plants.ToString();
    }

    public void AddWater(int amount)
    {
        water += amount;
        waterText.text = water.ToString();
    }

    void UpdateHealthUI()
    {
        if (healthImage != null)
            healthImage.fillAmount = health / 100f;
    }
}

