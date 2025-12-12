using UnityEngine;
using TMPro;

public class UIManagement : MonoBehaviour
{
    public GameObject panelDeInformacion;
    public TextMeshProUGUI textoDeInformacion;

    // Agrega una variable para el prompt "Presiona E"
    public GameObject promptE; // Nuevo: El objeto de texto que dice "Presiona E"

    void Update()
    {
        // Si el panel de información está activo (mostrando el mensaje)
        // y el jugador presiona ESCAPE, Ocultamos el panel y reanudamos el tiempo.
        if (panelDeInformacion.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            OcultarPanelDeInfo();
            // Despausar el juego si lo habías pausado
            // Time.timeScale = 1f; 
        }
    }

    public void MostrarPanelDeInfo(string mensaje)
    {
        textoDeInformacion.text = mensaje;
        panelDeInformacion.SetActive(true);
        // Opcional: Pausar el juego para que el jugador pueda leer tranquilo
        // Time.timeScale = 0f; 
    }

    public void OcultarPanelDeInfo()
    {
        panelDeInformacion.SetActive(false);
        // Opcional: Reanudar el juego
        // Time.timeScale = 1f; 
    }

    // Funciones para manejar el prompt de interacción
    public void MostrarPrompt()
    {
        if (promptE != null) promptE.SetActive(true);
    }

    public void OcultarPrompt()
    {
        if (promptE != null) promptE.SetActive(false);
    }
}