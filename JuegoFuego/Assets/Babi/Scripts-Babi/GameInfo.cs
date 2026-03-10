using UnityEngine;
using System.Collections.Generic;

public static class GameData
{
    public static int PreguntaActualID;
    public static Dictionary<int, string> respuestasEncuesta = new Dictionary<int, string>();
    public static Vector3 posicionRetorno; 
    public static bool regresarDeNubes = false;

    // Nueva lista para guardar qué banderas ya se usaron
    public static List<int> banderasCompletadas = new List<int>();
    public static bool puertaAbierta = false;
    public static int plantasGuardadas = 0;
    public static Vector3 ultimoCheckpointPos;
    public static bool tieneCheckpoint = false;
}