using System.Runtime.CompilerServices;
using UnityEngine;

public class TogglePlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite onSprite; // el sprite completo
    public Sprite offSprite; // el outline
    public float onDuration = 3f; // segundos on
    public float offDuration = 2f; // segundos off (igual esto se cambia en unity)
    public bool startOn = true; // empieza en on
    private SpriteRenderer sr;
    private Collider2D col;
    private bool isOn; // checa si esta on o no
    private float timer;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }
    void Start()
    {
        isOn = startOn;
        col.enabled = isOn; // si esta en on el collider funciona, sino no
        sr.sprite = isOn ? onSprite : offSprite; // cambia el dibujo depende de si esta on o no
        timer = isOn? onDuration : offDuration; // pone el reloj el tiempo que le toca a cada uno
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            // cambio, si estaba on ahora off y alrevez
            isOn = !isOn;
            col.enabled = isOn;
            sr.sprite = isOn ? onSprite : offSprite;
            timer = isOn ? onDuration : offDuration;
        }
    }
}
