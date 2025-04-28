using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int totalCoins; // N�mero total de monedas
    public TextMeshProUGUI gameOverText; // Referencia al texto "Game Over"

    void Start()
    {
        // Encuentra todas las monedas al iniciar
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        gameOverText.gameObject.SetActive(false); // Asegura que el texto est� oculto
    }

    // Este m�todo se llama cuando se recoge una moneda
    public void CoinCollected()
    {
        totalCoins--;

        if (totalCoins <= 0)
        {
            GameOver();
        }
    }

    // Mostrar el texto de Game Over
    void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        // Aqu� podr�as pausar el juego si quieres: Time.timeScale = 0;
    }
}

