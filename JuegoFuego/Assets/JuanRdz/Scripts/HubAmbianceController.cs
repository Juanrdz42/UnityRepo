using UnityEngine;

public class HubAmbianceController : MonoBehaviour
{
    [Header("Ambience")]
    public string ambienceName = "HubAmbiance";

    void Start()
    {
        Debug.Log("[HubAmbianceController] Starting ambience: " + ambienceName);

        if (SFXManager_JuanRdz.Instance == null)
        {
            Debug.LogError("[HubAmbianceController] SFXManager instance is NULL!");
            return;
        }

        SFXManager_JuanRdz.PlayAmbience(ambienceName);
    }

    void OnDestroy()
    {
        Debug.Log("[HubAmbianceController] Stopping ambience");
        SFXManager_JuanRdz.StopAmbience();
    }
}