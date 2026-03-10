using UnityEngine;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    [System.Serializable]
    public class PreguntaData {
        public string enunciado;
        public string opcion1; // Cambiado de 'correcta' a 'opcion1'
        public string opcion2;
        public string opcion3;
    }

    public List<PreguntaData> listaDePreguntas;

    public PreguntaData GetCurrentQuestion() {
        int index = GameData.PreguntaActualID - 1;
        if (index >= 0 && index < listaDePreguntas.Count) return listaDePreguntas[index];
        return null;
    }
}