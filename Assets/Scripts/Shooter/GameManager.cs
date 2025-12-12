using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{   public PlayerShooting PlayerShooting;   
    public Enemy Enemy;
    public  int cantidadEnemigos =5;
    private int enemigosDerrotados =0;
    // Asegúrate de que este es el prefab del enemigo, no una instancia en escena.
    public GameObject EnemyPrefab;
    public string nextSceneName = "Nivel2";
   
    // Referencia a la instancia actual del enemigo
    private GameObject currentEnemyInstance;
    // Lista de posiciones de spawn que configurarás en el Inspector
    public List<Transform> spawnPositions;

    public static GameManager instance; // ⭐️ La variable estática que contendrá la única instancia

    void Awake()
    {
        if (instance == null)
        {
            // 1. Asigna esta instancia si es la primera vez que se carga
            instance = this;

            // 2. ¡El comando mágico! Mantiene el objeto vivo al cargar nuevas escenas.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 3. Si ya existe una instancia (porque ya pasamos de escena), destruye esta nueva.
            Destroy(gameObject);
        }
    }




    void Start()
    {
        // Asegurarse de que tenemos posiciones y el prefab antes de empezar
        if (EnemyPrefab == null || spawnPositions.Count == 0)
        {
            Debug.LogError("Falta asignar el Prefab del Enemigo o las Posiciones de Spawn en el Inspector.");
            return;
        }

        // Inicia el juego haciendo spawn del primer enemigo
        SpawnNextEnemy();
    }

    /// <summary>
    /// Hace spawn del siguiente enemigo si hay posiciones disponibles.
    /// </summary>
    private void SpawnNextEnemy()
    {
        // Verifica si todavía quedan enemigos por aparecer en la secuencia
        if (enemigosDerrotados < spawnPositions.Count && enemigosDerrotados < cantidadEnemigos)
        {
            // 1. Obtiene la posición de la lista (el índice es igual a los derrotados)
            Transform spawnPoint = spawnPositions[enemigosDerrotados];

            // 2. Instancia el prefab en la posición y rotación del punto de spawn
            currentEnemyInstance = Instantiate(
                EnemyPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            // 3. Opcional: Obtener el script del enemigo instanciado
            Enemy enemyScript = currentEnemyInstance.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                // **Paso Crítico:** Pasar la referencia del GameManager al enemigo
                // para que pueda notificar cuando muera.
                enemyScript.SetGameManager(this);
            }
        }
        else if (enemigosDerrotados >= cantidadEnemigos)
        {
            // ¡Has derrotado a los 5 enemigos!
            Debug.Log("🎉 ¡VICTORIA! Todos los enemigos han sido derrotados.");
            // Aquí puedes añadir la lógica de "Ganar" (cargar otra escena, mostrar UI, etc.)
        }
    }

    /// <summary>
    /// Método público llamado por el script del enemigo cuando es derrotado.
    /// </summary>
    public void EnemyDefeated()
    {
        enemigosDerrotados++;
        Debug.Log("Enemigo Derrotado. Total: " + enemigosDerrotados);

        // Intenta hacer spawn del siguiente enemigo
        SpawnNextEnemy();
    
    if (enemigosDerrotados >= cantidadEnemigos)
        {
            // Detenemos la lógica del juego (por ejemplo, dejar de hacer spawn)
            // StopSpawning(); // Si tienes un método para esto
            LoadNextLevelByOrder(); // Usamos la función mejorada por índice
            //LoadNextScene();
        }
    }
    // **Función MEJORADA (que usa el índice):**
    public void LoadNextLevelByOrder() // Cambia el nombre para diferenciarla
    {
        // Obtiene el índice de la escena actual (Ej: 2 si estamos en Nivel2)
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calcula el índice de la siguiente escena (Ej: 3 para Nivel3)
        int nextSceneIndex = currentSceneIndex + 1;

        // Comprueba si hay más escenas en el Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("🎉 ¡Objetivo completado! Cargando escena con índice: " + nextSceneIndex);
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("¡No hay más niveles después de este! Juego completado.");
            // Opcional: Cargar la escena de "Créditos" (índice 0 o una escena específica)
        }
    }
    //private void LoadNextScene()
    //{
       // Debug.Log("¡Objetivo completado! Cargando la siguiente escena: " + nextSceneName);
        
        // ⭐️ Carga la escena por su nombre
       // SceneManager.LoadScene(nextSceneName);
        
        // **IMPORTANTE**: La escena 'nextSceneName' DEBE estar en Build Settings.
   // }

    // ... el resto del GameManager ...
}

