using UnityEngine;

public class Coin : MonoBehaviour
{
    public int points = 10; // Puntos que se otorgan al recolectar la moneda
    public ScoreManager scoreManager; // Referencia al script ScoreManager
    private CoinManager coinManager;

    void Start()
    {
        // Buscar el GameManager en la escena
        coinManager = FindObjectOfType<CoinManager>();
    }
    // Este método se llama cuando el jugador toca la moneda
    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto con el que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            // Sumar los puntos
            if (scoreManager != null)
            {
                scoreManager.AddPoints(points);
            }

            // Avisar al CoinManager que una moneda fue recogida
            if (coinManager != null)
            {
                coinManager.CoinCollected();
            }

            // Destruir la moneda (después de un pequeño retraso si quieres que suene completo el audio)
            Destroy(gameObject);
        }
    }
}

