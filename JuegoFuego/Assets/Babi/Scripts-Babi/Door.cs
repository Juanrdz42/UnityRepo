using UnityEngine;

public class Door : MonoBehaviour
{
    void Start()
    {
        //checa si ya se abrio
        if (GameData.puertaAbierta)
        {
            Destroy(gameObject);
        }
    }
}

// sin este scipt cada que regresa de las nubes volvia a aparecer
// porque la llave dejo la info en el gamegata diciendo que ya fue abierta (o no vd)