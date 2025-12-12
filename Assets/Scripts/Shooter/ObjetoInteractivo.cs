using UnityEngine;

public class ObjetoInteractivo : MonoBehaviour
{
    [TextArea(3, 10)]
    public string mensajeDeInformacion = "Hay una nota ensangrentada aquí: 'No confíes en nadie.'";

    public float radioInteraccion = 2f;

    private bool jugadorCerca = false;
    private UIManagement uiManager; // Referencia al Manager de UI

    void Start()
    {
        // Busca el Manager de UI una sola vez al inicio.
        uiManager = FindObjectOfType<UIManagement>();
        if (uiManager == null)
        {
            Debug.LogError("Error: Necesitas un objeto con el script 'UIManagement' en la escena.");
        }
    }

    void Update()
    {
        DetectarJugador();

        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            MostrarInformacion();
        }
    }

    void DetectarJugador()
    {
        GameObject jugador = GameObject.FindWithTag("Player");

        if (jugador != null && uiManager != null)
        {
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);

            if (distancia <= radioInteraccion)
            {
                if (!jugadorCerca)
                {
                    // ¡Ahora muestra el prompt visualmente!
                    uiManager.MostrarPrompt();
                }
                jugadorCerca = true;
            }
            else
            {
                if (jugadorCerca)
                {
                    // ¡Oculta el prompt cuando se va!
                    uiManager.OcultarPrompt();
                }
                jugadorCerca = false;
            }
        }
    }

    void MostrarInformacion()
    {
        if (uiManager != null)
        {
            // Muestra el panel con el mensaje.
            uiManager.MostrarPanelDeInfo(mensajeDeInformacion);

            // Opcional: Ocultamos el prompt "Presiona E" mientras leen el mensaje
            uiManager.OcultarPrompt();
        }
    }
}