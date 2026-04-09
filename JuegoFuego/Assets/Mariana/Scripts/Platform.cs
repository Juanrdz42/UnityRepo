using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlatformSpawner spawner;
    public Vector3 myPosition;
    public float timeBeforeDisappear = 3f; // tiempo antes de desaparecer
    public float blinkDuration = 1f; // cuánto tiempo parpadea antes de destruirse
    private bool triggered = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(BlinkAndDestroy(other.transform));
        }
    }

    IEnumerator BlinkAndDestroy(Transform player)
{
    Debug.Log("Iniciando parpadeo, espera: " + (timeBeforeDisappear - blinkDuration));
    
    float waitTime = Mathf.Max(0, timeBeforeDisappear - blinkDuration);
    yield return new WaitForSeconds(waitTime);

    float elapsed = 0f;
    while (elapsed < blinkDuration)
    {
        if (sr != null) sr.enabled = !sr.enabled;
        yield return new WaitForSeconds(0.15f);
        elapsed += 0.15f;
    }

    Debug.Log("Destruyendo plataforma");
    
    if (player != null)
        player.SetParent(null);
    
    if (spawner == null)
        Debug.LogError("Spawner es null");
    else
        spawner.PlatformDestroyed(myPosition);
        
    Destroy(gameObject);
}
}