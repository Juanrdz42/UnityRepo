using UnityEngine;
using System.Collections.Generic; // para poder usar los diccionarios y listas

public static class GameData // static porque no tengo que ponerlo en ningun objeto
{
    // memoria de las preguntas
    public static int PreguntaActualID; // guarda el numero de la bandera
   
   // El diccionario es como una "agenda" en el número 1 guarda la respuesta "1", etc.
    public static Dictionary<int, string> respuestasEncuesta = new Dictionary<int, string>();
    // GPS del avion
    
    public static Vector3 posicionRetorno; // guarda las coordenadas donde se quedo
    public static bool regresarDeNubes = false;  // checa si vienes de ese juego o no

    // una lista de los números de las banderas que ya pasamos para que no se repitan
    public static List<int> banderasCompletadas = new List<int>();
    // checa lo de la puerta
    public static bool puertaAbierta = false;
    public static int plantasGuardadas = 0; // cuantas plantas lleva
    public static Vector3 ultimoCheckpointPos; // loc de la ultima bandera que toco
    public static bool tieneCheckpoint = false; // checa si ya toco minimo una
    public static bool yaVioInstrucciones = false;
}

// cuando unity carga una escena nueva normalmente borra todo lo que habia en la de antees, pero game data sobrevive a eso, para que las plantas no se pongan en 0 cuando regese