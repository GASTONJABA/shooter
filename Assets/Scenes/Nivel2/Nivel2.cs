using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nivel2 : MonoBehaviour

{
    private int enemigosRestantes;
    public string siguienteNivel = "nivel3"; // O "PantallaDeVictoria"

    void Start()
    {
        // Esto se ejecutará al cargar el Nivel 2, donde tienes 1 enemigo.
        enemigosRestantes = 1;
    }

    // Esta función es llamada por el script del enemigo cuando muere
    public void EnemigoDerrotado()
    {
        enemigosRestantes--;
        Debug.Log("Enemigos restantes: " + enemigosRestantes);

        if (enemigosRestantes <= 0)
        {
            // ¡Condición de victoria! El nivel ha terminado.
            FinNivel();
        }
    }

    void FinNivel()
    {
        // Aquí pones la lógica para terminar el nivel:
        Debug.Log("¡Nivel 2 Completo!");

        // 1. Mostrar un mensaje de victoria.
        // 2. Detener el tiempo o la acción del jugador.
        // 3. Cargar el siguiente nivel o la pantalla de victoria.

        // Ejemplo de carga de escena (asegúrate de incluir 'using UnityEngine.SceneManagement;'):
         SceneManager.LoadScene("nivel3");
    }
}