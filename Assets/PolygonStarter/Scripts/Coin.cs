using UnityEngine;

public class Coin : MonoBehaviour
{
    public int points = 10; // Puntos que se otorgan al recolectar la moneda
    public ScoreManager scoreManager; // Referencia al script ScoreManager
    private CoinManager coinManager;
    public AudioSource coinSound;

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
            if (scoreManager != null)
            {
                scoreManager.AddPoints(points);
            }

            if (coinSound != null)
            {
                AudioSource.PlayClipAtPoint(coinSound.clip, transform.position, 0.2f);
            }
            Destroy(gameObject);


            if (coinManager != null)
            {
                coinManager.CoinCollected();
            }

        }
    }
}