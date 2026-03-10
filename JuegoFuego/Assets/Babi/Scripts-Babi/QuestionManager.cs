using UnityEngine;
using System.Collections.Generic; // este deja usar listas

public class QuestionManager : MonoBehaviour
{
    [System.Serializable] // para que las preguntas salgan en unity
    public class PreguntaData {
        public string enunciado;
        public string opcion1; 
        public string opcion2;
        public string opcion3;
    }

    // la biblioteca
    // aqui guarda todas las preguntsa que hice
    public List<PreguntaData> listaDePreguntas;

   // esta funcion busca la pregunta
    public PreguntaData GetCurrentQuestion() {
        // checa en que bandera esta (-1 porque empieza de 0)
        int index = GameData.PreguntaActualID - 1;
        // checa que si existe
        if (index >= 0 && index < listaDePreguntas.Count)
        {
            return listaDePreguntas[index]; // manda la pregunta correcta
        }
        // si algo sale mal no manda nada
        return null;
    }
}

// aqui decimos que cda pregunta tiene una pregunta y 3 opciones. en unity yo le puedo poner las preguntas que quisiera
// le pongo 5 porque nadamas tengo 5 niveles
// por ejemplo cuando el jugador llega a la bandera 3 el script busca la pregunta 2 y le dice que poner en la pantalla