using UnityEngine;
namespace Pablo{

public class RoomCameraTrigger : MonoBehaviour
{
    public GameObject miCamara; 
    
    [Header("Solo para la habitación del Jefe")]
    public BossController scriptDelJefe;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Camera[] todasLasCams = GameObject.FindObjectsOfType<Camera>(true);
            foreach (Camera cam in todasLasCams)
            {
                cam.gameObject.SetActive(false);
            }
            miCamara.SetActive(true);

            if (scriptDelJefe != null) 
            {
                scriptDelJefe.ActivarJefe();
            }
        }
    }
}
}