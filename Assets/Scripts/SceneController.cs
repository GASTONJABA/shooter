using UnityEngine;
using UnityEngine.SceneManagement;

// Este script maneja el cambio de escenas y el cierre del juego
public class SceneController : MonoBehaviour
{
	// Instancia única del SceneController (patrón Singleton)
	private static SceneController instance;

	// Este método se ejecuta al iniciar el objeto
	private void Awake()
	{
		// Si ya existe una instancia, destruimos esta para evitar duplicados
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}

		// Guardamos esta instancia como la única
		instance = this;

		// Hace que este objeto no se destruya al cambiar de escena
		DontDestroyOnLoad(gameObject);
	}

	// Carga una escena por su nombre
	public static void LoadScene(string sceneName)
	{
		Debug.Log("Loading scene: " + sceneName);
		SceneManager.LoadScene(sceneName);
	}

	// Cierra el juego (funciona tanto en el editor como en el build)
	public static void QuitGame()
	{
		Debug.Log("Quitting game...");
#if UNITY_EDITOR
		// Si estás en el editor, solo detiene el modo "Play"
		UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si estás en el juego compilado, lo cierra
        Application.Quit();
#endif
	}
}