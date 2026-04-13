using UnityEngine;
namespace Pablo{

public class Bullet : MonoBehaviour
{
    public float velocidad = 10f;
    public int daño = 1;
    public float distanciaMaxima = 15f;
    private Vector2 posicionInicial;

    void Start() {
        posicionInicial = transform.position;
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * velocidad;
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //script del enemigo normal
            EnemyController enemigo = collision.GetComponent<EnemyController>();
            if (enemigo != null) {
                enemigo.TomarDaño(daño);
                SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxGolpeJugador);
            }

            //script del jefe
            BossController jefe = collision.GetComponent<BossController>();
            if (jefe != null) {
                jefe.TomarDaño(daño);
                SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxGolpeJugador);
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        //distancia entre el punto actual y el inicial
        float distanciaRecorrida = Vector2.Distance(posicionInicial, transform.position);

        if (distanciaRecorrida >= distanciaMaxima)
        {
            Destroy(gameObject);
        }
    }
}
}