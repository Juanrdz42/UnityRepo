using UnityEngine;

public class Door : MonoBehaviour
{
    void Start()
    {
        // En cuanto la escena carga, la puerta revisa si ya fue abierta antes
        if (GameData.puertaAbierta)
        {
            Destroy(gameObject);
        }
    }
}