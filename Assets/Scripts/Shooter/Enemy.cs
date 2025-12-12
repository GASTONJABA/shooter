using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI; // NECESARIO para usar el NavMesh

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform playerTarget; // Asigna el Transform del jugador en el Inspector
    public PlayerShooting PlayerShooting;
    public GameManager GameManager;
    private TextMeshProUGUI currentHealthText;
    public int vidas = 3;
    public int currentHealth;
    public int damage = 1;
    // Método llamado por el GameManager al hacer spawn
    public void SetGameManager(GameManager manager)
    {
        GameManager = manager;

    }


    // Start is called before the first frame update
    void Start()
    {
        // Obtiene el componente NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        currentHealth = vidas;
        // ⭐️ BÚSQUEDA AUTOMÁTICA DEL TEXTO
        // Busca el componente TextMeshPro en este objeto o en cualquiera de sus hijos.
        currentHealthText = GetComponentInChildren<TextMeshProUGUI>();

        if (currentHealthText == null)
        {
            // Esto te avisará si creaste el texto pero Unity no lo encuentra
            Debug.LogWarning("WARNING: El componente TextMeshPro no se encontró en los hijos del enemigo. La UI de vida no se mostrará.");
        }

        UpdateHealthUI(); // Muestra la vida inicial
    }

    // 1. Inicializa el texto de vida al empezar

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " golpeado. Vidas restantes: " + currentHealth);
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que lo impactó es el proyectil del jugador.

        // Podrías chequear el Tag del proyectil (ej: "PlayerProjectile") o su capa (Layer).
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {// 1. Intentamos obtener el script Enemy del objeto golpeado
            Enemy enemyHit = collision.gameObject.GetComponent<Enemy>();

            const int fixedDamage = 1;
            TakeDamage(fixedDamage);
            if (enemyHit != null)
            {
                // 2. Si tiene el script Enemy, llamamos a TakeDamage()
                enemyHit.TakeDamage(damage);

                // ⭐️ Usamos la información almacenada:
                // Dado que el usuario pidió almacenar "muerte enemiga",
                // esto refuerza la importancia de este evento.
                Debug.Log("Impacto registrado: " + collision.gameObject.name + ". ¡Buscando Muerte Enemiga!");
            }
            // 1. Destruye el objeto proyectil (para que no siga rebotando).
            Destroy(collision.gameObject);

            // 2. Destruye la caja enemiga.
            //Die();
        }

    }

    private void UpdateHealthUI()
    {
        if (currentHealthText != null)
        {
            // Convierte el número entero (currentHealth) a una cadena de texto
            currentHealthText.text = currentHealth.ToString();
        }
    }
    void Die()
    {// 1. Notificar al GameManager ANTES de destruirse.
        if (GameManager != null)
        {
            GameManager.EnemyDefeated();
        }

        // Lógica de "muerte"(por ahora, solo borrar)
        Destroy(gameObject);
    }


    // Opcional: Podrías hacer un efecto de partículas aquí (Instantiate(explosionPrefab, ...))

    // **IMPORTANTE para el cambio de escena:**
    // Notificar al Game Manager que un enemigo murió.
    // GameController.EnemyKilled();
    // Update is called once per frame
    void Update()
    {
        // Si el objetivo (jugador) existe, establece su posición como destino
        if (playerTarget != null)
        {
            agent.SetDestination(playerTarget.position);
        }
    }
}
