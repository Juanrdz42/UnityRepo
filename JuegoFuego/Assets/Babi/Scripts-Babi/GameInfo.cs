using System.Collections.Generic;

public static class GameData
{
    public static int PreguntaActualID;
    
    // Aquí se guardan las respuestas. 
    // Ejemplo: En la bandera 1 respondió "1-5 años"
    public static Dictionary<int, string> respuestasEncuesta = new Dictionary<int, string>();
}