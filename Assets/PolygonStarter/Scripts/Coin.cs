using UnityEngine;

public class Coin : MonoBehaviour
{
    public int points = 10; // Puntos que se otorgan al recolectar la moneda
    public ScoreManager scoreManager; // Referencia al script ScoreManager
    private GameManager gameManager;

    void Start()
    {
        // Buscar el GameManager en la escena
        gameManager = FindObjectOfType<GameManager>();
    }
    // Este método se llama cuando el jugador toca la moneda
    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto con el que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            scoreManager.AddPoints(points); // Añade puntos al contador
            Destroy(gameObject); // Destruye la moneda después de recogerla
        }
    }
}

