using UnityEngine;
using TMPro;

public class CloudSetup : MonoBehaviour 
{
    public TextMeshPro[] textosNubes; 

    void Start() {
        QuestionManager qm = Object.FindFirstObjectByType<QuestionManager>();
        
        if (qm == null) {
            Debug.LogError("No se encontró el QuestionManager en esta escena.");
            return;
        }

        var data = qm.GetCurrentQuestion();

        if(data != null && textosNubes.Length >= 3) {
            // Ahora los nombres coinciden con el QuestionManager
            textosNubes[0].text = data.opcion1;
            textosNubes[1].text = data.opcion2;
            textosNubes[2].text = data.opcion3;
        }
    }
}