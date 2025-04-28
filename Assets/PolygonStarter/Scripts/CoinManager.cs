using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int totalCoins; // N�mero total de monedas en la escena
    public TextMeshProUGUI gameOverText; // Texto de "Game Over"

    void Start()
    {
        // Encuentra todas las monedas que tengan el tag "Coin"
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;

        // Aseg�rate de que el texto de Game Over est� oculto al inicio
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    // M�todo para llamar cada vez que se recoge una moneda
    public void CoinCollected()
    {
        totalCoins--;

        if (totalCoins <= 0)
        {
            GameOver();
        }
    }

    // M�todo que muestra el texto de "Game Over"
    void GameOver()
    {

        Debug.Log("�Game Over activado!");

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }

        // (Opcional) Pausar el juego
        // Time.timeScale = 0;
    }
}


