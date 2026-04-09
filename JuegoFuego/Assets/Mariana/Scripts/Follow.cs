using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform player;
    public float minX; // limite izq
    public float maxX;// lmite der
    public float smoothSpeed = 5f; // movimiento continuo

    private float fixedY;
    private float fixedZ;

    void Start()
    {
        // guarda la posición original en Y y Z
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    void LateUpdate()
    {
        // solo sigue al jugador en X
        float targetX = player.position.x;

        // límites
        targetX = Mathf.Clamp(targetX, minX, maxX);

        float smoothedX = Mathf.Lerp(transform.position.x, targetX, smoothSpeed * Time.deltaTime);

        // nueva posicio2n 
        transform.position = new Vector3(smoothedX, fixedY, fixedZ);
    }
}
