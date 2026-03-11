using UnityEngine;
using TMPro;

public class CloudSetup : MonoBehaviour 
{
   //lista de los texts que tienen las nubes, se necesitan 3 uno para cada opcion
    public TextMeshPro[] textosNubes; 

    void Start() {
        // busca al empty object que tiene el question manager script
        QuestionManager qm = Object.FindFirstObjectByType<QuestionManager>();
        
        // si no lo pone pues regresa nada
        if (qm == null) {
            return;
        }

        // aqui le pide a la biblioteca(qm) que le de la pregunta que va a poner
        var data = qm.GetCurrentQuestion();

        // si encuentra la pregunta y hay suficientes texts empieza a poner las opciones en los texts de las 3 nubes
        if(data != null && textosNubes.Length >= 3) {
            textosNubes[0].text = data.opcion1;
            textosNubes[1].text = data.opcion2;
            textosNubes[2].text = data.opcion3;
        }
    }
}