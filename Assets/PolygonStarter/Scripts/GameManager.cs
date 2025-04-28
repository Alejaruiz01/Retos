using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int totalCoins; // Número total de monedas
    public TextMeshProUGUI gameOverText; // Referencia al texto "Game Over"

    void Start()
    {
        // Encuentra todas las monedas al iniciar
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        gameOverText.gameObject.SetActive(false); // Asegura que el texto esté oculto
    }

    // Este método se llama cuando se recoge una moneda
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
        // Aquí podrías pausar el juego si quieres: Time.timeScale = 0;
    }
}

